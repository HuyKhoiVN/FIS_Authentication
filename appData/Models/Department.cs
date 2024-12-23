using System;
using System.Collections.Generic;

namespace appData.Models
{
    public partial class Department
    {
        public Department()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string? DepartmentCode { get; set; }
        public string DepartmentName { get; set; } = null!;
        public bool? Active { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
