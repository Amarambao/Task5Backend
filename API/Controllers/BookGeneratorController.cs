using API.Dto;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BookGeneratorController : Controller
    {
        private readonly IBookGeneratorService _bookGenSrv;

        public BookGeneratorController(IBookGeneratorService bookGenSrv)
        {
            _bookGenSrv = bookGenSrv;
        }

        [HttpGet]
        [Route("get")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] RequestDto dto)
        {
            var result = _bookGenSrv.GenerateBooks(dto);

            return Ok(result);
        }
    }
}
