namespace BetINK.Web.Areas.Admin.Controllers
{
    using BetINK.Common.Resources;
    using BetINK.DataAccess.Models;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Web.Areas.Admin.Models.Users;
    using BetINK.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : BaseAdminController
    {
        private readonly IAdminUserService adminUserService;

        private readonly RoleManager<IdentityRole> roleManager;


        private readonly UserManager<User> userManager;

        public UsersController(IAdminUserService adminUserService,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.adminUserService = adminUserService;
        }

        public IActionResult Index()
        {
            var users = this.adminUserService.All();
            var roles = this.roleManager
                .Roles
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToList();

            return View(new AdminUserListingsViewModel
            {
                Roles = roles,
                Users = users
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(UserRoleViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);

            if (!await this.IdentityDetailsValid(user, model.Role))
            {
                ModelState.AddModelError(string.Empty, MessageResources.msgInvalidIdentityDetails);
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool IsUserInRole = await this.userManager.IsInRoleAsync(user, model.Role);

            if (IsUserInRole)
            {
                TempData.AddErrorMessage(string.Format(MessageResources.msgUserAlreadyInRole, user.UserName, model.Role));
                return RedirectToAction(nameof(Index));
            }
            
            await this.userManager.AddToRoleAsync(user, model.Role);

            TempData.AddSuccessMessage(string.Format(MessageResources.msgSuccessAddedToRole, user.UserName, model.Role));
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(UserRoleViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);

            if (!await this.IdentityDetailsValid(user, model.Role))
            {
                ModelState.AddModelError(string.Empty, MessageResources.msgInvalidIdentityDetails);
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool IsUserInRole = await this.userManager.IsInRoleAsync(user, model.Role);

            if (!IsUserInRole)
            {
                TempData.AddErrorMessage(string.Format(MessageResources.msgUserNotInRole, user.UserName, model.Role));
                return RedirectToAction(nameof(Index));
            }

            await this.userManager.RemoveFromRoleAsync(user, model.Role);

            TempData.AddSuccessMessage(string.Format(MessageResources.msgSuccessRemovedFromRole, user.UserName, model.Role));
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IdentityDetailsValid(User user, string role)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(role);
            var userExists = user != null;
            return roleExists && userExists;
        }

    }
}