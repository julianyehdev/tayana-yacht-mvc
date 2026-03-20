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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
