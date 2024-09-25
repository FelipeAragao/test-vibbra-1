using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Services;

namespace src.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IInviteService _inviteService;

        public UsersController(IUserService userService, IInviteService inviteService)
        {
            this._userService = userService;
            this._inviteService = inviteService;
        }
        
        [Authorize("JwtOrGoogle")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get([FromRoute] int userId)
        {
            try {
                var user = await this._userService.Get(userId);
                return Ok(user);
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest(new { error = "Invalid user data" });
            }

            try {
                await this._userService.Add(userDTO);
                return Created("", userDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] int userId, [FromBody] UserDTO userDTO)
        {
            try {
                userDTO.UserId = userId;
                return Ok(await this._userService.Update(userDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpGet("{userId}/invites/{inviteId}")]
        public async Task<IActionResult> GetMessage([FromRoute] int userId, [FromRoute] int inviteId)
        {
            try {
                return Ok(await this._inviteService.Get(userId, inviteId));

            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpGet("{userId}/invites")]
        public async Task<IActionResult> GetInvitesByUser([FromRoute] int userId)
        {
            try {
                return Ok(await this._inviteService.GetAllByUser(userId));

            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPost("{userId}/invites")]
        public async Task<IActionResult> AddInvite([FromRoute] int userId, [FromBody] InviteDTO inviteDTO)
        {
            try {
                inviteDTO.UserId = userId;
                return Created("", await this._inviteService.Add(inviteDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPut("{userId}/invites/{inviteId}")]
        public async Task<IActionResult> UpdateInvite([FromRoute] int userId, [FromRoute] int inviteId, [FromBody] InviteDTO inviteDTO)
        {
            try {
                inviteDTO.UserId = userId;
                inviteDTO.InviteId = inviteId;
                return Ok(await this._inviteService.Update(inviteDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}