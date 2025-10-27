using FluentValidation;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.SkillRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IValidator<SkillRequestDto> _skillRequestValidator;

        public SkillService(ISkillRepository skillRepository, IValidator<SkillRequestDto> skillRequestValidator)
        {
            _skillRepository = skillRepository;
            _skillRequestValidator = skillRequestValidator;
        }

       
        public async Task<List<SkillResponseDto>> GetAllAsync()
        {
            var skills = await _skillRepository.GetAllAsync();

            if (skills == null || !skills.Any())
                throw new ApiException.NotFoundException("Không tìm thấy kĩ năng nào.");

            return skills.Select(MapToDTO).ToList();
        }

        
        public async Task<SkillResponseDto> GetByIdAsync(Guid id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);
            if (skill == null)
                throw new ApiException.NotFoundException($"Kĩ năng của ID {id} không tìm thấy.");

            return MapToDTO(skill);
        }

        
        public async Task<SkillResponseDto> CreateNewSkillAsync(SkillRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Kĩ năng mới không được để trống");

            var validationResult = await _skillRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newSkill = MapToEntity(request);
            await _skillRepository.AddAsync(newSkill);

            return MapToDTO(newSkill);
        }

        
        public async Task<SkillResponseDto> UpdateSkillByIdAsync(Guid id, SkillRequestDto request)
        {
            var existingSkill = await _skillRepository.GetByIdAsync(id);
            if (existingSkill == null)
                throw new ApiException.NotFoundException($"Kĩ năng của ID {id} Không tìm thấy.");

            if (request == null)
                throw new ApiException.BadRequestException("Kĩ năng mới không được để trống.");

            var validationResult = await _skillRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existingSkill.Text = request.Text;
            existingSkill.CourseId = request.CourseId;

            await _skillRepository.UpdateAsync(existingSkill);

            return MapToDTO(existingSkill);
        }

        
        public async Task<SkillResponseDto> DeleteSkillByIdAsync(Guid id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);
            if (skill == null)
                throw new ApiException.NotFoundException($"Kĩ năng của  ID {id} Không tìm thấy.");

            await _skillRepository.DeleteAsync(skill);
            return MapToDTO(skill);
        }

      
        protected Skill MapToEntity(SkillRequestDto request)
        {
            return new Skill
            {
                Text = request.Text,
                CourseId = request.CourseId
            };
        }

        public SkillResponseDto MapToDTO(Skill skill)
        {
            if (skill == null) return null;

            return new SkillResponseDto
            {
                Id = skill.Id,
                Text = skill.Text,
                CourseId = skill.CourseId
            };
        }
    }
}
