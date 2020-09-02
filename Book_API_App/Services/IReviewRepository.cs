using Book_API_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Services
{
    public interface IReviewRepository
    {
        ICollection<Review> GetAllReview();
        Review GetReviewer(int reviewId);
        ICollection<Review> GetReviewsByBook(int bookId);
        Book GetBookOfReview(int reviewId);
        bool ReviewExist(int reviewId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Save();
    }
}
