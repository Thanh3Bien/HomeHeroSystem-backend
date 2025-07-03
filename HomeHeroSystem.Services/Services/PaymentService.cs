using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Payment;

namespace HomeHeroSystem.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            try
            {
                // Validate booking exists
                var booking = await _unitOfWork.Bookings.GetByIdAsync(request.BookingId);
                if (booking == null)
                {
                    return new CreatePaymentResponse
                    {
                        IsSuccess = false,
                        Message = "Booking not found",
                        Data = null
                    };
                }

                // Check if payment already exists for this booking
                var existingPayments = await _unitOfWork.Payments.GetPaymentsByBookingIdAsync(request.BookingId);
                if (existingPayments.Any(p => p.Status != "Cancelled"))
                {
                    return new CreatePaymentResponse
                    {
                        IsSuccess = false,
                        Message = "Payment already exists for this booking",
                        Data = null
                    };
                }

                // Create new payment
                var payment = new Payment
                {
                    BookingId = request.BookingId,
                    Amount = request.Amount,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = request.PaymentMethod,
                    Status = "Unpaid", // Initial status
                    TransactionCode = request.TransactionCode
                };

                var createdPayment = _unitOfWork.Payments.AddEntity(payment);
                await _unitOfWork.CompleteAsync();

                // Get complete payment info for response
                var paymentResponse = await GetPaymentByIdAsync(createdPayment.PaymentId);

                return new CreatePaymentResponse
                {
                    IsSuccess = true,
                    Message = "Payment created successfully",
                    Data = paymentResponse
                };
            }
            catch (Exception ex)
            {
                return new CreatePaymentResponse
                {
                    IsSuccess = false,
                    Message = $"Error creating payment: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<UpdatePaymentStatusResponse> UpdatePaymentStatusAsync(int paymentId, UpdatePaymentStatusRequest request)
        {
            try
            {
                // Get existing payment
                var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(paymentId);
                if (payment == null)
                {
                    return new UpdatePaymentStatusResponse
                    {
                        IsSuccess = false,
                        Message = "Payment not found",
                        Data = null
                    };
                }

                // Update status
                var updateResult = await _unitOfWork.Payments.UpdatePaymentStatusAsync(paymentId, request.Status);
                if (!updateResult)
                {
                    return new UpdatePaymentStatusResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to update payment status",
                        Data = null
                    };
                }

                await _unitOfWork.CompleteAsync();

                // Get updated payment info for response
                var updatedPayment = await GetPaymentByIdAsync(paymentId);

                return new UpdatePaymentStatusResponse
                {
                    IsSuccess = true,
                    Message = $"Payment status updated to {request.Status}",
                    Data = updatedPayment
                };
            }
            catch (Exception ex)
            {
                return new UpdatePaymentStatusResponse
                {
                    IsSuccess = false,
                    Message = $"Error updating payment status: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<PaymentResponse?> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(paymentId);
                if (payment == null) return null;

                return new PaymentResponse
                {
                    PaymentId = payment.PaymentId,
                    BookingId = payment.BookingId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod,
                    Status = payment.Status,
                    TransactionCode = payment.TransactionCode,
                    FormattedAmount = $"{payment.Amount:N0} ₫",
                    FormattedDate = payment.PaymentDate.ToString("dd/MM/yyyy HH:mm"),
                    ServiceName = payment.Booking?.Service?.ServiceName,
                    BookingCode = $"BK{payment.BookingId:D8}"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
