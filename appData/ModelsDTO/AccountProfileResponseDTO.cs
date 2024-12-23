using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.ModelsDTO
{
    public class AccountProfileResponseDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string FullName { get; set; } = null!;
        public string? Photo { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; } = null!;
    }
}
