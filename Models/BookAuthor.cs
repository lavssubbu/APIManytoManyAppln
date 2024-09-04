namespace APICodeFirst.Models
{
    public class BookAuthor //Junction table
    {
        public int BookId { get; set; }
        public Book? book { get; set; }
        public int AuthorId {  get; set; }
        public Author? author { get; set; }
    }
}
