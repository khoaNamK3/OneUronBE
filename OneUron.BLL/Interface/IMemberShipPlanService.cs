using OneUron.BLL.DTOs.MemberShipPlanDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IMemberShipPlanService
    {
        public Task<List<MemberShipPlanResponseDto>> GetAllMembertShipPlanAsync();

        public Task<MemberShipPlanResponseDto> GetMemberShipPlanByIdAsync(Guid id);

        public  Task<MemberShipPlanResponseDto> CreateMemberShipPlanAsync(MemberShipPlanRequestDto requestDto);

        public  Task<MemberShipPlanResponseDto> UpdateMemberShipPlanByIdAsync(Guid id, MemberShipPlanRequestDto request);

        public  Task<MemberShipPlanResponseDto> DeleteMemberShipPlanByIdAsync(Guid id);

        public MemberShipPlanResponseDto MapToDTO(MemberShipPlan memberShipPlan);

    }
}
