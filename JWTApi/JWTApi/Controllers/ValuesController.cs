using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTApi.Controllers
{
    public class ValuesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("api/public")]
        public IActionResult GetPublic()
        {
            return Ok("YAY!!!");
        }

        [HttpGet]
        [Route("api/Private")]
        [Authorize]
        public IActionResult GetPrivate()
        {
            return Ok("WHHAAASSSUPPPP!!!!????!!!");
        }
    }
}
