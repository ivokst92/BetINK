using BetINK.Services.Models.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetINK.Services.Interfaces.Admin
{
    public interface IAdminUserService
    {
        IEnumerable<AdminUserListingServiceModel> All();
    }
}
