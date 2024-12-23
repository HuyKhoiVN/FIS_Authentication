using appData.Models;
using appAPI.Service;
using appAPI.Service.Interface;
using appData.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using appData.Utils.Enitties;

namespace appAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : BaseController<Account>
    {
        private readonly IAccountService _accountService;

        public AccountController(IBaseService<Account> baseService, IAccountService accountService) : base(baseService)
        {
            _accountService = accountService;
        }

        [HttpGet("getFullInfor")]
        public async Task<IActionResult> GetEmployeesFullInfor(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (items, totalCount) = await _accountService.GetAccountsFullInfor(pageNumber, pageSize);

                // Tạo đối tượng response bao gồm dữ liệu và tổng số bản ghi
                var response = new
                {
                    data = items,
                    totalCount = totalCount,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return Ok(response);
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Account request)
        {
            try
            {
                var isRegistered = await _accountService.RegisterAccountAsync(request);
                if (isRegistered)
                {
                    var response = new
                    {
                        devMsg = "Registration successful!",
                        userMsg = "Registration successful!",
                        data = request
                    };
                    return Ok(response);
                }
                return BadRequest(
                    new
                    {
                        devMsg = "Registration fail!",
                        userMsg = "Registration fail!",
                        data = request
                    }
                    );
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

        [HttpPost]
        [Route("list-server-side")]
        public async Task<IActionResult> ListServerSide([FromBody] EntityDTParameters parameters)
        {
            try
            {
                var data = await _accountService.ListServerSide(parameters);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        #region doc for listServerSide
        // Json input:
        /*
         {
  "draw": 1,
  "columns": [
    {
      "data": "id",
      "name": "ID",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "accountCode",
      "name": "Account Code",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "username",
      "name": "Username",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "fullName",
      "name": "Full Name",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "email",
      "name": "Email",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "dob",
      "name": "Date of Birth",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "phoneNumber",
      "name": "Phone Number",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "createdTime",
      "name": "Created Time",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "active",
      "name": "Active",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "roleName",
      "name": "Role Name",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    },
    {
      "data": "departmentName",
      "name": "Department Name",
      "searchable": true,
      "orderable": true,
      "visible": true,
      "search": {
        "value": "",
        "regex": true
      }
    }
  ],
  "order": [
    {
      "column": 0,
      "dir": "asc"
    }
  ],
  "start": 0,
  "length": 10,
  "search": {
    "value": "",
    "regex": true
  },
  "additionalValues": [],
  "searchAll": ""
}

         */

        // json output:
        /*
            {
      "draw": 1,
      "recordsTotal": 2,
      "recordsFiltered": 2,
      "data": []     
         */
        #endregion
    }
}
