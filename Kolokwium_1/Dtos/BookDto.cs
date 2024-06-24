namespace Kolokwium_1.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Rating { get; set; }
        public int LibraryId { get; set; }
        public AuthorDto Author { get; set; }
        public CategoryDto Category { get; set; }
    }
}