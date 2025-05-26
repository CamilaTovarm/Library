using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAll();
        Task<Country> GetCountry(int id);
        Task<Country> CreateCountry(string CountryName);
        Task<Country> UpdateCountry(Country country);
        Task<Country> DeleteCountry(Country country);
    }
    public class CountryRepository : ICountryRepository
    {
        private readonly LibraryDbContext _db;
        public CountryRepository(LibraryDbContext db)
        {
            _db = db;
        }

        public async Task<Country> CreateCountry(string CountryName)
        {
            Country newCountry = new Country
            {
                CountryName = CountryName,
                CreateDate = null,
                State = false

            };
            await _db.Country.AddAsync(newCountry);
            await _db.SaveChangesAsync();
            return newCountry;
        }

        public async Task<Country> DeleteCountry(Country country)
        {
            _db.Country.Attach(country);
            _db.Entry(country).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<Country> GetCountry(int id)
        {
            return await _db.Country.FirstOrDefaultAsync(e => e.CountryId == id);
        }

        public async Task<List<Country>> GetAll()
        {
            return await _db.Country.ToListAsync();
        }

        public async Task<Country> UpdateCountry(Country country)
        {
            Country countryUpdate = await _db.Country.FindAsync(country.CountryId);
            if (countryUpdate != null)
            {
                countryUpdate.CountryName = country.CountryName;
                await _db.SaveChangesAsync();
                return countryUpdate;
            }
            return null;
        }
    }
}

