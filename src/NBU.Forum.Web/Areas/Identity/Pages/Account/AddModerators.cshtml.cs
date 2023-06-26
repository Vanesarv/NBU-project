using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NBU.Forum.Contracts.Responses;
using NBU.Forum.Domain.AppUserAggregate;

namespace NBU.Forum.Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class AddModeratorsModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public AddModeratorsModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<GetUserResponse> Users { get; set; }

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            this.Users = users
                .Where(x => !_userManager.IsInRoleAsync(x, "Moderator").GetAwaiter().GetResult())
                .Select(x => new GetUserResponse()
                {
                    UserId = x.Id,
                    Username = x.UserName!,
                    Email = x.Email!
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.AddToRoleAsync(user, "Moderator");
            if (result.Succeeded)
            {
                return RedirectToPage("./AddModerators");
            }

            return BadRequest(result);
        }
    }
}
