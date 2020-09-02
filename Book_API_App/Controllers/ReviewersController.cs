using Book_API_App.Dtos;
using Book_API_App.Models;
using Book_API_App.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController : Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;
        public ReviewersController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        public IActionResult GetReviewers()
        {
            IEnumerable<Reviewer> reviewer = _reviewerRepository.GetAllReviewer();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<ReviewerDto> reviewerDto = new List<ReviewerDto>();

            foreach(Reviewer r in reviewer)
            {
                reviewerDto.Add(new ReviewerDto
                {
                    Id = r.Id,
                    FirstName = r.FirstName,
                    LastName = r.LastName
                });
            }

            return Ok(reviewerDto);
        }

        [HttpGet("{reviewerId}", Name ="GetReviewer")]
        public IActionResult GetReviwer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();

            Reviewer reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReviewerDto review = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(review);
        }

        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();

            IEnumerable<Review> review = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<ReviewDto> reviewDto = new List<ReviewDto>();

            foreach (Review r in review)
            {
                reviewDto.Add(new ReviewDto
                {
                    Id = r.Id,
                    HeadLine = r.HeadLine,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText
                });
            }

            return Ok(reviewDto);
        }

        [HttpGet("{reviewerId}/reviewer")]
        public IActionResult GetReviewerOfReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();

            Reviewer reviewer = _reviewerRepository.GetReviewerOfReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReviewerDto reviewerDto = new ReviewerDto
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
        }

        [HttpPost]
        public IActionResult CreateReviewer([FromBody] Reviewer reviewertoCreate)
        {
            if (reviewertoCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewerRepository.CreateReviewer(reviewertoCreate))
                return BadRequest(ModelState);

            return CreatedAtRoute("GetReviewer", new { reviewerId = reviewertoCreate.Id }, reviewertoCreate);
        }

        [HttpPut("{reviewerId}")]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] Reviewer updateReviewer)
        {
            if (updateReviewer == null)
                return BadRequest();

            if (reviewerId != updateReviewer.Id)
                return BadRequest();

            if (!_reviewerRepository.ReviewerExist(updateReviewer.Id))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewerRepository.UpdateReviewer(updateReviewer))
                return BadRequest(ModelState);
            else
            {
                return Ok();
            }
        }

        [HttpDelete("{reviewerId}")]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return BadRequest();

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            var reviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
                return BadRequest(ModelState);
            else
                return Ok();
        }
    }
}
