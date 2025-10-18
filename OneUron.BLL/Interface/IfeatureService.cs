using OneUron.BLL.DTOs.FeatureDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IfeatureService
    {
        public Task<List<FeatureResponseDto>> GetAllAsync();

        public Task<FeatureResponseDto> GetByIdAsync(Guid id);

        public  Task<FeatureResponseDto> CreateFeatureAsync(FeatureRequestDto requestDto);

        public  Task<FeatureResponseDto> UpdateFeatureByIdAsync(Guid id, FeatureRequestDto requestDto);

        public  Task<FeatureResponseDto> DeleteFuatureByIdAsync(Guid id);

        public FeatureResponseDto MapToDTO(Features feature);

    }
}
