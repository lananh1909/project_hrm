using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HRM.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Web.Infrastructure
{
    public class HRMDbContext: IdentityDbContext<ApplicationUser>
    {
        public HRMDbContext(DbContextOptions<HRMDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public DbSet<UserInfo> UserInfos { get; set; }
    }
}
