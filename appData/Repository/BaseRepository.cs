using appData.Models;
using appData.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        /// <summary>
        /// db
        /// </summary>
        protected readonly FIS_AuthenContext _dbContext;

        /// <summary>
        /// Db set table
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(FIS_AuthenContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }


        /// <summary>
        /// Kiểm tra thuộc tính active của obj
        /// Lấy obj mà active true hoặc lấy toàn bộ nếu obj không có active
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync()
        {
            // Kiểm tra xem entity có thuộc tính "Active" không
            var entityType = typeof(T);
            var activeProperty = entityType.GetProperty("Active");

            // Nếu có thuộc tính "Active", lọc những đối tượng có Active == true
            if (activeProperty != null && activeProperty.PropertyType == typeof(bool))
            {
                var data = await _dbSet.Where(entity => EF.Property<bool>(entity, "Active") == true).ToListAsync();
                return data;
            }
            else
            {
                // Nếu không có thuộc tính "Active", lấy toàn bộ đối tượng
                var data = await _dbSet.ToListAsync();
                return data;
            }
        }

        /// <inheritdoc/>
        public async Task<(List<T> Items, int TotalCount)> GetPagingAsync(int pageNumber, int pageSize)
        {
            // Kiểm tra xem entity có thuộc tính "Active" không
            var entityType = typeof(T);
            var activeProperty = entityType.GetProperty("Active");

            IQueryable<T> query = _dbSet;

            // Nếu có thuộc tính "Active", lọc những đối tượng có Active == true
            if (activeProperty != null && activeProperty.PropertyType == typeof(bool))
            {
                query = query.Where(entity => EF.Property<bool>(entity, "Active") == true);
            }

            // Đếm tổng số bản ghi trước khi phân trang
            int totalCount = await query.CountAsync();

            // Thực hiện phân trang: lấy số phần tử tương tứng pageSize mà được skip từ phần tử thứ: (pageNumber - 1) * pageSize
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Trả về danh sách đã phân trang cùng với tổng số bản ghi
            return (items, totalCount);
        }


        /// <inheritdoc/>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Lấy tất cả property, duyệt từng property, nếu là Active, CreatedTime thì set giá trị mặc định
        /// Thêm vào db
        /// Save
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                var entityProps = entity.GetType().GetProperties();
                foreach (var prop in entityProps)
                {
                    var propName = prop.Name;

                    // Set giá trị mặc định cho Status
                    if (propName == "Active" && prop.PropertyType == typeof(bool))
                    {
                        prop.SetValue(entity, true);
                    }

                    // Set giá trị mặc định cho DateTime
                    if (propName == "CreatedTime" &&
                        (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?)))
                    {
                        prop.SetValue(entity, DateTime.Now);
                    }
                }

                await _dbSet.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.InnerException
                };
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(int entityId, T entity)
        {
            try
            {
                var existingEntity = await _dbSet.FindAsync(entityId);
                if (existingEntity == null)
                    return false;

                var entityProps = entity.GetType().GetProperties();
                foreach (var prop in entityProps)
                {
                    var propName = prop.Name;

                    // Bỏ qua trường Id
                    if ((propName == "Id" && prop.PropertyType == typeof(int)) || (propName == "EmployeeId" && prop.PropertyType == typeof(int)) ||
                        (propName == "CreatedTime") ||
                        (propName == "Active"))
                    {
                        // Set lại giá trị ban đầu của thuộc tính
                        var originalValue = prop.GetValue(existingEntity);
                        prop.SetValue(entity, originalValue);
                    }
                }

                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // Tìm đối tượng bằng FindAsync,
                var entity = await _dbSet.FindAsync(id);

                // không tồn tại -> flase
                if (entity == null)
                {
                    return false;
                }
                // Tồn tại đối tượng -> xoá theo async, return true
                _dbSet.Remove(entity);
                return await _dbContext.SaveChangesAsync() > 0;
                //return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Xoá mềm, chuyển Active sang false
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        public async Task<bool> SoftDeleteAsync(int entityId)
        {
            try
            {
                var existingEntity = await _dbSet.FindAsync(entityId);
                if (existingEntity == null)
                    return false;

                var activeProperty = existingEntity.GetType().GetProperty("Active"); // Get Active
                if (activeProperty != null && activeProperty.PropertyType == typeof(bool))
                {
                    activeProperty.SetValue(existingEntity, false); // Update Active
                }
                else
                {
                    throw new ValidateException("Xoá không thành công, thuộc tính Active không tồn tại");
                }

                _dbContext.Entry(existingEntity).State = EntityState.Modified;

                // số dòng bị ảnh hưởng > 0 true, <= 0 false
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
