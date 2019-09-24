using KuuhakuFramework.Web.Models.Configuration;
using KuuhakuFramework.Web.Models.Security.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuuhakuFramework.Web.Models
{
    public class KuuhakuContext : IdentityDbContext<Identity, Role, int, IdentityClaim, IdentityRole, IdentityLogin, RoleClaim, IdentityToken>
    {
        public KuuhakuContext(DbContextOptions options) : base(options) { }

        public DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SetIdentityTables(builder);
            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new MenuRoleConfiguration());



        }


        private void SetIdentityTables(ModelBuilder builder)
        {
            builder.Entity<Identity>().ToTable("Security_IdentityUser");
            builder.Entity<IdentityClaim>().ToTable("Security_UserClaim");
            builder.Entity<IdentityLogin>().ToTable("Security_UserLogin");
            builder.Entity<IdentityRole>().ToTable("Security_UserRole");
            builder.Entity<IdentityToken>().ToTable("Security_UserToken");
            builder.Entity<Role>().ToTable("Security_Role");
            builder.Entity<RoleClaim>().ToTable("Security_RoleClaim");
        }

    }
}
