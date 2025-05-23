using System;
using System.Collections.Generic;

namespace HomeHeroSystem.Repositories.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int TechnicianId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public bool? IsReviewed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Technician Technician { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
