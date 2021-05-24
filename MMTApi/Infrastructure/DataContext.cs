using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMTApi.Infrastructure
{
    public class DataContext:DbContext
    {
        //connection string can be stored in database and injected using DI
        public string connectionString { get; set; } = "Server=tcp:mmt-sse-test.database.windows.net,1433;Initial Catalog=SSE_Test;Persist Security Info=False;User ID=mmt-sse-test;Password=database-user-01;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);

            }
            base.OnConfiguring(optionsBuilder);
        }
       public  DbSet<Order> Orders { get; set; }
       public  DbSet<OrderItem> OrderItems { get; set; }
       public  DbSet<Product> Products { get; set; }
       
    }
}
