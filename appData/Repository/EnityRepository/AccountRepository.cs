using appData.Constants;
using appData.Models;
using appData.ModelsDTO;
using appData.Repository.Interface;
using appData.Utils.Enitties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.Repository.EnityRepository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(FIS_AuthenContext dbContext) : base(dbContext)
        {
        }

        public async Task<Account> GetEmployeeByUserPassword(string username, string password)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(e => e.Username == username && e.Password == password);
        }

        public async Task<bool> IsUserNameExist(string username)
        {
            return await _dbContext.Accounts.AnyAsync(e => e.Username == username);
        }

        public async Task<DTResult<AccountDTO>> ListServerSide(EntityDTParameters parameters)
        {
            //0. Options
            string searchAll = parameters.SearchAll.Trim();//Trim text
            string orderCritirea = "Id";//Set default critirea
            int recordTotal, recordFiltered;
            bool orderDirectionASC = true;//Set default ascending
            if (parameters.Order != null)
            {
                orderCritirea = parameters.Columns[parameters.Order[0].Column].Data;
                orderDirectionASC = parameters.Order[0].Dir == DTOrderDir.ASC;
            }
            //1. Join (join table role and department for getting roleName and departmentName)
            var query = from row in _dbContext.Accounts
                        join role in _dbContext.Roles on row.RoleId equals role.Id
                        join department in _dbContext.Departments on row.DepartmentId equals department.Id

                        where row.Active == true && row.RoleId == role.Id

                        select new
                        {
                            row,
                            role,
                            department
                        };

            recordTotal = await query.CountAsync();
            //2. Fillter by 
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c =>
                            EF.Functions.Collate(c.row.AccountCode.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                            EF.Functions.Collate(c.row.Username.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                            EF.Functions.Collate(c.row.FullName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                            EF.Functions.Collate(c.row.Email.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                            EF.Functions.Collate(c.row.PhoneNumber.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                            EF.Functions.Collate(c.role.RoleName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))
                );
            }
            foreach (var item in parameters.Columns)
            {
                var fillter = item.Search.Value.Trim();
                if (fillter.Length > 0)
                {
                    switch (item.Data)
                    {
                        case "id":
                            query = query.Where(c => c.row.Id.ToString().Trim().Contains(fillter));
                            break;
                        case "accountCode":
                            query = query.Where(c => (c.row.AccountCode ?? "").Contains(fillter));
                            break;
                        case "username":
                            query = query.Where(c => (c.row.Username ?? "").Contains(fillter));
                            break;
                        case "password":
                            query = query.Where(c => (c.row.Password ?? "").Contains(fillter));
                            break;
                        case "fullName":
                            query = query.Where(c => (c.row.FullName ?? "").Contains(fillter));
                            break;
                        case "email":
                            query = query.Where(c => (c.row.Email ?? "").Contains(fillter));
                            break;
                        case "photo":
                            query = query.Where(c => (c.row.Photo ?? "").Contains(fillter));
                            break;
                        case "dOB":
                            if (fillter.Contains(" - "))
                            {
                                var dates = fillter.Split(" - ");
                                var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                query = query.Where(c => c.row.Dob >= startDate && c.row.Dob <= endDate);
                            }
                            else
                            {
                                var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                query = query.Where(c => c.row.Dob.Date == date.Date);
                            }
                            break;
                        case "phoneNumber":
                            query = query.Where(c => (c.row.PhoneNumber ?? "").Contains(fillter));
                            break;
                        case "active":
                            query = query.Where(c => c.row.Active.ToString().Trim().Contains(fillter));
                            break;
                        case "roleId":
                            query = query.Where(c => c.row.RoleId.ToString().Trim().Contains(fillter));
                            break;
                        case "departmentId":
                            query = query.Where(c => c.row.DepartmentId.ToString().Trim().Contains(fillter));
                            break;
                        case "createdTime":
                            if (fillter.Contains(" - "))
                            {
                                var dates = fillter.Split(" - ");
                                var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                query = query.Where(c => c.row.CreatedTime >= startDate && c.row.CreatedTime <= endDate);
                            }
                            else
                            {
                                var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                query = query.Where(c => c.row.CreatedTime.Date == date.Date);
                            }
                            break;

                    }
                }
            }

            //3.Query second
            var query2 = query.Select(c => new AccountDTO()
            {
                Id = c.row.Id,
                AccountCode = c.row.AccountCode,
                Username = c.row.Username,
                Password = c.row.Password,
                FullName = c.row.FullName,
                Email = c.row.Email,
                Photo = c.row.Photo,
                Dob = c.row.Dob,
                PhoneNumber = c.row.PhoneNumber,
                Active = c.row.Active == true,
                RoleId = c.row.RoleId,
                CreatedTime = c.row.CreatedTime,
                RoleName = c.role.RoleName,
                DepartmentId = c.row.DepartmentId,
                DepartmentName = c.department.DepartmentName
            });
            //4. Sort
            query2 = query2.OrderByDynamic<AccountDTO>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = query2.Count();
            //5. Return data
            return new DTResult<AccountDTO>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
    }
}
