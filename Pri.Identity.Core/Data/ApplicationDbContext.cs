using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pri.Identity.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Identity.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string AdminRoleId = "00000000-0000-0000-0000-000000000001";
            const string AdminRoleName = "Admin";

            const string AdminUserId = "00000000-0000-0000-0000-000000000001";
            const string AdminUserName = "admin@programmingintegration.be";
            const string AdminUserPassword = "Test123?"; // For demo purposes only! Don't do this in real application!

            IPasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser adminApplicationUser = new ApplicationUser
            {
                Id = AdminUserId,
                UserName = AdminUserName,
                NormalizedUserName = AdminUserName.ToUpper(),
                Email = AdminUserName,
                NormalizedEmail = AdminUserName.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "VVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "c8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
                City = "Brugge",
            };

            adminApplicationUser.PasswordHash = passwordHasher.HashPassword(adminApplicationUser, AdminUserPassword);

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = AdminRoleId,
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper()
            });

            builder.Entity<ApplicationUser>().HasData(adminApplicationUser);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = AdminRoleId,
                UserId = AdminUserId
            });
        }
    }

}

