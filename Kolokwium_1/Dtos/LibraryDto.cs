namespace Kolokwium_1.Dtos
{
    public class LibraryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Location { get; set; }
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }
}