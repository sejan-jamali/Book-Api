using Book_API_App.Models;
using Book_API_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private BookDBContext _reviewRepository;
        public ReviewRepository(BookDBContext bookDBContext)
        {
            _reviewRepository = bookDBContext;
        }

        public bool CreateReview(Review review)
        {
            _reviewRepository.AddAsync(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _reviewRepository.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _reviewRepository.RemoveRange(reviews);
            return Save();
        }

        public ICollection<Review> GetAllReview()
        {
            return _reviewRepository.Reviews.OrderBy(r => r.Rating).ToList();
        }

        public Book GetBookOfReview(int reviewId)
        {
            var bookId = _reviewRepository.Reviews.Where(r => r.Id == reviewId).Select(b => b.Books.Id).FirstOrDefault();
            return _reviewRepository.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Review GetReviewer(int reviewId)
        {
            return _reviewRepository.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByBook(int bookId)
        {
            return _reviewRepository.Reviews.Where(r => r.Books.Id == bookId).ToList();
        }

        public bool ReviewExist(int reviewId)
        {
           return _reviewRepository.Reviews.Any(r => r.Id == reviewId) ;
        }

        public bool Save()
        {
            var saved = _reviewRepository.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _reviewRepository.Update(review);
            return Save();
        }
    }
}
