﻿using Domian.Common;
using System;

namespace Domian.Entities.Accounts
{
    public class UserRole : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
