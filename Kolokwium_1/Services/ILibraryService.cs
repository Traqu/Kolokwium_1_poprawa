using Kolokwium_1.Dtos;

namespace Services
{
    public interface ILibraryService
    {
        LibraryDto GetLibrary(int id);
    }
}