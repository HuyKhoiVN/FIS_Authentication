using appData.Constants;
using appData.Models;
using appData.ModelsDTO;
using appData.Utils;
using appData.Utils.Enitties;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appAPI.Service.Interface
{
    public interface IAccountService : IBaseService<Account>
    {
        Task<(List<AccountDTO> Items, int TotalCount)> GetAccountsFullInfor(int pageNumber, int pageSize);

        Task<Account> ValidateUser(string username, string password);

        Task<bool> RegisterAccountAsync(Account request);
        Task<FISAuthenticationResponse> LoginService(LoginRequest loginRequest);

        Task<DTResult<AccountDTO>> ListServerSide(EntityDTParameters parameters);
    }
}
