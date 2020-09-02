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
    public class ReviewsController : Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;
        private IBookRepository _bookRepository;
        public ReviewsController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository, IBookRepository bookRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            IEnumerable<Review> review = _reviewRepository.GetAllReview();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<ReviewDto> reviewDto = new List<ReviewDto>();

            foreach (Review r in review)
            {
                reviewDto.Add(new ReviewDto
                {
                    Id = r.Id,
                    HeadLine = r.HeadLine,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating
                });
            }

            return Ok(reviewDto);
        }

        [HttpGet("{reviewId}", Name = "GetReview")]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();

            Review review = _reviewRepository.GetReviewer(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReviewDto reviewdto = new ReviewDto()
            {
                Id = review.Id,
                HeadLine = review.HeadLine,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };

            return Ok(reviewdto);
        }

        [HttpGet("books/{bookId}")]
        public IActionResult GetReviewsForBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            IEnumerable<Review> reviews = _reviewRepository.GetReviewsByBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<ReviewDto> reviewDto = new List<ReviewDto>();

            foreach (Review review in reviews)
            {
                reviewDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    HeadLine = review.HeadLine,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewDto);
        }

        [HttpGet("{reviewId}/book")]
        public IActionResult GetBookOfReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();

            Book books = _reviewRepository.GetBookOfReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BookDto bookDto = new BookDto
            {
                Id = books.Id,
                Datetime = books.PublishedDate,
                ISBN = books.Isbn,
                Title = books.Title
            };

            return Ok(bookDto);
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] Review reviewtoCreate)
        {
            if (reviewtoCreate == null)
                return BadRequest();

            if (!_reviewerRepository.ReviewerExist(reviewtoCreate.Reviewers.Id))
                return BadRequest();

            if (!_bookRepository.BookExist(reviewtoCreate.Books.Id))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            reviewtoCreate.Books = _bookRepository.GetBook(reviewtoCreate.Books.Id);
            reviewtoCreate.Reviewers = _reviewerRepository.GetReviewer(reviewtoCreate.Reviewers.Id);

            if (!_reviewRepository.CreateReview(reviewtoCreate))
                return BadRequest(ModelState);

            return CreatedAtRoute("GetReview", new { reviewId = reviewtoCreate.Id }, reviewtoCreate);
        }
    }
}
