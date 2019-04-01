using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloOData
{
    public class CustomersContext : DbContext
    {
        public CustomersContext() : base("name=localdb") { }
        public DbSet<Customer> Customers { get; set; }
    }
}
