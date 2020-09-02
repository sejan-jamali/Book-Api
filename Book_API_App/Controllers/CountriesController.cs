using Book_API_App.Dtos;
using Book_API_App.DTOs;
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
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepository;
        private IAuthorRepository _authorRepository;
        public CountriesController(ICountryRepository countryRepository, IAuthorRepository authorRepository)
        {
            _countryRepository = countryRepository;
            _authorRepository = authorRepository;
        }

        //api/get/get all country
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            List<Country> country = _countryRepository.GetAllCountry().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<CountryDto> countryDto = new List<CountryDto>();
            foreach (Country c in country)
            {
                countryDto.Add(new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name
                });
            }
            return Ok(countryDto);
        }

        //api/get/get specific country
        [HttpGet("{countryId}", Name = "GetCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            Country country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CountryDto countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };
            
            return Ok(countryDto);
        }


        [HttpGet("authors/{authorId}")]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExist(authorId))
                return NotFound();

            Country country = _countryRepository.GetAuthorCountry(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CountryDto countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);

        }

        //api/countries/countryid/auuthors
        [HttpGet("{countryId}/authors")]
        public IActionResult GetAuthorsFromCounrty(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            ICollection<Author> author = _countryRepository.GetAllAuthorFromACountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest();

            List<AuthorDto> authorDto = new List<AuthorDto>();

            foreach(Author a in author)
            {
                authorDto.Add(new AuthorDto{
                    Id = a.Id,
                    FirstName = a.First_Name,
                    LastName = a.Last_Name
                });
            }

            return Ok(authorDto);
        }

        [HttpPost]
        public IActionResult CreateCountry([FromBody]Country countrytoCreate)
        {
            if (countrytoCreate == null)
                return BadRequest();

            if (_countryRepository.isDuplicateCountryName(countrytoCreate.Id, countrytoCreate.Name))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_countryRepository.CreateCountry(countrytoCreate))
                return BadRequest(ModelState);

            return CreatedAtRoute("GetCountry", new { countryId = countrytoCreate.Id }, countrytoCreate);
        }

        [HttpPut("{countryId}")]
        public IActionResult UpdateCountry(int countryId, [FromBody]Country updateCountry)
        {
            if (updateCountry == null)
                return BadRequest();

            if (countryId != updateCountry.Id)
                return BadRequest();

            if (!_countryRepository.CountryExists(updateCountry.Id))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (_countryRepository.isDuplicateCountryName(countryId, updateCountry.Name))
                return BadRequest();

            if (!_countryRepository.UpdateCountry(updateCountry))
                return BadRequest(ModelState);
            
            return NotFound();
        }

        [HttpDelete("{countryId}")]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return BadRequest();

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if (_countryRepository.isDuplicateCountryName(countryId, countryToDelete.Name))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_countryRepository.DeleteCountry(countryToDelete))
                return BadRequest(ModelState);
            else
                return Ok();
        }
    }
}
