using API.Dto;
using API.Interfaces;
using Bogus;
using System.Text;

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
                    Likes = GetWeightedRandom(faker, dto.AvgLikes),
                };
                book.Reviews = GenerateReviews(faker, book.Title, dto.AvgReviewCount);

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

        private IEnumerable<ReviewDto> GenerateReviews(Faker faker, string bookTitle, decimal? requestedCount = null)
        {
            var reviewCount = GetWeightedRandom(faker, requestedCount);

            var reviews = new List<ReviewDto>();

            for (int i = 0; i < reviewCount; i++)
                reviews.Add(new ReviewDto
                {
                    Author = faker.Name.FindName(),
                    Text = faker.Rant.Review(bookTitle),
                });

            return reviews;
        }

        private int GetWeightedRandom(Faker faker, decimal? requestedCount = null)
        {
            if (requestedCount is null)
                return faker.Random.Int(0, 10);

            var likeItems = new int[]
            {
                (int)Math.Floor(requestedCount.Value),
                (int)Math.Ceiling(requestedCount.Value)
            };

            var likeWeights = new float[]
            {
                (float)(Math.Ceiling(requestedCount.Value) - requestedCount),
                (float)(requestedCount - Math.Floor(requestedCount.Value)),
            };

            return faker.Random.WeightedRandom(likeItems, likeWeights);
        }
    }
}
