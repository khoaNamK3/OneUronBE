using OneUron.BLL.DTOs.ContactDTO;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IContactService
    {
        public  Task<List<ContactResponseDto>> GetAllContactAsync();
        public  Task<ContactResponseDto> GetByIdAsync(Guid id);
        public  Task<ContactResponseDto> CreateNewContactAsync(ContactRequestDto requestDto);
        public  Task<ContactResponseDto> DeleteContactByIdAsync(Guid id);

    }
}
