using API.Dto;

namespace API.Interfaces
{
    public interface IBookGeneratorService
    {
        public IEnumerable<BookDto> GenerateBooks(RequestDto dto);
    }
}
