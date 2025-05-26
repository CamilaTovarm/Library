using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface ICountryService
    {
        Task<List<Country>> GetAll();
        Task<Country> GetCountry(int idCountry);
        Task<Country> CreateCountry(string CountryName);
        Task<Country> UpdateCountry(int idCountry, string? CountryName = null);
        Task<Country> DeleteCountry(int idCountry);
    }
    public class CountryService
    {
        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<Country> CreateCountry(string countryName)
        {
            return await _countryRepository.CreateCountry(countryName);
        }
        public async Task<Country> DeleteCountry(int idCountry)
        {
            Country countryToDelete = await _countryRepository.GetCountry(idCountry);
            if (countryToDelete == null)
            {
                throw new Exception($"This Country with the Id {idCountry} doesn't exist.");
            }
            countryToDelete.State = true;
            countryToDelete.CreateDate = DateTime.Now;
            return await _countryRepository.DeleteCountry(countryToDelete);
        }
        public async Task<List<Country>> GetAll()
        {
            return await _countryRepository.GetAll();
        }
        public async Task<Country> GetCountry(int idCountry)
        {
            return await _countryRepository.GetCountry(idCountry);
        }
        public async Task<Country> UpdateCountry(int idCountry, string? countryName = null)
        {
            Country newCountry = await _countryRepository.GetCountry(idCountry);
            if (newCountry != null)
            {
                if (countryName != null)
                {
                    newCountry.CountryName = (string)countryName;
                }
                return await _countryRepository.UpdateCountry(newCountry);
            }
            throw new NotImplementedException();
        }
    }
}