using Book_API_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Services
{
    public interface ICountryRepository
    {
        ICollection<Country> GetAllCountry();
        Country GetCountry(int countryId);
        Country GetAuthorCountry(int authorId);
        ICollection<Author> GetAllAuthorFromACountry(int countryId);
        bool CountryExists(int countryId);
        bool isDuplicateCountryName(int countryId, string countryName);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
    }
}
