using Microsoft.EntityFrameworkCore;

namespace Library.Context
{
    public class LibraryDbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary Keys
            

            // Foreign Keys
           

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;

            }
        }
    }
}
