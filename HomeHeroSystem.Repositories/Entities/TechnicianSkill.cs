using System;
using System.Collections.Generic;

namespace HomeHeroSystem.Repositories.Entities;

public partial class TechnicianSkill
{
    public int TechSkillId { get; set; }

    public int TechnicianId { get; set; }

    public int SkillId { get; set; }

    public int? ProficiencyLevel { get; set; }

    public int? YearsOfExperience { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Skill Skill { get; set; } = null!;

    public virtual Technician Technician { get; set; } = null!;
}
