using System.ComponentModel.DataAnnotations;

namespace APICodeFirst.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string? Title { get; set; }
        public int Price { get;set; }

        //Navigation Property
        public ICollection<BookAuthor>? BookAuthors { get; set; }

    }
}
