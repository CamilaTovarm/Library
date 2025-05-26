using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IFeeRepository
    {
        Task<List<Fee>> GetAllFee();
        Task<Fee> GetFeeById(int id);
        Task<Fee> CreateFee(int daysMin, int daysMax, float feeValue, string feeDescription);
        Task<Fee> UpdateFee(Fee fee);
        Task<Fee> DeleteFee(Fee fee);
    }
    public class FeeRepository : IFeeRepository
    {
        private readonly LibraryDbContext _db;
        public FeeRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<Fee> CreateFee(int daysMin, int daysMax, float feeValue, string feeDescription)
        {
            Fee newFee = new Fee
            {
                DaysMin = daysMin,
                DaysMax = daysMax,
                FeeValue = feeValue,
                FeeDescription = feeDescription,
                CreateDate = null,
                State = false
            };
            await _db.fee.AddAsync(newFee);
            _db.SaveChanges();
            return newFee;
        }

        public async Task<Fee> DeleteFee(Fee fee)
        {
            _db.fee.Attach(fee); //Llamamos la actualizacion
            _db.Entry(fee).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return fee;
        }

        public async Task<List<Fee>> GetAllFee()
        {
            return await _db.fee.ToListAsync();
        }

        public async Task<Fee> GetFeeById(int id)
        {
            return await _db.fee.FirstOrDefaultAsync(u => u.FeeId == id);
        }

        public async Task<Fee> UpdateFee(Fee fee)
        {
            Fee feeUpdate = await _db.fee.FindAsync(fee.FeeId);

            if (feeUpdate != null)
            {
                feeUpdate.DaysMin = fee.DaysMin;
                feeUpdate.DaysMax = fee.DaysMax;
                feeUpdate.FeeValue = fee.FeeValue;
                feeUpdate.FeeDescription = fee.FeeDescription;
                await _db.SaveChangesAsync();
            }

            return feeUpdate;
        }
    }
}
