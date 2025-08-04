namespace API.Dto
{
    public class BookDto
    {
        public int BookSeed { get; set; }
        public int Index { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int ReleaseYear { get; set; }
        public int Likes { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
    }
}
