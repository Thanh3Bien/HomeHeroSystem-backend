using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Payment
{
    public class CreatePaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
        public PaymentResponse? Data { get; set; }
    }
}
