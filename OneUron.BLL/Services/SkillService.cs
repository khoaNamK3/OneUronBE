using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.ExceptionHandle;
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

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<ApiResponse<List<SkillResponseDto>>> GetAllAsync()
        {
            try
            {
                var skills = await _skillRepository.GetAllAsync();

                if (!skills.Any())
                {
                    return ApiResponse<List<SkillResponseDto>>.FailResponse("Get All Skill Fail", "Skills Are Empty");
                }

                var result = skills.Select(MapToDTO).ToList();
                return ApiResponse<List<SkillResponseDto>>.SuccessResponse(result, "Get All Skill Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SkillResponseDto>>.FailResponse("Get All Skill Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SkillResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existSkill = await _skillRepository.GetByIdAsync(id);
                if (existSkill == null)
                {
                    return ApiResponse<SkillResponseDto>.FailResponse("Get Skill By Id Fail", "Skills Are Not Exist");
                }

                var result = MapToDTO(existSkill);
                return ApiResponse<SkillResponseDto>.SuccessResponse(result, "get  Skill By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<SkillResponseDto>.FailResponse("Get Skill By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SkillResponseDto>> CreateNewSkillAsync(SkillRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<SkillResponseDto>.FailResponse("Create New Skill Fail", "New Skill Are Empty");
                }
                var newSkill = MapToEnitity(request);

                await _skillRepository.AddAsync(newSkill);

                var result = MapToDTO(newSkill);
                return ApiResponse<SkillResponseDto>.SuccessResponse(result, "Create New Skill Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<SkillResponseDto>.FailResponse("Create New Skill Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SkillResponseDto>> UpdateSkillByIdAsync(Guid id, SkillRequestDto newSkill)
        {
            try
            {
                var existSkill = await _skillRepository.GetByIdAsync(id);
                if (existSkill == null)
                {
                    return ApiResponse<SkillResponseDto>.FailResponse("Update Skill By Id Fail", "Skills Are Not Exist");
                }

                if (newSkill == null)
                {
                    return ApiResponse<SkillResponseDto>.FailResponse("Create New Skill Fail", "New Skill Are Empty");
                }
                existSkill.Text = newSkill.Text;
                existSkill.CourseId = newSkill.CourseId;

                await _skillRepository.UpdateAsync(existSkill);

                var result = MapToDTO(existSkill);

                return ApiResponse<SkillResponseDto>.SuccessResponse(result, "Update Skill Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<SkillResponseDto>.FailResponse("Update New Skill Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SkillResponseDto>> DeleteSkillByIdAsync(Guid id)
        {
            try
            {
                var existSkill = await _skillRepository.GetByIdAsync(id);
                if (existSkill == null)
                {
                    return ApiResponse<SkillResponseDto>.FailResponse("Delete SKill Fail", "Skill Are Not Exist");
                }

                var result = MapToDTO(existSkill);
                await _skillRepository.DeleteAsync(existSkill);
                return ApiResponse<SkillResponseDto>.SuccessResponse(result, "Delete Skill Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<SkillResponseDto>.FailResponse("Delete SKill Fail",ex.Message);
            }
        }

        protected Skill MapToEnitity(SkillRequestDto request)
        {
            return new Skill
            {
                Text = request.Text,
                CourseId = request.CourseId,
            };
        }

        protected SkillResponseDto MapToDTO(Skill skill)
        {
            return new SkillResponseDto
            {
                Id = skill.Id,
                Text = skill.Text,
                CourseId = skill.CourseId,
            };
        }
    }
}
