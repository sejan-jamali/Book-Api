using Book_API_App.Models;
using Book_API_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        public BookDBContext _bookDBContext { get; set; }

        public ReviewerRepository(BookDBContext bookDBContext)
        {
            _bookDBContext = bookDBContext;
        }
        public ICollection<Reviewer> GetAllReviewer()
        {
            return _bookDBContext.Reviewers.OrderBy(r => r.FirstName).ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _bookDBContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerOfReview(int reviewId)
        {
            var reviewerid = _bookDBContext.Reviews.Where(r => r.Id == reviewId).Select(rr => rr.Reviewers.Id).FirstOrDefault();
            return _bookDBContext.Reviewers.Where(c => c.Id == reviewerid).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _bookDBContext.Reviews.Where(r => r.Reviewers.Id == reviewerId).ToList();
        }

        public bool ReviewerExist(int reviewerId)
        {
            return _bookDBContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool ReviewExist(int reviewId)
        {
            throw new NotImplementedException();
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _bookDBContext.AddAsync(reviewer);
            return Save();
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _bookDBContext.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _bookDBContext.Remove(reviewer);
            return Save();
        }

        public bool Save()
        {
            var saved = _bookDBContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
