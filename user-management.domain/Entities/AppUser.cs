﻿using Microsoft.AspNetCore.Identity;

namespace user_management.domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public bool IsTenantStaff { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefereshTokenExpiry { get; set; }
        public Address Address { get; set; }
    }
}