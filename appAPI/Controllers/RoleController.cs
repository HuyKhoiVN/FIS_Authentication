using appData.Models;
using appAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace appAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController : BaseController<Role>
    {
        public RoleController(IBaseService<Role> baseService) : base(baseService)
        {
        }
    }
}
