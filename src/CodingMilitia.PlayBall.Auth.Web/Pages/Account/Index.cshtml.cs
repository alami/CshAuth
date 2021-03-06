using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.Auth.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Utilities;

namespace CodingMilitia.PlayBall.Auth.Web.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<PlayBallUser> _userManager;
        private readonly SignInManager<PlayBallUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<PlayBallUser> userManager,
            SignInManager<PlayBallUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Username { get; set; }
        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email
            };
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email  = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Email);
                var setEmailResult = setUserNameResult.Succeeded
                    ? (await _userManager.SetEmailAsync(user, Input.Email))
                    : IdentityResult.Failed();
                if (!setUserNameResult.Succeeded || !setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                $"Unexpected error occurred setting email for user with ID `userId`.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmail ()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage();
        }
    }
}