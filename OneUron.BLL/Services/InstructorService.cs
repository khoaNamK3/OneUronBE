using FluentValidation;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
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
        private readonly IValidator<InstructorRequestDto> _instructorValidator;

        public InstructorService(
            IInstructorRepository instructorRepository,
            IValidator<InstructorRequestDto> instructorValidator)
        {
            _instructorRepository = instructorRepository;
            _instructorValidator = instructorValidator;
        }

        public async Task<List<InstructorResponseDto>> GetAllAsync()
        {
            var instructors = await _instructorRepository.GetAllAsync();

            if (instructors == null || !instructors.Any())
                throw new ApiException.NotFoundException("Không tìm thấy giảng viên.");

            return instructors.Select(MapToDTO).ToList();
        }


        public async Task<InstructorResponseDto> GetInstructorByIdAsync(Guid id)
        {
            var existInstructor = await _instructorRepository.GetInstructorByIdAsync(id);
            if (existInstructor == null)
                throw new ApiException.NotFoundException($"Giảng viên của  ID {id} Không tìm thấy.");

            return MapToDTO(existInstructor);
        }

  
        public async Task<InstructorResponseDto> CreateNewInstructorAsync(InstructorRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Giảng viên mới không được để trống");

            var validationResult = await _instructorValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newInstructor = MapToEntity(request);

            await _instructorRepository.AddAsync(newInstructor);

            return MapToDTO(newInstructor);
        }

        public async Task<InstructorResponseDto> UpdateInstructorByIdAsync(Guid id, InstructorRequestDto newInstructor)
        {
            var existInstructor = await _instructorRepository.GetInstructorByIdAsync(id);
            if (existInstructor == null)
                throw new ApiException.NotFoundException($"Giảng viên của  ID {id} Không tìm thấy.");

            if (newInstructor == null)
                throw new ApiException.BadRequestException("Giảng viên mới không được để trống");

            var validationResult = await _instructorValidator.ValidateAsync(newInstructor);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existInstructor.Name = newInstructor.Name;
            existInstructor.Description = newInstructor.Description;
            existInstructor.Contact = newInstructor.Contact;
            existInstructor.Experience = newInstructor.Experience;
            existInstructor.CourseId = newInstructor.CourseId;

            await _instructorRepository.UpdateAsync(existInstructor);

            return MapToDTO(existInstructor);
        }

        public async Task<InstructorResponseDto> DeleteInstructorByIdAsync(Guid id)
        {
            var existInstructor = await _instructorRepository.GetInstructorByIdAsync(id);
            if (existInstructor == null)
                throw new ApiException.NotFoundException($"Giảng viên của  ID {id} Không tìm thấy.");

            await _instructorRepository.DeleteAsync(existInstructor);

            return MapToDTO(existInstructor);
        }

        protected Instructor MapToEntity(InstructorRequestDto newInstructor)
        {
            return new Instructor
            {
                Name = newInstructor.Name,
                Description = newInstructor.Description,
                Experience = newInstructor.Experience,
                Contact = newInstructor.Contact,
                CourseId = newInstructor.CourseId
            };
        }

        public InstructorResponseDto MapToDTO(Instructor instructor)
        {
            if (instructor == null) return null;

            return new InstructorResponseDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Description = instructor.Description,
                Experience = instructor.Experience,
                Contact = instructor.Contact,
                CourseId = instructor.CourseId
            };
        }
    }

}
