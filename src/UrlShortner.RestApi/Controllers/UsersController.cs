using Microsoft.AspNetCore.Mvc;
using UrlShortner.Application.Interfaces;
using UrlShortner.RestApi.Mappers;
using UrlShortner.RestApi.Models.Users;

namespace UrlShortner.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserCreateRequest userCreateReq)
        {
            var ipAddress = "";
            var requestDto = userCreateReq.ToDto(ipAddress);
            var createdUser = await _userService.CreateAsync(requestDto);

            return Created($"api/users/{createdUser}", createdUser.ToResponse());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] UserUpdateRequest reqData)
        {
            var ipAddress = "";
            var userUpdateDto = reqData.ToDto(ipAddress);
            return Ok(await _userService.UpdateAsync(userUpdateDto, id));
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
