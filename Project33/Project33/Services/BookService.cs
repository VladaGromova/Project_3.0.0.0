using System.Collections.Generic;
using System.Linq;
using Project33.Data;
using Project33.Models;
using Project33.Services;

namespace Project33.Services
{
    public class BookService
    {
        private readonly BooksContext _bookContext;

        public BookService()
        {
            _bookContext = new BooksContext();
        }

        public List<Books> GetBooks()
        {
            return _bookContext.Books.Select(BuildBook).ToList();
        }
        
        public List<string> GetAuthors()
        {
            var authors = _bookContext.Books.Select(BuildBook).Select(book => book.Author);
            return authors.Distinct().ToList();
        }
        
        public Books FindByName(string name)
        {
            return _bookContext.Books.Select(BuildBook).FirstOrDefault(book => book.Name == name);
        }
        
        public Books FindByAuthor(string author)
        {
            return _bookContext.Books.Select(BuildBook).FirstOrDefault(book => book.Author == author);
        }
        
        public Books FindByGenre(string genre)
        {
            return _bookContext.Books.Select(BuildBook).FirstOrDefault(book => book.Genre == genre);
        }

        private Books BuildBook(Books b)
        {
            return new Books
            {
                Name = b.Name,
                Author = b.Author,
                Genre = b.Genre,
            };
        }
    }
}