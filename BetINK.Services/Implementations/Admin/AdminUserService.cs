using BetINK.Services.Interfaces.Admin;
using BetINK.Services.Models.Admin;
using BetINK.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BetINK.Services.Implementations.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly BetINKDbContext db;

        public AdminUserService(BetINKDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<AdminUserListingServiceModel> All()
        => (from u in db.Users
            select new AdminUserListingServiceModel()
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.UserName,
                Roles = db.Roles
                        .Join(db.UserRoles.Where(a => a.UserId == u.Id)
                        , x => x.Id, y => y.RoleId, (p, x) => p.Name)
                        .ToList()
            }).ToList();
    }
}
