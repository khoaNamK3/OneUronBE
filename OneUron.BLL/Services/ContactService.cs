using FluentValidation;
using OneUron.BLL.DTOs.ContactDTO;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.contactRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class ContactService : IContactService
    {

        private readonly IContactRepository _contactRepository;
        private readonly IValidator<ContactRequestDto> _contactValidator;
        public ContactService(IContactRepository contactRepository, IValidator<ContactRequestDto> contactValidator)
        {
            _contactRepository = contactRepository;
            _contactValidator = contactValidator;
        }

        public async Task<List<ContactResponseDto>> GetAllContactAsync()
        {
            var contactlist = await _contactRepository.GetAllContactAsync();

            if (!contactlist.Any())
                throw new ApiException.NotFoundException("No Contact Found");

            var result = contactlist.Select(MapToDTO).ToList();
            return result;
        }

        public async Task<ContactResponseDto> GetByIdAsync(Guid id)
        {
            var contact =  await _contactRepository.GetByIdAsync(id);

            if (contact == null)
                throw new ApiException.NotFoundException("contact does not exist");

            var result  = MapToDTO(contact);
            return result;
        }

        public async Task<ContactResponseDto> CreateNewContactAsync(ContactRequestDto requestDto)
        {
           var validationResult = await _contactValidator.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
            {
                throw new ApiException.ValidationException(validationResult.Errors);
            }

            var newContact = MapToEntity(requestDto);

            await _contactRepository.AddAsync(newContact);

            var result = MapToDTO(newContact);
            return result;
        }

        public async Task<ContactResponseDto> DeleteContactByIdAsync(Guid id)
        {
            var existContact = await _contactRepository.GetByIdAsync(id);

            if (existContact == null)
                throw new ApiException.NotFoundException("contact does not exist");

            var result = MapToDTO(existContact);

            await _contactRepository.DeleteAsync(existContact);

            return result;
        }


        public Contact MapToEntity(ContactRequestDto requestDto)
        {
            return new Contact {
                Email = requestDto.Email,
                Message = requestDto.Message,
                createAt = DateTime.UtcNow,
                Phone = requestDto.Phone,
            };
            
        }

        public ContactResponseDto MapToDTO(Contact contact)
        {
            return new ContactResponseDto
            {
                Email = contact.Email,
                Id = contact.Id,
                Phone = contact.Phone,
                CreateAt = contact.createAt,
                Message = contact.Message,
            };
        }
    }
}
