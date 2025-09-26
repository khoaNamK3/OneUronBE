using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.TechniqueRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class TechniqueService : ITechniqueService
    {
        private readonly ITechniqueRepository _techniqueRepository;

        public TechniqueService(ITechniqueRepository techniqueRepository)
        {
            _techniqueRepository = techniqueRepository;
        }

        public async Task<ApiResponse<List<TechniqueResponseDto>>> GetAllAsync()
        {
            try
            {
                var techniques = await _techniqueRepository.GetAllAsync();

                if (!techniques.Any())
                {
                    return ApiResponse<List<TechniqueResponseDto>>.FailResponse("Get All Techinique Fail", "Techniques Are Empty");
                }

                var result = techniques.Select(MapToDTO).ToList();

                return ApiResponse<List<TechniqueResponseDto>>.SuccessResponse(result, "Get All Technique Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TechniqueResponseDto>>.FailResponse("Get All Techinique Fail", ex.Message);

            }
        }

        public async Task<ApiResponse<TechniqueResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existTechnique = await _techniqueRepository.GetByIdAsync(id);

                if (existTechnique == null)
                {
                    return ApiResponse<TechniqueResponseDto>.FailResponse("Get Technique By Id Fail", "Technique Is Not Exist");
                }

                var result = MapToDTO(existTechnique);

                return ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Get Technique By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TechniqueResponseDto>.FailResponse("Get Technique By Id Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<TechniqueResponseDto>> CreateNewTechiqueAsync(TechniqueRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<TechniqueResponseDto>.FailResponse("Create New Technique Fail", "New Technique Is Null");
                }

                var newTechnique = MapToEntity(request);

                await _techniqueRepository.AddAsync(newTechnique);

                var result = MapToDTO(newTechnique);

                return ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Get All Technique Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<TechniqueResponseDto>.FailResponse("Create New Technique Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<TechniqueResponseDto>> UpdateTechniqueByIdAsync(Guid id, TechniqueRequestDto newTechnique)
        {
            try
            {
                var existTechnique = await _techniqueRepository.GetByIdAsync(id);

                if (existTechnique == null)
                {
                    return ApiResponse<TechniqueResponseDto>.FailResponse("Update Technique By Id Fail", "Technique Is Not Exist");
                }

                if (newTechnique == null)
                {
                    return ApiResponse<TechniqueResponseDto>.FailResponse("Update Technique By Id Fail", "New Technique is Null");
                }
                existTechnique.Name = newTechnique.Name;
                existTechnique.MethodId = newTechnique.MethodId;

                await _techniqueRepository.UpdateAsync(existTechnique);

                var result = MapToDTO(existTechnique);

                return ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Update Technique By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TechniqueResponseDto>.FailResponse("Update Technique By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<TechniqueResponseDto>> DeleteTechniqueByidAsync(Guid id)
        {
            try
            {
                var existTechnique = await _techniqueRepository.GetByIdAsync(id);

                if (existTechnique == null)
                {
                    return ApiResponse<TechniqueResponseDto>.FailResponse("Delete Technique By Id Fail", "Technique Is Not Exist");
                }
                
                 var result = MapToDTO(existTechnique);

                await _techniqueRepository.DeleteAsync(existTechnique);

                return ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Delete Technique By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<TechniqueResponseDto>.FailResponse("Delete Technique By Id Fail", ex.Message);
            }
        }


        protected Technique MapToEntity(TechniqueRequestDto request)
        {
            return new Technique
            {
                Name = request.Name,
                MethodId = request.MethodId,
            };
        }

        public TechniqueResponseDto MapToDTO(Technique technique)
        {
            return new TechniqueResponseDto
            {
                Id = technique.Id,
                Name = technique.Name,
                MethodId = technique.MethodId,
            };
        }
    }
}
