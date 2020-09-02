using Book_API_App.Models;
using Book_API_App.Services;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Repository
{
    public class BookRepository : IBookRepository
    {
        private BookDBContext _bookDBContext;
        public BookRepository(BookDBContext bookDBContext)
        {
            _bookDBContext = bookDBContext;
        }
        public bool BookExist(int bookId)
        {
            return _bookDBContext.Books.Any(b => b.Id == bookId);
        }

        public bool CreateBook(List<int> authorId, List<int> categorieId, Book book)
        {
            var authors = _bookDBContext.Authors.Where(a => authorId.Contains(a.Id)).ToList();
            var categories = _bookDBContext.Categories.Where(c => categorieId.Contains(c.Id)).ToList();

            foreach(var a in authors)
            {
                var bookauthor = new BookAuthor
                {
                    Author = a,
                    Book = book
                };
            }

            foreach (var c in categories)
            {
                var bookcategories = new BookCategory
                {
                    Category = c,
                    Book = book
                };
            }

            _bookDBContext.Add(book);

            return Save();
        }

        public bool DeleteBook(Book book)
        {
            _bookDBContext.Remove(book);
            return Save();
        }

        public ICollection<Book> GetAllBooks()
        {
            return _bookDBContext.Books.OrderBy(b => b.Title).ToList();
        }

        public Book GetBook(int bookId)
        {
            return _bookDBContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public decimal GetBookRatings(int bookId)
        {
            var review = _bookDBContext.Reviews.Where(r => r.Books.Id == bookId);
            if (review.Count() <= 0)
                return 0;
            return ((decimal) review.Sum(r => r.Rating) / review.Count());
        }

        public bool ISBNExist(string bookISBN)
        {
            return _bookDBContext.Books.Any(b => b.Isbn == bookISBN);
        }

        public bool IsDuplicateISBN(int bookId, string bookISBN)
        {
            var book = _bookDBContext.Books.Where(b => b.Isbn.Trim().ToUpper() == bookISBN.Trim().ToUpper() && b.Id != bookId).FirstOrDefault();
            return book == null ? false : true;
        }

        public bool Save()
        {
            var saved = _bookDBContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBook(List<int> authorId, List<int> categorieId, Book book)
        {
            var authors = _bookDBContext.Authors.Where(a => authorId.Contains(a.Id)).ToList();
            var categories = _bookDBContext.Categories.Where(c => categorieId.Contains(c.Id)).ToList();

            var bookauthortodelete = _bookDBContext.BookAuthors.Where(b => b.BookId == book.Id);
            var bookcategorytodelete = _bookDBContext.BookCategories.Where(c => c.BookId == book.Id);

            _bookDBContext.RemoveRange(bookauthortodelete);
            _bookDBContext.RemoveRange(bookcategorytodelete);

            foreach (var a in authors)
            {
                var bookauthor = new BookAuthor
                {
                    Author = a,
                    Book = book
                };
            }

            foreach (var c in categories)
            {
                var bookcategories = new BookCategory
                {
                    Category = c,
                    Book = book
                };
            }

            _bookDBContext.Update(book);

            return Save();
        }
    }
}
