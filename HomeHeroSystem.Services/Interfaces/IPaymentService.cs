using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Services.Models.Payment;

namespace HomeHeroSystem.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        Task<UpdatePaymentStatusResponse> UpdatePaymentStatusAsync(int paymentId, UpdatePaymentStatusRequest request);
        Task<PaymentResponse?> GetPaymentByIdAsync(int paymentId);
    }
}
