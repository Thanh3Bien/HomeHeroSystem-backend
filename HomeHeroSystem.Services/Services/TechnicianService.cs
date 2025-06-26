using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Technician;

namespace HomeHeroSystem.Services.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TechnicianService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public async Task<GetTechnicianResponse> GetTechnicianAsync(GetTechnicianRequest request)
        {
            try
            {
                var (technicians, totalCount) = await _unitOfWork.Technicians.GetTechniciansWithFilterAsync(
                    request.Status,
                    request.Skill,
                    request.Search,
                    request.Page,
                    request.PageSize);

                var technicianItems = new List<TechnicianItem>();
                foreach (var technician in technicians)
                {
                    var skills = await _unitOfWork.Technicians.GetTechnicianSkillsAsync(technician.TechnicianId);
                    var rating = await _unitOfWork.Technicians.GetTechnicianRatingAsync(technician.TechnicianId);
                    var jobsCount = await _unitOfWork.Technicians.GetTechnicianJobsCountAsync(technician.TechnicianId);
                    technicianItems.Add(new TechnicianItem
                    {
                        TechnicianId = technician.TechnicianId,
                        FullName = technician.FullName,
                        Email = technician.Email,
                        Phone = technician.Phone,
                        Skills = skills,
                        Rating = rating,
                        JobsCount = jobsCount,
                        IsActive = technician.IsActive, 
                        JoinDate = technician.CreatedAt
                    });
                }
                return new GetTechnicianResponse
                {
                    Technicians = technicianItems,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<CreateTechnicianResponse> CreateTechnicianAsync(CreateTechnicianRequest request)
        {
            try
            {

                //await ValidateCreateTechnicianRequest(request);

                var technician = new Technician
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Phone = request.Phone,
                    PasswordHash = request.PasswordHash,
                    ExperienceYears = request.ExperienceYears,
                    AddressId = request.AddressId,
                    IsActive = true, 
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    EmailConfirmed = false
                };

                var createdTechnician = _unitOfWork.Technicians.AddEntity(technician);
                await _unitOfWork.CompleteAsync();

                if (request.Skills.Any())
                {
                    foreach (var skillRequest in request.Skills)
                    {
                        var technicianSkill = new TechnicianSkill
                        {
                            TechnicianId = createdTechnician.TechnicianId,
                            SkillId = skillRequest.SkillId,
                            ProficiencyLevel = skillRequest.ProficiencyLevel,
                            YearsOfExperience = skillRequest.YearsOfExperience,
                            CreatedAt = DateTime.Now
                        };

                        _unitOfWork.TechnicianSkills.AddEntity(technicianSkill);
                    }
                    await _unitOfWork.CompleteAsync();
                }


                return new CreateTechnicianResponse
                {
                    TechnicianId = createdTechnician.TechnicianId,
                    FullName = createdTechnician.FullName,
                    Email = createdTechnician.Email,
                    Phone = createdTechnician.Phone,
                    IsActive = (bool)createdTechnician.IsActive,
                    CreatedAt = (DateTime)createdTechnician.CreatedAt,
                    Message = "Technician created successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //private async Task ValidateCreateTechnicianRequest(CreateTechnicianRequest request)
        //{
        //    if (string.IsNullOrWhiteSpace(request.FullName))
        //        throw new ArgumentException("Full name is required");

        //    if (string.IsNullOrWhiteSpace(request.Email))
        //        throw new ArgumentException("Email is required");

        //    if (string.IsNullOrWhiteSpace(request.Phone))
        //        throw new ArgumentException("Phone is required");

        //    if (request.AddressId <= 0)
        //        throw new ArgumentException("Valid Address ID is required");

        //    // 2. Check duplicates
        //    var emailExists = await _unitOfWork.Technicians.isEmailExistAsync(request.Email);
        //    if (emailExists)
        //        throw new ArgumentException("Email already exists");

        //    var phoneExists = await _unitOfWork.Technicians.isPhoneExistAsync(request.Phone);
        //    if (phoneExists)
        //        throw new ArgumentException("Phone number already exists");

        //    // 3. Check address exists
        //    var addressExists = await _unitOfWork.Technicians.isAddressExistAsync(request.AddressId);
        //    if (!addressExists)
        //        throw new ArgumentException("Address not found");

        //    // 4. Validate skills
        //    if (request.Skills.Any())
        //    {
        //        foreach (var skill in request.Skills)
        //        {
        //            if (skill.ProficiencyLevel < 1 || skill.ProficiencyLevel > 5)
        //                throw new ArgumentException("Proficiency level must be between 1 and 5");

        //            var skillExists = await _unitOfWork.TechnicianSkillRepository.IsSkillExistAsync(skill.SkillId);
        //            if (!skillExists)
        //                throw new ArgumentException($"Skill with ID {skill.SkillId} not found");
        //        }
        //    }

        public async Task<UpdateTechnicianResponse> UpdateTechnicianAsync(int id, UpdateTechnicianRequest request)
        {
            try
            {
                // 1. Get existing technician
                var existingTechnician = await _unitOfWork.Technicians.GetByIdAsync(id);
                if (existingTechnician == null || existingTechnician.IsDeleted == true)
                {
                    throw new ArgumentException("Technician not found");
                }

                // 2. Validate request
                await ValidateUpdateTechnicianRequest(request, id);

                await _unitOfWork.BeginTransactionAsync();

                // 4. Update basic information
                UpdateBasicInfo(existingTechnician, request);

                _unitOfWork.Technicians.UpdateEntity(existingTechnician);
                await _unitOfWork.CompleteAsync();

                // 6. Update skills if provided
                if (request.Skills != null)
                {
                    await UpdateTechnicianSkills(id, request.Skills);
                }
                await _unitOfWork.CommitTransactionAsync();


                return new UpdateTechnicianResponse
                {
                    TechnicianId = existingTechnician.TechnicianId,
                    FullName = existingTechnician.FullName,
                    Email = existingTechnician.Email,
                    Phone = existingTechnician.Phone,
                    IsActive = (bool)existingTechnician.IsActive,
                    UpdatedAt = existingTechnician.UpdatedAt ?? DateTime.Now,
                    Message = "Technician updated successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateBasicInfo(Technician technician, UpdateTechnicianRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.FullName))
                technician.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.Email))
                technician.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Phone))
                technician.Phone = request.Phone;

            if (!string.IsNullOrWhiteSpace(request.PasswordHash))
                technician.PasswordHash = request.PasswordHash;

            if (request.ExperienceYears.HasValue)
                technician.ExperienceYears = request.ExperienceYears;

            if (request.AddressId.HasValue)
                technician.AddressId = request.AddressId;

            if (request.IsActive.HasValue)
                technician.IsActive = request.IsActive.Value;

            technician.UpdatedAt = DateTime.Now;
        }

        private async Task UpdateTechnicianSkills(int technicianId, List<TechnicianSkillRequest> newSkills)
        {
            // 1. Delete existing skills
            await _unitOfWork.TechnicianSkills.DeleteByTechnicianIdAsync(technicianId);
            await _unitOfWork.CompleteAsync();

            // 2. Add new skills
            if (newSkills.Any())
            {
                foreach (var skillRequest in newSkills)
                {
                    var technicianSkill = new TechnicianSkill
                    {
                        TechnicianId = technicianId,
                        SkillId = skillRequest.SkillId,
                        ProficiencyLevel = skillRequest.ProficiencyLevel,
                        YearsOfExperience = skillRequest.YearsOfExperience,
                        CreatedAt = DateTime.Now
                    };

                    _unitOfWork.TechnicianSkills.AddEntity(technicianSkill);
                }
                await _unitOfWork.CompleteAsync();
            }
        }

        private async Task ValidateUpdateTechnicianRequest(UpdateTechnicianRequest request, int technicianId)
        {
            // 1. Validate email if provided
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var emailExists = await _unitOfWork.Technicians.IsEmailExistForUpdateAsync(request.Email, technicianId);
                if (emailExists)
                    throw new ArgumentException("Email already exists");
            }

            // 2. Validate phone if provided
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                var phoneExists = await _unitOfWork.Technicians.IsPhoneExistForUpdateAsync(request.Phone, technicianId);
                if (phoneExists)
                    throw new ArgumentException("Phone number already exists");
            }

            // 3. Validate address if provided
            if (request.AddressId.HasValue && request.AddressId > 0)
            {
                var addressExists = await _unitOfWork.Technicians.IsAddressExistAsync(request.AddressId.Value);
                if (!addressExists)
                    throw new ArgumentException("Address not found");
            }

            if (request.Skills != null && request.Skills.Any())
            {
                foreach (var skill in request.Skills)
                {
                    if (skill.ProficiencyLevel < 1 || skill.ProficiencyLevel > 5)
                        throw new ArgumentException("Proficiency level must be between 1 and 5");

                    var skillExists = await _unitOfWork.TechnicianSkills.IsSkillExistAsync(skill.SkillId);
                    if (!skillExists)
                        throw new ArgumentException($"Skill with ID {skill.SkillId} not found");
                }
            }
        }

        public async Task<DeleteTechnicianResponse> DeleteTechnicianAsync(int id)
        {
            var technician = await _unitOfWork.Technicians.GetByIdAsync(id);
            if (technician == null || technician.IsDeleted == true)
                throw new ArgumentException("Technician not found");

            technician.IsDeleted = true;
            technician.UpdatedAt = DateTime.Now;

            _unitOfWork.Technicians.UpdateEntity(technician);
            await _unitOfWork.CompleteAsync();

            return new DeleteTechnicianResponse { TechnicianId = id };
        }

        public async Task<UpdateStatusResponse> UpdateTechnicianStatusAsync(int id, UpdateStatusRequest request)
        {
            var technician = await _unitOfWork.Technicians.GetByIdAsync(id);
            if (technician == null || technician.IsDeleted == true)
                throw new ArgumentException("Technician not found");

            technician.IsActive = request.IsActive;
            technician.UpdatedAt = DateTime.Now;

            _unitOfWork.Technicians.UpdateEntity(technician);
            await _unitOfWork.CompleteAsync();

            return new UpdateStatusResponse
            {
                TechnicianId = id,
                IsActive = (bool)technician.IsActive,
                UpdatedAt = technician.UpdatedAt.Value
            };
        }


        public async Task<TechnicianStatisticsResponse> GetTechnicianStatisticsAsync()
        {
            var activeTechnicians = await _unitOfWork.Technicians.GetActiveTechniciansCountAsync();
            var inactiveTechnicians = await _unitOfWork.Technicians.GetInactiveTechniciansCountAsync();
            var averageRating = await _unitOfWork.Technicians.GetAverageRatingAsync();
            var totalJobsCompleted = await _unitOfWork.Technicians.GetTotalJobsCompletedAsync();

            return new TechnicianStatisticsResponse
            {
                ActiveTechnicians = activeTechnicians,
                InactiveTechnicians = inactiveTechnicians,
                AverageRating = Math.Round(averageRating, 2),
                TotalJobsCompleted = totalJobsCompleted
            };
        }


        public async Task<IEnumerable<string>> GetTechnicianNamesAsync()
        {
            try
            {
                var technicianNames = await _unitOfWork.Technicians.GetTechnicianNamesAsync();
                return technicianNames.Where(name => !string.IsNullOrEmpty(name))
                                    .OrderBy(name => name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<string>> SearchTechnicianNamesAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return await GetTechnicianNamesAsync();
                }

                var filteredNames = await _unitOfWork.Technicians.SearchTechnicianNamesAsync(keyword.Trim());
                return filteredNames.Where(name => !string.IsNullOrEmpty(name))
                                  .OrderBy(name => name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<TechnicianItem> GetTechnicianByIdAsync(int technicianId)
        {
            try
            {
                var technician = await _unitOfWork.Technicians.GetByIdAsync(technicianId);
                if (technician == null || technician.IsDeleted == true)
                {
                    throw new ArgumentException("Technician not found");
                }
                var skills = await _unitOfWork.Technicians.GetTechnicianSkillsAsync(technician.TechnicianId);
                var rating = await _unitOfWork.Technicians.GetTechnicianRatingAsync(technician.TechnicianId);
                var jobsCount = await _unitOfWork.Technicians.GetTechnicianJobsCountAsync(technician.TechnicianId);
                var technicianDetail = new TechnicianItem
                {
                    TechnicianId = technician.TechnicianId,
                    FullName = technician.FullName,
                    Email = technician.Email,
                    Phone = technician.Phone,
                    ExperienceYears = technician.ExperienceYears,
                    Skills = skills,
                    Rating = rating,
                    JobsCount = jobsCount,
                    IsActive = technician.IsActive ?? false,
                    JoinDate = technician.CreatedAt,
                };
                return technicianDetail;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
