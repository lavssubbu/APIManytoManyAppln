namespace APICodeFirst.Models
{
    public class Author //Many-Many Relationship
    {
        public int AuthorId {  get; set; }
        public string? AuthorName { get; set; }

        //Navigation Property
        public ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}
