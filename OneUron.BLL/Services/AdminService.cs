using OneUron.BLL.DTOs.AdminDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using OneUron.DAL.Repository.MemberShipRepo;
using OneUron.DAL.Repository.PaymentRepo;
using OneUron.DAL.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        private readonly IPaymentRepository _paymentRepository;

        private readonly IMemberShipRepository _memberShipRepository;
        public AdminService(IUserRepository userRepository, IPaymentRepository paymentRepository, IMemberShipRepository memberShipRepository)
        {

            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _memberShipRepository = memberShipRepository;
        }

        public async Task<AdminResponseDto> GetAdminInforAsync()
        {
            var users = await _userRepository.GetAllUserAsync();

            var totalUsers = users.Count();

            var successPayment = await _paymentRepository.GetAllPaymentSucessfullyAsync();
            var totalSucessPayment = successPayment.Count();
            var totalAmount = successPayment.Select(x => x.Amount).Sum();

            var failedPayment = await _paymentRepository.GetAllPaymentFaidAsync();
            var totalfaidPayment = failedPayment.Count();

            var memberShip = await _memberShipRepository.GetAllAsync();
            var totalMemberShip = memberShip.Count();


            var result = new AdminResponseDto
            {
                UserCount = totalUsers,
                TotalAmount = totalAmount,
                PaymentFail = totalfaidPayment,
                MemberShip = totalMemberShip,
            };
            return result;
        }

        public async Task<PagedResult<UserPagingResponseDto>> GetUserPagingAsync(int pageNumber, int pageSize, string userName)
        {
            var userPaging = await _userRepository.GetUserPagingAsync(pageNumber, pageSize, userName);

            if(!userPaging.Items.Any())
                throw new ApiException.NotFoundException("Không tìm thấy người dùng");

            var result = userPaging.Items.Select(MapToDTO).ToList();

            return new PagedResult<UserPagingResponseDto>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = result.Count,
                Items = result
            };
        }

        public UserPagingResponseDto MapToDTO(User user)
        {
            return new UserPagingResponseDto
            {
                UserName = user.UserName,
                CreatedDate = user.CreatedDate,
                UpdateDate = user.UpdateDate,
                IsDeleted = user.IsDeleted,
            };
        }
    }
}
