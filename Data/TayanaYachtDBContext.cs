using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Data
{
    public partial class TayanaYachtDBContext : DbContext
    {
        public TayanaYachtDBContext()
            : base("name=TayanaYachtDB")
        {
        }

        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
