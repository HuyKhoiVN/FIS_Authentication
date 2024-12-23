using appData.Constants;
using appData.Models;
using appData.ModelsDTO;
using appData.Utils.Enitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.Repository.Interface
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<Account> GetEmployeeByUserPassword(string username, string password);

        Task<bool> IsUserNameExist(string username);

        Task<DTResult<AccountDTO>> ListServerSide(EntityDTParameters parameters);
    }
}
