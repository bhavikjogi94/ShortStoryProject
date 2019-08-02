using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShortStoryBOL;
using System;

namespace DAL
{
    public class SSDbContext : IdentityDbContext


    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
            optionBuilder.UseSqlServer(@"Server=CATO-BJOGI\SQLEXPRESS;Database=SSAngular8Db;Trusted_Connection=True;");
        }

        public DbSet<Story> Stories { get; set; }
    }
}
