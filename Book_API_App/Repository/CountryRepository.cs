using Book_API_App.Models;
using Book_API_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private BookDBContext _bookDBContext;

        public CountryRepository(BookDBContext bookDBContext)
        {
            _bookDBContext = bookDBContext;
        }

        public bool CountryExists(int countryId)
        {
            return _bookDBContext.Countries.Any(c => c.Id == countryId);
        }

        public bool CreateCountry(Country country)
        {
            _bookDBContext.AddAsync(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _bookDBContext.Remove(country);
            return Save();
        }

        public ICollection<Author> GetAllAuthorFromACountry(int countryId)
        {
            return _bookDBContext.Authors.Where(c => c.Country.Id == countryId).ToList();
        }

        public ICollection<Country> GetAllCountry()
        {
            return _bookDBContext.Countries.OrderBy(c => c.Name).ToList();
        }

        public Country GetAuthorCountry(int authorId)
        {
            return _bookDBContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
        }

        public Country GetCountry(int countryId)
        {
            return _bookDBContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public bool isDuplicateCountryName(int countryId, string countryName)
        {
            var country = _bookDBContext.Countries.Where(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper() && c.Id != countryId).FirstOrDefault();
            return country == null ? false : true;
        }

        public bool Save()
        {
            var saved = _bookDBContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _bookDBContext.Update(country);
            return Save();
        }
    }
}
