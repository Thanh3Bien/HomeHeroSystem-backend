using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;

namespace HomeHeroSystem.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
        Task<List<Payment>> GetPaymentsByBookingIdAsync(int bookingId);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, string status);
    }
}
