using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs;
using OneUron.BLL.DTOs.AuthDTOs;
using OneUron.BLL.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using OneUron.BLL.DTOs.ProfileDTOs;
using OneUron.BLL.Services;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/profile - Lấy profile của người dùng đăng nhập
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            // Lấy ID người dùng từ token JWT
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(profile);
        }

        // GET: api/profile/{id} - Lấy profile theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileById(Guid id)
        {
            // Lấy ID người dùng từ token JWT
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isAdmin = User.IsInRole("Admin");

            var profile = await _profileService.GetProfileByIdAsync(id);
            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            // Kiểm tra quyền: Chỉ cho phép chủ profile hoặc admin xem
            if (!isAdmin && profile.UserId != userId)
                return Forbid();

            return Ok(profile);
        }

        // POST: api/profile - Tạo mới profile
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDto profileDto)
        {
            // Lấy ID người dùng từ token JWT
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var createdProfile = await _profileService.CreateProfileAsync(profileDto, userId);
                return CreatedAtAction(nameof(GetProfileById), new { id = createdProfile.Id }, createdProfile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/profile/{id} - Cập nhật profile
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateProfileDto profileDto)
        {
            // Lấy ID người dùng từ token JWT
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isAdmin = User.IsInRole("Admin");

            var profile = await _profileService.GetProfileByIdAsync(id);
            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            // Chỉ cho phép chủ profile hoặc admin cập nhật
            if (!isAdmin && profile.UserId != userId)
                return Forbid();

            try
            {
                var updatedProfile = await _profileService.UpdateProfileAsync(id, profileDto);
                return Ok(updatedProfile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
