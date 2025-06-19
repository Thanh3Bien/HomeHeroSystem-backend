using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Skill;

namespace HomeHeroSystem.Services.Services
{
    public class SkillService : ISkillService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SkillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetSkillsResponse> GetSkillsAsync()
        {
            var skills = await _unitOfWork.Skills.GetActiveSkillsAsync();

            return new GetSkillsResponse
            {
                Skills = skills.Select(s => new SkillResponse
                {
                    SkillId = s.SkillId,
                    SkillName = s.SkillName,
                    Description = s.Description
                }).ToList()
            };
        }
    }
}
