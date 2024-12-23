using appData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appAPI.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Service GetAll
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Service GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Service Create
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(T entity)
        {
            Validate(entity);
            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// Service Update
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(int entityId, T entity)
        {
            Validate(entity, entityId);
            return await _repository.UpdateAsync(entityId, entity);
        }

        /// <summary>
        /// Service Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Service SoftDelete
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task<bool> SoftDeleteAsync(int entityId)
        {
            return await _repository.SoftDeleteAsync(entityId);
        }

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<T> Items, int TotalCount)> GetPagingAsync(int pageNumber, int pageSize)
        {
            return await _repository.GetPagingAsync(pageNumber, pageSize);
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        protected virtual void Validate(T entity, int? entityId = null)
        {

        }
    }
}
