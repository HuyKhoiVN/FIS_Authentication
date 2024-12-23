using appAPI.Service;
using appData.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace appAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected IBaseService<T> _baseService;

        public BaseController(IBaseService<T> baseService)
        {
            _baseService = baseService;
        }

        #region Methods

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var dataList = await _baseService.GetAllAsync();
                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }
                var res = FISAuthenticationResponse.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(res);
            }
            catch (ValidateException ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.Data
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.Data
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Lấy dữ liệu theo phân trang
        /// </summary>
        /// <param name="pageNumber">số trnag</param>
        /// <param name="pageSize">số phần tử</param>
        /// <returns>
        /// 200 - thành công
        /// 400 - lỗi k hợp lệ
        /// 500 - lỗi hệ thống
        /// </returns>
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Lấy dữ liệu phân trang từ service
                var (items, totalCount) = await _baseService.GetPagingAsync(pageNumber, pageSize);

                // Tạo đối tượng response bao gồm dữ liệu và tổng số bản ghi
                var response = new
                {
                    items = items,
                    totalCount = totalCount,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
                var res = FISAuthenticationResponse.SUCCESS(response);

                return Ok(res);
            }
            catch (ValidateException ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.Data
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu.",
                    data = ex.InnerException
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// 200 - thành công
        /// 400 - lỗi k hợp lệ
        /// 500 - lỗi hệ thống
        /// </returns>
        [HttpGet("{entityId}")]
        public async Task<IActionResult> GetById(int entityId)
        {
            try
            {
                var dataList = await _baseService.GetByIdAsync(entityId);
                if (dataList == null)
                {
                    return NotFound();
                }
                var res = FISAuthenticationResponse.SUCCESS(dataList);
                return Ok(res);
            }
            catch (ValidateException ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.Data
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu.",
                    data = ex.InnerException
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Thêm entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// 201 - thêm thành công
        /// 400 - lỗi không hợp lệ
        /// 500 - lỗi hệ thống
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] T entity)
        {
            try
            {
                var data = await _baseService.CreateAsync(entity);
                var res = FISAuthenticationResponse.CREATED(entity);
                return Created("", res);
                //return StatusCode(201, data);
            }
            catch (ValidateException ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = entity
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.InnerException
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpPut("{entityId}")]
        public async Task<IActionResult> Put([FromRoute] int entityId, [FromBody] T entity)
        {
            try
            {
                var data = await _baseService.UpdateAsync(entityId, entity);
                var res = FISAuthenticationResponse.SUCCESS(entity);
                return Ok(res);
            }
            catch (ValidateException ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = entity
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.InnerException
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpDelete("{entityId}")]
        public async Task<IActionResult> Delete(int entityId)
        {
            try
            {
                var data = await _baseService.DeleteAsync(entityId);
                var res = FISAuthenticationResponse.SUCCESS(entityId);
                return Ok(res);
            }
            catch (ValidateException ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.Data
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.InnerException
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpPut("softDelete/{entityId}")]
        public async Task<IActionResult> SoftDelete([FromRoute] int entityId)
        {
            try
            {
                var data = await _baseService.SoftDeleteAsync(entityId);
                return Ok(data);
            }
            catch (ValidateException ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = entityId
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                    data = ex.InnerException
                };
                return StatusCode(500, response);
            }
        }
        #endregion
    }
}
