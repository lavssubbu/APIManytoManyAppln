namespace APICodeFirst.DTO
{
    public class BookDTO
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public List<int> AuthorIDs { get; set; }
    }
}
