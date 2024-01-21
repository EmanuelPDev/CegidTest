using EmanuelCegidTest.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmanuelCegidTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _SignInManager;

        public AuthenticationController(UserManager<IdentityUser> usermaneger, SignInManager<IdentityUser> SignInManager)
        {
            _userManager = usermaneger;
            _SignInManager = SignInManager;
        }

        //[HttpGet]
        //public ActionResult<string> Get()
        //{
        //    return $"AuthenticationController :: Access o : {DateTime.Now.ToLongDateString()}";
        //}

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserDTO utilizador)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(errors => errors.Errors));

            var user = utilizador.DtoToEntity(utilizador);

            var result = await _userManager.CreateAsync(user, utilizador.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _SignInManager.SignInAsync(user, false);
            return Ok(/*GeraToken(utilizador)*/);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserDTO utilizador)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(errors => errors.Errors));

            var result = await _SignInManager.PasswordSignInAsync(utilizador.Email, utilizador.Password, false, false);

            if (result.Succeeded)
                return Ok();
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login.");
                return BadRequest(ModelState);
            }
        }
    }
}
