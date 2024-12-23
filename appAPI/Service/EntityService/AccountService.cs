using appAPI.Service.Interface;
using appData.Constants;
using appData.Models;
using appData.ModelsDTO;
using appData.Repository;
using appData.Repository.EnityRepository;
using appData.Repository.Interface;
using appData.Utils;
using appData.Utils.Enitties;
using appData.Utils.Enitty;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace appAPI.Service.EntityService
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(IBaseRepository<Account> repository, IBaseRepository<Role> roleRepository, 
            IBaseRepository<Department> departmentRepository, IAccountRepository accountRepository,
            IConfiguration configuration, ITokenService tokenService, IMapper mapper) : base(repository)
        {
            _roleRepository = roleRepository;
            _departmentRepository = departmentRepository;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<(List<AccountDTO> Items, int TotalCount)> GetAccountsFullInfor(int pageNumber, int pageSize)
        {
            var (accounts, totalCount) = await _repository.GetPagingAsync(pageNumber, pageSize);

            var accountDtoList = new List<AccountDTO>();

            foreach (var account in accounts)
            {
                var department = await _departmentRepository.GetByIdAsync(account.DepartmentId);
                var role = await _roleRepository.GetByIdAsync(account.RoleId);

                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    AccountCode = account.AccountCode,
                    Username = account.Username,
                    FullName = account.FullName,
                    Password = account.Password,
                    Email = account.Email,
                    Photo = account.Photo,
                    Dob = account.Dob,
                    PhoneNumber = account.PhoneNumber,
                    RoleId = account.RoleId,
                    DepartmentId = account.DepartmentId,
                    RoleName = role.RoleName,
                    DepartmentName = department.DepartmentName,
                    CreatedTime = account.CreatedTime,
                    Active = account.Active
                };

                accountDtoList.Add(accountDTO);
            }

            return (accountDtoList, totalCount);
        }

        public async Task<bool> RegisterAccountAsync(Account request)
        {
            // Kiểm tra email trùng lặp
            if (await _accountRepository.IsUserNameExist(request.Username))
            {
                throw new ValidateException("Email already exists.");
            }

            var hashedPassword = HashPassword(request.Password);

            // Tạo đối tượng Employee
            var employee = new Account
            {
                AccountCode = request.AccountCode,
                Username= request.Username,
                FullName = request.FullName,
                Password = hashedPassword,
                Email = request.Email,
                Photo = request.Photo,
                Dob = request.Dob,
                PhoneNumber = request.PhoneNumber,
                RoleId = request.RoleId,
                DepartmentId = request.DepartmentId,
                CreatedTime= DateTime.Now.ToLocalTime(),
                Active = true
            };

            // Lưu vào cơ sở dữ liệu
            await _accountRepository.CreateAsync(employee);
            return true;
        }

        public async Task<Account> ValidateUser(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            return await _accountRepository.GetEmployeeByUserPassword(username, hashedPassword);
        }

        public async Task<FISAuthenticationResponse> LoginService(LoginRequest model)
        {
            // trim
            model.Username = model.Username.Trim();
            model.Password = model.Password.Trim();

            var hashedPassword = HashPassword(model.Password);
            if (model.Username.Length == 0 || model.Password.Length == 0)
            {
                return FISAuthenticationResponse.BAD_REQUEST();
            }
            var data = await _accountRepository.GetEmployeeByUserPassword(model.Username, hashedPassword);

            if (data == null)
            {
                return FISAuthenticationResponse.Failed("404", "Tài khoản không tìm thấy", null);
            }
            else if (data.Password == HashPassword(model.Password))
            {
                string accessToken = await GenerateJwtToken(data);

                data.Role = await _roleRepository.GetByIdAsync(data.RoleId);
                
                var account = _mapper.Map<AccountProfileResponseDTO>(data);
                return FISAuthenticationResponse.Success(new SignInResponse()//Change resources
                {
                    AccessToken = accessToken,
                    Profile = account
                });
            }
            else
            {
                return FISAuthenticationResponse.BAD_REQUEST();
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private async Task<string> GenerateJwtToken(Account user)
        {
            var positionName = (await _roleRepository.GetByIdAsync(user.RoleId)).RoleName;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, positionName)
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<DTResult<AccountDTO>> ListServerSide(EntityDTParameters parameters)
        {
            return await _accountRepository.ListServerSide(parameters);
        }
    }
}
