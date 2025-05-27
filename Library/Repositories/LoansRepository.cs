using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface ILoansRepository
    {
        Task<List<Loans>> GetAll();
        Task<Loans> GetLoans(int id);
        Task<Loans> CreateLoans(int userId, int bookId, DateTime loandDate);
        Task<Loans> UpdateLoans(Loans Loans);
        Task<Loans> DeleteLoans(Loans Loans);
    }
    public class LoansRepository : ILoansRepository
    {
        private readonly LibraryDbContext _db;
        public LoansRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<Loans> CreateLoans(int userId, int bookId, DateTime loandDate)
        {
            Loans newLoans = new Loans
            {
                UserId = userId,
                BookId = bookId,
                LoanDate = loandDate,
                CreateDate = null,
                State = false
            };
            await _db.Loans.AddAsync(newLoans);
            await _db.SaveChangesAsync();
            return newLoans;
        }
        public async Task<Loans> DeleteLoans(Loans Loans)
        {
            _db.Loans.Attach(Loans);
            _db.Entry(Loans).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Loans;
        }
        public async Task<List<Loans>> GetAll()
        {
            return await _db.Loans.ToListAsync();
        }
        public async Task<Loans> GetLoans(int id)
        {
            return await _db.Loans.FirstOrDefaultAsync(l => l.LoanId == id);
        }
        public async Task<Loans> UpdateLoans(Loans Loans)
        {
            Loans loansUpdate = await _db.Loans.FindAsync(Loans.LoanId);
            if (loansUpdate != null)
            {
                loansUpdate.UserId = Loans.UserId;
                loansUpdate.BookId = Loans.BookId;
                loansUpdate.LoanDate = Loans.LoanDate;
                await _db.SaveChangesAsync();
                return loansUpdate;
            }
            return null;
        }
    }
}
