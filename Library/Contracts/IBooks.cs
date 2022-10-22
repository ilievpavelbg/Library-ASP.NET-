using Library.Data;
using Library.Models;

namespace Library.Contracts
{
    public interface IBooks
    {
        Task<IEnumerable<BooksViewModel>> GetAllBooks();

        Task<IEnumerable<Category>> GetAllCategories();

        Task AddBooksAsync(AddBooksViewModel model);

        Task AddToCollectionAsync(int bookId, string userId);

        Task RemoveFromCollectionAsync(int movieId, string userId);

        Task<IEnumerable<MyBooksViewModel>> GetMyBooks(string userId);
    }
}
