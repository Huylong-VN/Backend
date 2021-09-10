using Microsoft.AspNetCore.Identity;
using System;

namespace Backend.Data.Entity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { set; get; }
        public string Address { set; get; }

        public string ImagePath { set; get; }
    }
}