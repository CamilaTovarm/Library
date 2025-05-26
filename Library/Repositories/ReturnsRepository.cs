using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IReturnsRepository
    {
        Task<List<Returns>> GetAll();
        Task<Returns> GetReturns(int id);
        Task<Returns> CreateReturns(DateTime returnDate, int loanId, float fineImposed);
        Task<Returns> UpdateReturns(Returns returns);
        Task<Returns> DeleteReturns(Returns returns);
    }
    public class ReturnsRepository
    {
        private readonly LibraryDbContext _db;
        public ReturnsRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<Returns> CreateReturns(DateTime returnDate, int loanId, float fineImposed)
        {
            Loans? loans = _db.Loans.FirstOrDefault(ut => ut.LoanId == loanId);


            Returns newReturns = new Returns
            {
                ReturnDate = returnDate,
                LoanId = loanId,
                FineImposed = fineImposed,
                CreateDate = DateTime.Now,
                State = true
            };
            await _db.Returns.AddAsync(newReturns);
            _db.SaveChanges();
            return newReturns;
        }
        public async Task<Returns> DeleteReturns(Returns returns)
        {
            _db.Returns.Attach(returns);
            _db.Entry(returns).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return returns;
        }
        public async Task<List<Returns>> GetAll()
        {
            return await _db.Returns.ToListAsync();
        }
        public async Task<Returns> GetReturns(int id)
        {
            return await _db.Returns.FirstOrDefaultAsync(l => l.ReturnId == id);
        }
        public async Task<Returns> UpdateReturns(Returns returns)
        {
            Returns returnsUpdate = await _db.Returns.FindAsync(returns.ReturnId);

            if (returnsUpdate != null)
            {
                returnsUpdate.ReturnDate = returns.CreateDate;
                returnsUpdate.LoanId = returns.LoanId;
                returnsUpdate.FineImposed = returns.FineImposed;
                await _db.SaveChangesAsync();
            }
            return returnsUpdate;
        }
    }
}