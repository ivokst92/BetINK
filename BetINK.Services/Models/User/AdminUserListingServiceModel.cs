namespace BetINK.Services.Models.Admin
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class AdminUserListingServiceModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
