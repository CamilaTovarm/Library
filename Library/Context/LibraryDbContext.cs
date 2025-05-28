using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Context
{
    public class LibraryDbContext: DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<AuthorVsBook> AuthorVsBooks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Editorial> Editorial { get; set; }
        public DbSet<Fee> fee { get; set; }
        public DbSet<Loans> Loans { get; set; }
        public DbSet<Returns> Returns { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public object Database { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary Keys
            modelBuilder.Entity<Authors>().HasKey(e => e.AuthorId);
            modelBuilder.Entity<AuthorVsBook>().HasKey(e => e.AuthorVsBookId);
            modelBuilder.Entity<Book>().HasKey(e => e.BookId);
            modelBuilder.Entity<Country>().HasKey(e => e.CountryId);
            modelBuilder.Entity<Editorial>().HasKey(e => e.EditorialId);
            modelBuilder.Entity<Fee>().HasKey(e => e.FeeId);
            modelBuilder.Entity<Loans>().HasKey(e => e.LoanId);
            modelBuilder.Entity<Returns>().HasKey(e => e.ReturnId);
            modelBuilder.Entity<User>().HasKey(e => e.UserId);
            modelBuilder.Entity<UserType>().HasKey(e => e.UserTypeId);

            // Foreign Keys
            modelBuilder.Entity<AuthorVsBook>().HasOne(e => e.Authors).WithMany().HasForeignKey(e => e.AuthorId);
            modelBuilder.Entity<AuthorVsBook>().HasOne(e => e.Book).WithMany().HasForeignKey(e => e.BookId);
            modelBuilder.Entity<Book>().HasOne(e => e.Editorial).WithMany().HasForeignKey(e => e.EditorialId);
            modelBuilder.Entity<Book>().HasOne(e => e.Country).WithMany().HasForeignKey(e => e.CountryId);
            modelBuilder.Entity<Book>().HasOne(e => e.Author).WithMany().HasForeignKey(e => e.AuthorId);
            modelBuilder.Entity<User>().HasOne(e => e.UserType).WithMany().HasForeignKey(e => e.UserTypeId);
            modelBuilder.Entity<Loans>().HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Loans>().HasOne(e => e.Book).WithMany().HasForeignKey(e => e.BookId);
            modelBuilder.Entity<Returns>().HasOne(e => e.Loans).WithMany().HasForeignKey(e => e.LoanId);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
