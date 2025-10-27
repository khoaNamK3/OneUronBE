using OneUron.BLL.DTOs.ProfileDTOs;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ProfileRepository;
using OneUron.DAL.Repository.UserRepo;
using System;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileByUserIdAsync(Guid userId);
        Task<ProfileDto> GetProfileByIdAsync(Guid profileId);
        Task<ProfileDto> CreateProfileAsync(CreateProfileDto dto, Guid userId);
        Task<ProfileDto> UpdateProfileAsync(Guid profileId, UpdateProfileDto dto);
        Task<bool> DeleteProfileAsync(Guid profileId);
        Task<bool> IsProfileOwnerAsync(Guid profileId, Guid userId);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;

        public ProfileService(IProfileRepository profileRepository, IUserRepository userRepository)
        {
            _profileRepository = profileRepository;
            _userRepository = userRepository;
        }

        public async Task<ProfileDto> GetProfileByUserIdAsync(Guid userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
                return null;

            return MapToProfileDto(profile);
        }

        public async Task<ProfileDto> GetProfileByIdAsync(Guid profileId)
        {
            var profile = await _profileRepository.GetByIdAsync(profileId);
            if (profile == null)
                return null;

            return MapToProfileDto(profile);
        }

        public async Task<ProfileDto> CreateProfileAsync(CreateProfileDto dto, Guid userId)
        {
            // Check if user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("user No exist  ");

            // Check if user already has a profile
            var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
            if (existingProfile != null)
                throw new InvalidOperationException("user earldy have profile");

            // Create new profile
            var profile = new Profile
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Address = dto.Address,
                Avatar = dto.Avatar,
                Dob = dto.Dob,
                UserId = userId
            };

            await _profileRepository.AddAsync(profile);
            return MapToProfileDto(profile);
        }

        public async Task<ProfileDto> UpdateProfileAsync(Guid profileId, UpdateProfileDto dto)
        {
            var profile = await _profileRepository.GetByIdAsync(profileId);
            if (profile == null)
                throw new ArgumentException("cv No exist");

            // Update profile fields
            profile.FullName = dto.FullName;
            profile.Address = dto.Address;
            profile.Avatar = dto.Avatar;
            profile.Dob = dto.Dob;

            await _profileRepository.UpdateAsync(profile);
            return MapToProfileDto(profile);
        }

        public async Task<bool> DeleteProfileAsync(Guid profileId)
        {
            var profile = await _profileRepository.GetByIdAsync(profileId);
            if (profile == null)
                return false;

            await _profileRepository.DeleteAsync(profile);
            return true;
        }

        public async Task<bool> IsProfileOwnerAsync(Guid profileId, Guid userId)
        {
            var profile = await _profileRepository.GetByIdAsync(profileId);
            return profile != null && profile.UserId == userId;
        }

        private ProfileDto MapToProfileDto(Profile profile)
        {
            return new ProfileDto
            {
                Id = profile.Id,
                FullName = profile.FullName,
                Address = profile.Address,
                Avatar = profile.Avatar,
                Dob = profile.Dob,
                UserId = profile.UserId
            };
        }
    }
}