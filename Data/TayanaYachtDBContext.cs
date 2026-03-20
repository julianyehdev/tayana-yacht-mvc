using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TayanaYachtMVC.Data
{
    public partial class TayanaYachtDBContext : DbContext
    {
        public TayanaYachtDBContext()
            : base("name=TayanaYachtDB")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
