using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.IntructorRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<ApiResponse<List<InstructorResponseDto>>> GetAllAsync()
        {
            try
            {
                var instructors = await _instructorRepository.GetAllAsync();

                if (!instructors.Any())
                {
                    return ApiResponse<List<InstructorResponseDto>>.FailResponse("Get All Instructor Fail", "Instructor Are Empty");
                }
                var result = instructors.Select(MapToDTO).ToList();

                return ApiResponse<List<InstructorResponseDto>>.SuccessResponse(result, "Get All Instructor Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<InstructorResponseDto>>.FailResponse("Get All Instructor Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<InstructorResponseDto>> GetInstructorByIdAsync(Guid id)
        {
            try
            {
                var existInstructor = await _instructorRepository.GetByIdAsync(id);
                if (existInstructor == null)
                {
                    return ApiResponse<InstructorResponseDto>.FailResponse("Get Instructor By Id Fail", "Instructor Are Not Exist");
                }

                var result = MapToDTO(existInstructor);
                return ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Get Instructor By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<InstructorResponseDto>.FailResponse("Get Instructor By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<InstructorResponseDto>> CreateNewInstructorAsync(InstructorRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<InstructorResponseDto>.FailResponse("Create New Instructor Fail", "New Intructor is Null");
                }

                var newInstructor = MapToEntity(request);

                await _instructorRepository.AddAsync(newInstructor);

                var result = MapToDTO(newInstructor);

                return ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Create New Instructor Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<InstructorResponseDto>.FailResponse("Create New Instructor Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<InstructorResponseDto>> UpdateInstructorByIdAsync(Guid id, InstructorRequestDto newInstructor)
        {
            try
            {
                var existInstructor = await _instructorRepository.GetInstructorByIdAsync(id);

                if (existInstructor == null)
                {
                    return ApiResponse<InstructorResponseDto>.FailResponse("Update Instructor By Id Fail", "Instructor Are Not Exist");
                }
                if (newInstructor == null)
                {
                    return ApiResponse<InstructorResponseDto>.FailResponse("Update Instructor By Id Fail", "Instructor is Null");
                }
                existInstructor.Name = newInstructor.Name;
                existInstructor.Description = newInstructor.Description;
                existInstructor.Contact = newInstructor.Contact;
                existInstructor.Experience = newInstructor.Experience;
                existInstructor.CourseId = newInstructor.CourseId;

                await _instructorRepository.UpdateAsync(existInstructor);

                var result = MapToDTO(existInstructor);

                return ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Update Instructor By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<InstructorResponseDto>.FailResponse("Update Instructor By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<InstructorResponseDto>> DeleteInstructorByIdAsync(Guid id)
        {
            try
            {
                var existInstructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (existInstructor == null)
                {
                    return ApiResponse<InstructorResponseDto>.FailResponse("Delete Instructor By Id Fail", "Instructor Are Not Exist");
                }

                var result = MapToDTO(existInstructor);

                await _instructorRepository.DeleteAsync(existInstructor);
                
                return ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Delete Instructor By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<InstructorResponseDto>.FailResponse("Delete Instructor By Id Fail",ex.Message);
            }
        }


        protected Instructor MapToEntity(InstructorRequestDto newInstructor)
        {
            return new Instructor
            {
                Name = newInstructor.Name,
                Description = newInstructor.Description,
                Experience = newInstructor.Experience,
                Contact = newInstructor.Contact,
                CourseId = newInstructor.CourseId,
            };
        }

        public InstructorResponseDto MapToDTO(Instructor instructor)
        {
            return new InstructorResponseDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Description = instructor.Description,
                Experience = instructor.Experience,
                Contact = instructor.Contact,
                CourseId = instructor.CourseId,
            };
        }
    }
}
