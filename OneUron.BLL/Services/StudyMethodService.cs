using OneUron.BLL.DTOs.StudyMethodDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.StudyMethodRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class StudyMethodService : IStudyMethodService
    {
        private readonly IStudyMethodRepository _studyMethodRepository;

        public StudyMethodService(IStudyMethodRepository studyMethodRepository)
        {
            _studyMethodRepository = studyMethodRepository;
        }

        public async Task<ApiResponse<List<StudyMethodResponseDto>>> GetALlAsync()
        {
            try
            {
                var studyMethods = await _studyMethodRepository.GetALlAsync();

                if (!studyMethods.Any())
                {
                    return ApiResponse<List<StudyMethodResponseDto>>.FailResponse("Get All Study Method Fail", "Study Method Are Empty");
                }

                var result = studyMethods.Select(MapToDTO).ToList();

                return ApiResponse<List<StudyMethodResponseDto>>.SuccessResponse(result, "Get All Study Method Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudyMethodResponseDto>>.FailResponse("Get All Study Method Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<StudyMethodResponseDto>> GetByIdAsyc(Guid id)
        {
            try
            {
                var existStudyMethod = await _studyMethodRepository.GetByIdAsync(id);

                if (existStudyMethod == null)
                {
                    return ApiResponse<StudyMethodResponseDto>.FailResponse("Get Study Method By Id Fail", "Study Method Are Note Exist");
                }

                var result = MapToDTO(existStudyMethod);

                return ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Get Study Method By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyMethodResponseDto>.FailResponse("Get Study Method By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<StudyMethodResponseDto>> CreateNewStudyMethodAsync(StudyMethodRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<StudyMethodResponseDto>.FailResponse("Create New Study Method Fail", "New Study Method is Null");
                }

                var newStudyMethod = MapToEntity(request);

                await _studyMethodRepository.AddAsync(newStudyMethod);

                var result = MapToDTO(newStudyMethod);

                return ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Create New Study Method Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyMethodResponseDto>.FailResponse("Create New Study Method Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<StudyMethodResponseDto>> UpdateStudyMethodbyIdAsync(Guid id, StudyMethodRequestDto newStudyMethod)
        {
            try
            {
                var existStudyMethod = await _studyMethodRepository.GetByIdAsync(id);

                if (existStudyMethod == null)
                {
                    return ApiResponse<StudyMethodResponseDto>.FailResponse("Update Study Method By Id Fail", "Study Method Are Not Exist");
                }

                if (newStudyMethod == null)
                {
                    return ApiResponse<StudyMethodResponseDto>.FailResponse("Update Study Method By Id Fail", "Study Method is Null");
                }

                existStudyMethod.UpdateDate = DateTime.UtcNow;
                existStudyMethod.IsDeleted = newStudyMethod.IsDeleted;
                existStudyMethod.MethodId = newStudyMethod.MethodId;
                existStudyMethod.UserId = newStudyMethod.UserId;

                await _studyMethodRepository.UpdateAsync(existStudyMethod);

                var result = MapToDTO(existStudyMethod);

                return ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Update Study Method Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyMethodResponseDto>.FailResponse("Update Study Method By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<StudyMethodResponseDto>> DeleteStudyMethodbyIdAsync(Guid id)
        {
            try
            {
                var existStudyMethod = await _studyMethodRepository.GetByIdAsync(id);

                if (existStudyMethod == null)
                {
                    return ApiResponse<StudyMethodResponseDto>.FailResponse("Update Study Method By Id Fail", "Study Method Are Not Exist");
                }

                existStudyMethod.IsDeleted = true;

                var result = MapToDTO(existStudyMethod);

                await _studyMethodRepository.UpdateAsync(existStudyMethod);

                return ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Study Method Delete Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudyMethodResponseDto>.FailResponse("Update Study Method By Id Fail", ex.Message);
            }
        }

        protected StudyMethod MapToEntity(StudyMethodRequestDto request)
        {
            return new StudyMethod
            {
                IsDeleted = request.IsDeleted,
                MethodId = request.MethodId,
                UserId = request.UserId,
            };
        }

        protected StudyMethodResponseDto MapToDTO(StudyMethod studyMethod)
        {
            return new StudyMethodResponseDto
            {
                Id = studyMethod.Id,
                UpdateDate = studyMethod.UpdateDate,
                IsDeleted = studyMethod.IsDeleted,
                MethodId = studyMethod.MethodId,
                UserId = studyMethod.UserId,
            };
        }
    }
}
