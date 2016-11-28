namespace OdeToFood.Controllers
{
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody]CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User()
            {
                UserName = model.Username
            };

            var createdUser = await _userManager.CreateAsync(user, model.Password);

            if (createdUser.Succeeded)
            {
                //login
                //await _signInManager.SignInAsync(user, false);
                return Ok();
            }
            else
            {
                foreach (var error in createdUser.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return BadRequest(ModelState);
            }


        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LogIn([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (signInResult.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError("", "Counld not login.");
                return BadRequest(ModelState);
            }
        }
    }
}
