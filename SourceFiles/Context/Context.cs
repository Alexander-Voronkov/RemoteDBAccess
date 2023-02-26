using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Context
{
    public class Context : DbContext
    {
        public Context(string conn = "Default") : base(conn)
        {

        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        protected override void OnModelCreating(DbModelBuilder dmb)
        {
            base.OnModelCreating(dmb);
        }
    }
}
