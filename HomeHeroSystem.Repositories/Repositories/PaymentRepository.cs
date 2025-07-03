using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Repositories.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(HomeHeroContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                return await _dbSet
                    .Include(p => p.Booking)
                    .ThenInclude(b => b.Service)
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by ID: {PaymentId}", paymentId);
                throw;
            }
        }

        public async Task<List<Payment>> GetPaymentsByBookingIdAsync(int bookingId)
        {
            try
            {
                return await _dbSet
                    .Where(p => p.BookingId == bookingId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments for booking: {BookingId}", bookingId);
                throw;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            try
            {
                var payment = await _dbSet.FindAsync(paymentId);
                if (payment == null)
                {
                    _logger.LogWarning("Payment not found: {PaymentId}", paymentId);
                    return false;
                }

                payment.Status = status;
                _context.Update(payment);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status: {PaymentId}", paymentId);
                throw;
            }
        }
    }
}
