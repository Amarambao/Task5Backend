using API.Dto;
using API.Interfaces;
using Bogus;
using System.Text;
using Bogus.Distributions.Gaussian;

namespace API.Services
{
    public class BookGeneratorService : IBookGeneratorService
    {
        public BookGeneratorService() { }

        public IEnumerable<BookDto> GenerateBooks(RequestDto dto)
        {
            var books = new List<BookDto>();

            var faker = new Faker(dto.Region);

            for (int i = 0; i < dto.ReturnCount; i++)
            {
                var bookSeed = dto.Seed + dto.StartIndex + i;

                faker.Random = new Randomizer(bookSeed);

                var book = new BookDto()
                {
                    BookSeed = bookSeed,
                    Index = dto.StartIndex + i,
                    Isbn = FormatIsbn(faker.Commerce.Ean13()),
                    Title = faker.Commerce.ProductName(),
                    Author = GenerateAuthors(faker),
                    Publisher = $"{faker.Company.CompanySuffix()} {faker.Company.CompanyName()}",
                    ReleaseYear = faker.Date.BetweenDateOnly(new DateOnly(1800, 1, 1), new DateOnly(2025, 1, 1)).Year,
                    Likes = GetCountByRequest(faker, dto.AvgLikes.HasValue ? (double)dto.AvgLikes : null),
                };
                book.Reviews = GenerateReviews(faker, book.Title, dto.AvgReviewCount.HasValue ? (double)dto.AvgReviewCount : null);

                books.Add(book);
            }

            return books;
        }

        private string FormatIsbn(string isbn)
            => $"{isbn[0..3]}-{isbn[3..4]}-{isbn[4..7]}-{isbn[7..12]}-{isbn[12..]}";

        private string GenerateAuthors(Faker faker)
        {
            var authorsCount = faker.Random.Int(1, 3);

            var sb = new StringBuilder();

            for (int i = 0; i < authorsCount; i++)
                sb.Append($"{faker.Name.FullName()}, ");

            return sb.ToString().TrimEnd([' ', ',']);
        }

        private IEnumerable<ReviewDto> GenerateReviews(Faker faker, string bookTitle, double? requestedCount = null)
        {
            var reviewCount = GetCountByRequest(faker, requestedCount);

            var reviews = new List<ReviewDto>();

            for (int i = 0; i < reviewCount; i++)
                reviews.Add(new ReviewDto
                {
                    Author = faker.Name.FindName(),
                    Text = faker.Rant.Review(bookTitle),
                });

            return reviews;
        }

        private int GetCountByRequest(Faker faker, double? requestedCount = null)
        {
            if (!requestedCount.HasValue)
                return faker.Random.Int(0, 10);
            
            var result = faker.Random.GaussianInt(requestedCount.Value, 2);

            return result >= 0 ? result : 0;
        }
    }
}
