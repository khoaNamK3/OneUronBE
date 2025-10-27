using FluentValidation;
using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodProRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodProService : IMethodProSerivce
    {
        private readonly IMethodProRepository _methodProRepository;
        private readonly IValidator<MethodProRequestDto> _methodProValidator;

        public MethodProService(
            IMethodProRepository methodProRepository,
            IValidator<MethodProRequestDto> methodProValidator)
        {
            _methodProRepository = methodProRepository;
            _methodProValidator = methodProValidator;
        }

        
        public async Task<List<MethodProResponseDto>> GetAllAsync()
        {
            var methodPros = await _methodProRepository.GetAllAsync();

            if (methodPros == null || !methodPros.Any())
                throw new ApiException.NotFoundException("Không tìm thấy lợi ích của phương pháp.");

            return methodPros.Select(MapToDTO).ToList();
        }

       
        public async Task<MethodProResponseDto> GetByIdAsync(Guid id)
        {
            var existMethodPro = await _methodProRepository.GetByIdAsync(id);
            if (existMethodPro == null)
                throw new ApiException.NotFoundException($"Lợi ích phượng pháp của ID {id} không tìm thấy.");

            return MapToDTO(existMethodPro);
        }

      
        public async Task<MethodProResponseDto> CreateNewMethodProAsync(MethodProRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Lợi ích mới của phương pháp không được để trống ");

            var validationResult = await _methodProValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newMethodPro = MapToEntity(request);

            await _methodProRepository.AddAsync(newMethodPro);

            return MapToDTO(newMethodPro);
        }

      
        public async Task<MethodProResponseDto> UpdateMethodProByIdAsync(Guid id, MethodProRequestDto newMethodPro)
        {
            var existMethodPro = await _methodProRepository.GetByIdAsync(id);
            if (existMethodPro == null)
                throw new ApiException.NotFoundException($"Lợi ích Phương pháp của  ID {id} Không tìm thấy.");

            if (newMethodPro == null)
                throw new ApiException.BadRequestException("Lợi ích mới của phương pháp không được để trống.");

            var validationResult = await _methodProValidator.ValidateAsync(newMethodPro);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existMethodPro.Pro = newMethodPro.Pro;
            existMethodPro.MethodId = newMethodPro.MethodId;

            await _methodProRepository.UpdateAsync(existMethodPro);

            return MapToDTO(existMethodPro);
        }

        public async Task<MethodProResponseDto> DeleteMethodProByIdAsync(Guid id)
        {
            var existMethodPro = await _methodProRepository.GetByIdAsync(id);
            if (existMethodPro == null)
                throw new ApiException.NotFoundException($"Lợi ích phương pháp của  ID {id} không tìm thấy.");

            await _methodProRepository.DeleteAsync(existMethodPro);

            return MapToDTO(existMethodPro);
        }

     
        protected MethodPro MapToEntity(MethodProRequestDto request)
        {
            return new MethodPro
            {
                Pro = request.Pro,
                MethodId = request.MethodId
            };
        }

        public MethodProResponseDto MapToDTO(MethodPro methodPro)
        {
            if (methodPro == null) return null;

            return new MethodProResponseDto
            {
                Id = methodPro.Id,
                Pro = methodPro.Pro,
                MethodId = methodPro.MethodId
            };
        }
    }
}
