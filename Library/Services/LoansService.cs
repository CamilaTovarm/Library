using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface ILoansService
    {
        Task<List<Loans>> GetAll();
        Task<Loans> GetLoan(int id);
        Task<Loans> CreateLoan(int userId, int bookId, DateTime loanDate);
        Task<Loans> UpdateLoan(int id, int? userId = null, int? bookId = null, DateTime? loanDate = null);
        Task<Loans> DeleteLoan(int id);
    }
    public class LoansService : ILoansService
    {
        public readonly ILoansRepository _loansRepository;
        public LoansService(ILoansRepository loansRepository)
        {
            _loansRepository = loansRepository;
        }
        public async Task<Loans> CreateLoan(int userId, int bookId, DateTime loanDate)
        {
            return await _loansRepository.CreateLoans(userId, bookId, loanDate);
        }
        public async Task<Loans> DeleteLoan(int id)
        {
            Loans loanToDelete = await _loansRepository.GetLoans(id);
            if (loanToDelete == null)
            {
                throw new Exception($"This Loan with the Id {id} doesn't exist.");
            }
            loanToDelete.State = true;
            loanToDelete.CreateDate = DateTime.Now;
            return await _loansRepository.DeleteLoans(loanToDelete);
        }
        public async Task<List<Loans>> GetAll()
        {
            return await _loansRepository.GetAll();
        }
        public async Task<Loans> GetLoan(int id)
        {
            return await _loansRepository.GetLoans(id);
        }
        public async Task<Loans> UpdateLoan(int id, int? userId = null, int? bookId = null, DateTime? loanDate = null)
        {
            Loans loanToUpdate = await _loansRepository.GetLoans(id);
            if (loanToUpdate != null)
            {
                if (userId != null)
                {
                    loanToUpdate.UserId = (int)userId;
                }
                if (bookId != null)
                {
                    loanToUpdate.BookId = (int)bookId;
                }
                if (loanDate != null)
                {
                    loanToUpdate.LoanDate = (DateTime)loanDate;
                }
                return await _loansRepository.UpdateLoans(loanToUpdate);
            }
            throw new NotImplementedException();
        }
    }
}
