using System;
using System.Collections.Generic;

namespace HomeHeroSystem.Repositories.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int TechnicianId { get; set; }

    public int ServiceId { get; set; }

    public DateTime BookingDate { get; set; }

    public string Status { get; set; } = null!;

    public int AddressId { get; set; }

    public string? Note { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? UrgencyLevel { get; set; }

    public decimal? UrgencyFee { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerPhone { get; set; }

    public string? PreferredTimeSlot { get; set; }

    public string? ProblemDescription { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Service Service { get; set; } = null!;

    public virtual Technician Technician { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
