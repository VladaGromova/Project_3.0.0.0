using System.Collections.Generic;
using System.Linq;
using Project33.Data;
using Project33.Services.Models;

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
            var authors = _bookContext.Books.Select(BuildBook).Select(book => book.author);
            return authors.Distinct().ToList();
        }
        
        public Books FindByName(string name)
        {
            return _bookContext.Books.Select(BuildBook).FirstOrDefault(book => book.name == name);
        }
        
        public Books FindByAuthor(string author)
        {
            return _bookContext.Books.Select(BuildBook).FirstOrDefault(book => book.author == author);
        }
        
        public Books FindByGenre(string genre)
        {
            return _bookContext.Books.Select(BuildBook).FirstOrDefault(book => book.genre == genre);
        }

        private Books BuildBook(Books b)
        {
            return new Books
            {
                name = b.name,
                author = b.author,
                genre = b.genre,
                description = b.description,
                cover=b.cover,
                likes = b.likes,
            };
        }
    }
}