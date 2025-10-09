using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.SubjectRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<ApiResponse<List<SubjectResponseDto>>> GetAllAsync()
        {
            try
            {
                var subjects = await _subjectRepository.GetAllAsync();

                if (subjects == null)
                {
                    return ApiResponse<List<SubjectResponseDto>>.FailResponse("Get All Subject fail", "subjects are empty");
                }

                var result = subjects.Select(MapToDTO).ToList();

                return ApiResponse<List<SubjectResponseDto>>.SuccessResponse(result, "Get All Subjecct Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SubjectResponseDto>>.FailResponse("Get All Subject fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SubjectResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existSubject = await _subjectRepository.GetByIdAsync(id);

                if (existSubject == null)
                {
                    return ApiResponse<SubjectResponseDto>.FailResponse("Get Subject By Id Fail", "Subject are not exist");
                }

                var result = MapToDTO(existSubject);

                return ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Get Subject by Id Succcessfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponseDto>.FailResponse("Get Subject By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SubjectResponseDto>> CreateNewSubjectAsync(SubjectRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<SubjectResponseDto>.FailResponse("Create New Subject Fail", "New Subject are null");
                }

                var newSubject = MapToEntity(request);

                await _subjectRepository.AddAsync(newSubject);

                var result = MapToDTO(newSubject);

                return ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Create New Subject sucessfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponseDto>.FailResponse("Create New Subject Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SubjectResponseDto>> UpdateSubjectByIdAsync(Guid id, SubjectRequestDto newSubject)
        {
            try
            {
                var existSubject = await _subjectRepository.GetByIdAsync(id);

                if (existSubject == null)
                {
                    return ApiResponse<SubjectResponseDto>.FailResponse("Update Subject By Id Fail", "Subject are not exist");
                }

                if (newSubject == null)
                {
                    return ApiResponse<SubjectResponseDto>.FailResponse("Update subject By Id Fail", "new Subject are not exist");
                }

                existSubject.Name = newSubject.Name;
                existSubject.Priority = newSubject.Priority;
                existSubject.ProcessId = newSubject.ProcessId;
                existSubject.ScheduleId = newSubject.ScheduleId;

                await _subjectRepository.UpdateAsync(existSubject);

                var result = MapToDTO(existSubject);

                return ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Update subject by id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponseDto>.FailResponse("Update subject By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<SubjectResponseDto>> DeleteSubjectByIdAsync(Guid id)
        {
            try
            {
                var existSubject = await _subjectRepository.GetByIdAsync(id);

                if (existSubject == null)
                {
                    return ApiResponse<SubjectResponseDto>.FailResponse("Delete Subject By Id Fail", "Subject are not exist");
                }

                var result = MapToDTO(existSubject);

                await _subjectRepository.DeleteAsync(existSubject);

                return ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Delete Subject By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponseDto>.FailResponse("Delete Subject By Id Fail", ex.Message);
            }
        }

        public SubjectResponseDto MapToDTO(Subject subject)
        {
            return new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Priority = subject.Priority,
                ProcessId = subject.ProcessId,
                ScheduleId = subject.ScheduleId
            };
        }

        public Subject MapToEntity(SubjectRequestDto request)
        {
            return new Subject
            {
                Name = request.Name,
                Priority = request.Priority,
                ProcessId = request.ProcessId,
                ScheduleId = request.ScheduleId
            };
        }
    }
}
