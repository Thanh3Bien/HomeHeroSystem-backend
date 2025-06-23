using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHeroSystem.Services.Models.Address
{
    public class CreateAddressRequest
    {
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Ward is required")]
        public string Ward { get; set; } = null!;

        [Required(ErrorMessage = "District is required")]
        public string District { get; set; } = null!;

        public string City { get; set; } = "Ho Chi Minh City";
    }
}
