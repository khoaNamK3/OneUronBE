using OneUron.BLL.DTOs.AdminDTOs;
using OneUron.BLL.Interface;
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

            var failedPayment = await _paymentRepository.GetAllPaymentFaidAsync();
            var totalfaidPayment = failedPayment.Count;

            var memberShip = await _memberShipRepository.GetAllAsync();
            var totalMemberShip = memberShip.Count();


            var result = new AdminResponseDto
            {
                UserCount = totalUsers,
                PaymentSuccess = totalSucessPayment,
                PaymentFail = totalfaidPayment,
                MemberShip = totalMemberShip,
            };
            return result;
        }
    }
}
