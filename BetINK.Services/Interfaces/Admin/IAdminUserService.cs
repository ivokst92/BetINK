namespace BetINK.Services.Interfaces.Admin
{
    using BetINK.Services.Models.Admin;
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    public interface IAdminUserService
    {
        IEnumerable<AdminUserListingServiceModel> All();
    }
}
