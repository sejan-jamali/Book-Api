using Book_API_App.Models;
using Book_API_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookDBContext _bookDBContext;
        public AuthorRepository(BookDBContext bookDBContext)
        {
            _bookDBContext = bookDBContext;
        }
        public bool AuthorExist(int authorId)
        {
            return _bookDBContext.Authors.Any(a => a.Id == authorId);
        }

        public bool CreateAuthor(Author author)
        {
            _bookDBContext.AddAsync(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _bookDBContext.Remove(author);
            return Save();
        }

        public ICollection<Author> GetAllAuthors()
        {
            return _bookDBContext.Authors.OrderBy(a => a.First_Name).ToList();
        }

        public Author GetAuthor(int authorId)
        {
            return _bookDBContext.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthorsOfBook(int bookId)
        {
            return _bookDBContext.BookAuthors.Where(b => b.BookId == bookId).Select(a => a.Author).ToList();
        }

        public ICollection<Book> GetBooksOfAuthor(int authorId)
        {
            return _bookDBContext.BookAuthors.Where(a => a.AuthorId == authorId).Select(b => b.Book).ToList();
        }

        public bool Save()
        {
            var saved = _bookDBContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _bookDBContext.Update(author);
            return Save();
        }
    }
}
