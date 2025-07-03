using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Payment
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? TransactionCode { get; set; }
        public string FormattedAmount { get; set; } = null!;
        public string FormattedDate { get; set; } = null!;

        public string? ServiceName { get; set; }
        public string? BookingCode { get; set; }
    }
}
