using BusinessLayer.Interfaces;
using DataTypes.Requests;
using Microsoft.AspNetCore.Mvc;

namespace RESTWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserRegisterReq reqData)
        {
            var ipAddress = "";
            return Ok(await _userService.RegisterAsync(reqData, ipAddress));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] UserRegisterReq reqData)
        {
            return Ok(await _userService.UpdateAsync(reqData, id));
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
