using Microsoft.AspNetCore.Mvc;
using Services;
using Kolokwium_1.Dtos;

namespace Zadanie5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("{id}")]
        public ActionResult<LibraryDto> GetLibrary(int id)
        {
            try
            {
                var result = _libraryService.GetLibrary(id);
                if (result == null)
                {
                    return NotFound("There is no library with given id");
                }
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}