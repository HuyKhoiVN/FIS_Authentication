using System;
using System.Collections.Generic;

namespace appData.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string? AccountCode { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Photo { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public bool? Active { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }

        public virtual Department? Department { get; set; } = null!;
        public virtual Role? Role { get; set; } = null!;
    }
}
