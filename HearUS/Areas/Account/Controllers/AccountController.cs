using FluentValidation;
using HearUS.Areas.Account.ViewModels;
using HearUS.Application.Features.Accounts.Commands.Login;
using HearUS.Application.Features.Accounts.Commands.Logout;
using HearUS.Application.Features.Accounts.Commands.Register;
using HearUS.Application.Features.Accounts.Queries.GetProfile;
using HearUS.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly ISender _mediator;

        public AccountController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _mediator.Send(
                    new LoginCommand(model.Email, model.Password, model.RememberMe),
                    cancellationToken);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                ModelState.AddModelError(
                    string.Empty,
                    result.IsLockedOut
                        ? "تم قفل الحساب مؤقتاً بسبب محاولات دخول خاطئة متكررة. حاول لاحقاً."
                        : "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
            }

            return View(model);
        }

        [HttpGet("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(new RegisterViewModel());
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _mediator.Send(
                    new RegisterCommand(model.FullName, model.Email, model.City, model.Password),
                    cancellationToken);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, TranslateIdentityError(error.Code, error.Description));
                    }

                    return View(model);
                }
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(model);
            }

            TempData["Success"] = "تم إنشاء الحساب بنجاح، أهلاً بك في صوتنا.";
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost("Logout")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await _mediator.Send(new LogoutCommand(), cancellationToken);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet("Profile")]
        [Authorize]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            var profile = await _mediator.Send(new GetProfileQuery(), cancellationToken);

            return View(new ProfileViewModel
            {
                FullName = profile.FullName,
                Email = profile.Email,
                City = profile.City,
                Bio = profile.Bio,
                CreatedAt = profile.CreatedAt,
                Roles = profile.Roles.ToList(),
                PostsCount = profile.PostsCount,
                InsightsCount = profile.InsightsCount
            });
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private static string TranslateIdentityError(string code, string description)
        {
            return code switch
            {
                "DuplicateEmail" or "DuplicateUserName" => "هذا البريد الإلكتروني مسجّل بالفعل.",
                "PasswordMismatch" => "كلمتا المرور غير متطابقتين.",
                "PasswordTooShort" => "كلمة المرور قصيرة جداً.",
                "PasswordRequiresDigit" => "يجب أن تحتوي كلمة المرور على رقم واحد على الأقل.",
                "PasswordRequiresLower" => "يجب أن تحتوي كلمة المرور على حرف صغير واحد على الأقل.",
                "PasswordRequiresUpper" => "يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل.",
                "PasswordRequiresNonAlphanumeric" => "يجب أن تحتوي كلمة المرور على رمز خاص واحد على الأقل.",
                "InvalidUserName" or "InvalidEmail" => "البريد الإلكتروني غير صالح.",
                _ => description
            };
        }
    }
}
