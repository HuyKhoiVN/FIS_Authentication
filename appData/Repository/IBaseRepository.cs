using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.Repository
{
    /// <summary>
    /// Base Repository: CRUD
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Lấy tất cả item
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Lấy phân trang
        /// </summary>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="pageSize">Số item/trang</param>
        /// <returns></returns>
        Task<(List<T> Items, int TotalCount)> GetPagingAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Lấy item theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Tạo mới item
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(T entity);

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(int entityId, T entity);

        /// <summary>
        /// Xoá item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Soft Del
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Task<bool> SoftDeleteAsync(int entityId);
    }
}
