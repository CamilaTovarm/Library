using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IReturnsService
    {
        Task<List<Returns>> GetAll();
        Task<Returns> GetReturns(int id);
        Task<Returns> CreateReturns(DateTime returnDate, int loansId, float fineImposed);
        Task<Returns> UpdateReturns(int id, DateTime? returnDate = null, int? loansId = null, float?  fineImposed= null);
        Task<Returns> DeleteReturns(int id);
    }
    public class ReturnsService : IReturnsService
    {
        private readonly IReturnsRepository _returnsRepository;
        public ReturnsService(IReturnsRepository returnsRepository)
        {
            _returnsRepository = returnsRepository;
        }
        public Task<Returns> CreateReturns(DateTime returnDate, int loansId, float fineImposed)
        {
            return _returnsRepository.CreateReturns(returnDate, loansId, fineImposed);
        }
        public async Task<Returns> DeleteReturns(int id)
        {
            Returns returnsToDelete = await _returnsRepository.GetReturns(id);
            if (returnsToDelete == null)
            {
                throw new Exception($"This Returns with the Id {id} doesn't exist.");
            }
            returnsToDelete.State = true;
            returnsToDelete.CreateDate = DateTime.Now;
            return await _returnsRepository.DeleteReturns(returnsToDelete);
        }
        public async Task<List<Returns>> GetAll()
        {
            return await _returnsRepository.GetAll();
        }
        public async Task<Returns> GetReturns(int id)
        {
            return await _returnsRepository.GetReturns(id);
        }
        public async Task<Returns> UpdateReturns(int id, DateTime? returnDate = null, int? loansId = null, float? fineImposed = null)
        {
            Returns returnsToUpdate = await _returnsRepository.GetReturns(id);
            if (returnsToUpdate != null)
            {
                if (returnDate != null)
                {
                    returnsToUpdate.ReturnDate = (DateTime)returnDate;
                }
                if (loansId != null)
                {
                    returnsToUpdate.LoanId = (int)loansId;
                }
                if (fineImposed != null)
                {
                    returnsToUpdate.FineImposed = (float)fineImposed;
                }
                return await _returnsRepository.UpdateReturns(returnsToUpdate);
            }
            throw new NotImplementedException();
        }
    }
}

