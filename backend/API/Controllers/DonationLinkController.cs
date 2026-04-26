using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationLinkController : ControllerBase
    {
        private readonly IDonationLinkService _donationLinkService;

        public DonationLinkController(IDonationLinkService donationLinkService)
        {
            _donationLinkService = donationLinkService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationLinkDTO>>> GetByCreatorId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Invalid User Id in token.");
            }

            var result = await _donationLinkService.GetByCreatorIdAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DonationLinkDTO>> Create([FromBody] string message)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Invalid User Id in token.");
            }

            var result = await _donationLinkService.CreateAsync(new CreateDonationLinkDTO
            {
                CreatorId = userId,
                Message = message
            });
            return Ok(result);
        }
    }
}
