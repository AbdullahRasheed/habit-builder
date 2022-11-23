using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using net_api.Controllers.Context;
using net_api.Data;
using net_api.Data.Requests;
using net_api.Database;

namespace net_api.Controllers
{
    [ApiController]
    [Route("habit")]
    public class HabitController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly HttpUserService _userContext;

        public HabitController(UserService userService, HttpUserService userContext)
        {
            _userService = userService;
            _userContext = userContext;
        }

        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult> HabitInfo()
        {
            ObjectId? id = _userContext.GetObjectId();
            if(id is not null)
            {
                User? user = await _userService.FindAsync(id.Value);
                if(user is not null)
                {
                    return Ok(user.Habits);
                }

                return Unauthorized("ID invalid");
            }

            return Unauthorized("Credentials invalid");
        }

        [Authorize]
        [HttpPost("createhabit")]
        public async Task<ActionResult> CreateHabit([FromBody] HabitCreateRequest request)
        {
            ObjectId? id = _userContext.GetObjectId();
            if(id is not null)
            {
                UpdateResult result = await _userService.PushHabitAsync(id.Value, new()
                {
                    Name = request.Name,
                    Description = request.Description
                });

                if (result.MatchedCount == 0) return Unauthorized("ID invalid");

                if (result.IsAcknowledged)
                {
                    return Ok();
                }

                return Conflict("Could not add the habit");
            }

            return Unauthorized("Credentials invalid");
        }
    }
}
