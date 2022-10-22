using Library.Contracts;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BooksServices : IBooks
    {
        private readonly LibraryDbContext context;

        public BooksServices(LibraryDbContext _context)
        {
            context = _context;
        }

        public async Task AddBooksAsync(AddBooksViewModel model)
        {
            var book = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                Rating = model.Rating,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId
            };

            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();

        }

        public async Task AddToCollectionAsync(int bookId, string userId)
        {
            var user = context.Users
               .Where(x => x.Id == userId)
              .Include(um => um.ApplicationUsersBooks)
              .FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException("Can not found user with such ID !");
            }

            var book = context.Books.FirstOrDefault(m => m.Id == bookId);

            if (book == null)
            {
                throw new ArgumentException("Can not found book with such ID !");
            }

            if (!user.ApplicationUsersBooks.Any(m => m.BookId == bookId))
            {
                user.ApplicationUsersBooks.Add(new ApplicationUserBook()
                {
                    ApplicationUserId = user.Id,
                    BookId = book.Id,
                    ApplicationUser = user,
                    Book = book
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BooksViewModel>> GetAllBooks()
        {
            var entity = await context.Books
                .Include(g => g.Category)
                .ToListAsync();

            return entity.Select(m => new BooksViewModel
            {
                Id = m.Id,
                Title = m.Title,
                ImageUrl = m.ImageUrl,
                Rating = m.Rating,
                Author = m.Author,
                Category = m.Category.Name
            }).ToList();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<MyBooksViewModel>> GetMyBooks(string userId)
        {
            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .ThenInclude(x => x.Book)
                .ThenInclude(s => s.Category)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException();
            }

            return user.ApplicationUsersBooks
               .Select(x => new MyBooksViewModel()
               {
                   Id = x.Book.Id,
                   ImageUrl = x.Book.ImageUrl,
                   Title = x.Book.Title,
                   Author = x.Book.Author,
                   Description = x.Book.Description,
                   Category = x.Book.Category.Name
               });
        }

        public async Task RemoveFromCollectionAsync(int bookId, string userId)
        {
            var user = context.Users
                .Where(x => x.Id == userId)
               .Include(um => um.ApplicationUsersBooks)
               .FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException("Can not found user with such ID !");
            }

            var book = user.ApplicationUsersBooks.FirstOrDefault(m => m.BookId == bookId);

            if (book != null)
            {
                user.ApplicationUsersBooks.Remove(book);
                await context.SaveChangesAsync();
            }

        }
    }
}
