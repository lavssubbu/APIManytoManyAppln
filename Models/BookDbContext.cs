using Microsoft.EntityFrameworkCore;

namespace APICodeFirst.Models
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> AuthorBooks { get; set; }

        public BookDbContext(DbContextOptions opt) : base(opt) { }

        //FluentAPI - to configure many-many relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new {ba.BookId, ba.AuthorId});

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.book)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(a => a.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(a => a.AuthorId);

        }
    }
}
