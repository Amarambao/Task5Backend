namespace API.Dto
{
    public class RequestDto
    {
        public int Seed { get; set; }
        public int StartIndex { get; set; }
        public int ReturnCount { get; set; }
        public string? Region { get; set; }
        public decimal? AvgLikes { get; set; }
        public decimal? AvgReviewCount { get; set; }
    }
}
