using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Web.Models;
using OnlineCourse.Web.Services.Interfaces;

namespace OnlineCourse.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _identityService.SignIn(input);

            if (response.IsSuccessful) return RedirectToAction(nameof(Index), "Home");
            
            response.Errors.ForEach(x =>
            {
                ModelState.AddModelError(String.Empty, x);
            });

            return View();
        }
    }
}
