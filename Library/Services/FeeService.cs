using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IFeeService
    {
        Task<List<Fee>> GetAllFee();
        Task<Fee> GetFeeById(int id);
        Task<Fee> CreateFee(int daysMin, int daysMax, float feeValue, string feeDescription);
        Task<Fee> UpdateFee(int id, int? daysMin = null, int? daysMax = null, float? feeValue = null, string? feeDescription = null);
        Task<Fee> DeleteFee(int id);
    }
    public class FeeService : IFeeService
    {
        private readonly IFeeRepository _feeRepository;
        public FeeService(IFeeRepository feeRepository)
        {
            _feeRepository = feeRepository;
        }
        public Task<Fee> CreateFee(int daysMin, int daysMax, float feeValue, string feeDescription)
        {
            return _feeRepository.CreateFee(daysMin, daysMax, feeValue, feeDescription);
        }
        public async Task<Fee> DeleteFee(int id)
        {
            Fee feeToDelete = await _feeRepository.GetFeeById(id);
            if (feeToDelete == null)
            {
                throw new Exception($"This Fee with the Id {id} doesn't exist.");
            }
            feeToDelete.State = true;
            feeToDelete.CreateDate = DateTime.Now;
            return await _feeRepository.DeleteFee(feeToDelete);
        }
        public async Task<List<Fee>> GetAllFee()
        {
            return await _feeRepository.GetAllFee();
        }
        public async Task<Fee> GetFeeById(int id)
        {
            return await _feeRepository.GetFeeById(id);
        }
        public async Task<Fee> UpdateFee(int id, int? daysMin = null, int? daysMax = null, float? feeValue = null, string? feeDescription = null)
        {
            Fee feeToUpdate = await _feeRepository.GetFeeById(id);
            if (feeToUpdate != null)
            {
                if (daysMin != null)
                {
                    feeToUpdate.DaysMin = (int)daysMin;
                }
                if (daysMax != null)
                {
                    feeToUpdate.DaysMax = (int)daysMax;
                }
                if (feeValue != null)
                {
                    feeToUpdate.FeeValue = (float)feeValue;
                }
                if (feeDescription != null)
                {
                    feeToUpdate.FeeDescription = feeDescription;
                }
                return await _feeRepository.UpdateFee(feeToUpdate);
            }
            throw new NotImplementedException();
        }
    }
}

