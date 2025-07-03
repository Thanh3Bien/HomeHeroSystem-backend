using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Payment
{
    public class UpdatePaymentStatusRequest
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = null!;

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
