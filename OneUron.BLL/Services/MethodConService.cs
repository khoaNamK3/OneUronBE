using FluentValidation;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodConRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodConService : IMethodConService
    {
        private readonly IMethodConRepository _methodConRepository;
        private readonly IValidator<MethodConRequestDto> _methodConValidator;

        public MethodConService(IMethodConRepository methodConRepository, IValidator<MethodConRequestDto> methodConValidator)
        {
            _methodConRepository = methodConRepository;
            _methodConValidator = methodConValidator;
        }

        
        public async Task<List<MethodConResponseDto>> GetAllAsync()
        {
            var methodCons = await _methodConRepository.GetAllAsync();

            if (methodCons == null || !methodCons.Any())
                throw new ApiException.NotFoundException("Không tìm thấy tác hại phương pháp học.");

            return methodCons.Select(MapToDTO).ToList();
        }

        
        public async Task<MethodConResponseDto> GetByIdAsync(Guid id)
        {
            var existMethodCon = await _methodConRepository.GetByIdAsync(id);
            if (existMethodCon == null)
                throw new ApiException.NotFoundException($"tác hại Phương pháp học của ID {id} không tìm thấy.");

            return MapToDTO(existMethodCon);
        }

       
        public async Task<MethodConResponseDto> CreateNewMethodConAsync(MethodConRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("tác hại phương pháp học mới không được để trống.");

            var validationResult = await _methodConValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newMethodCon = MapToEntity(request);

            await _methodConRepository.AddAsync(newMethodCon);

            return MapToDTO(newMethodCon);
        }

        
        public async Task<MethodConResponseDto> UpdateMethodConByIdAsync(Guid id, MethodConRequestDto newMethodCon)
        {
            var existMethodCon = await _methodConRepository.GetByIdAsync(id);
            if (existMethodCon == null)
                throw new ApiException.NotFoundException($"tác hại phương pháp học của ID {id} không tìm thấy .");

            if (newMethodCon == null)
                throw new ApiException.BadRequestException("tác hại phương pháp học mới không được để trống");

            var validationResult = await _methodConValidator.ValidateAsync(newMethodCon);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existMethodCon.Con = newMethodCon.Con;
            existMethodCon.MethodId = newMethodCon.MethodId;

            await _methodConRepository.UpdateAsync(existMethodCon);

            return MapToDTO(existMethodCon);
        }

        
        public async Task<MethodConResponseDto> DeleteMethodConByIdAsync(Guid id)
        {
            var existMethodCon = await _methodConRepository.GetByIdAsync(id);
            if (existMethodCon == null)
                throw new ApiException.NotFoundException($"tác hại phương pháp học của ID {id} không tìm thấy ");

            await _methodConRepository.DeleteAsync(existMethodCon);

            return MapToDTO(existMethodCon);
        }

        
        protected MethodCon MapToEntity(MethodConRequestDto request)
        {
            return new MethodCon
            {
                Con = request.Con,
                MethodId = request.MethodId
            };
        }

        public MethodConResponseDto MapToDTO(MethodCon method)
        {
            if (method == null) return null;

            return new MethodConResponseDto
            {
                Id = method.Id,
                Con = method.Con,
                MethodId = method.MethodId
            };
        }
    }
}
