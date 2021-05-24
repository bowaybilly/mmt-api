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
        public static string ConnectionString { get; set; }  
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataContext.ConnectionString);

            }
            base.OnConfiguring(optionsBuilder);
        }
       public  DbSet<Order> Orders { get; set; }
       public  DbSet<OrderItem> OrderItems { get; set; }
       public  DbSet<Product> Products { get; set; }
       
    }
}
