using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Entities;

namespace WebApi.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        ///<Summary>
        /// Connects the **Users** Entity class to the Database.
        ///</Summary>
        public DbSet<User> Users { get; set; }

        ///<Summary>
        /// Connects the **Feedbacks** Entity class to the Database.
        ///</Summary>
        public DbSet<Feedback> Feedbacks { get; set; }
        
    }
}