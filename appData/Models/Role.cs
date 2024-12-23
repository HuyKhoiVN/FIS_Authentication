﻿using System;
using System.Collections.Generic;

namespace appData.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string? RoleCode { get; set; }
        public string RoleName { get; set; } = null!;
        public bool? Active { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
