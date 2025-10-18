using OneUron.BLL.DTOs.MemberShipDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IMemberShipService
    {
        public Task<List<MemberShipResponseDto>> GetAllAsync();

        public Task<MemberShipResponseDto> GetByIdAsync(Guid id);

        public Task<MemberShipResponseDto> CreateMemberShipAsync(MemberShipRequestDto requestDto);


        public Task<MemberShipResponseDto> UpdateMemberShipByIdAsync(Guid id, MemberShipRequestDto requestDto);

        public Task<MemberShipResponseDto> DeleteMemberShipByIdAsync(Guid id);

        public MemberShipResponseDto MapToDTO(MemberShip memberShip);
    }
}
