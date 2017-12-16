namespace BetINK.DataAccess.Models
{
    using BetINK.Common.Constants;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [Required]
        [MinLength(DataConstants.MinLength)]
        [MaxLength(DataConstants.MaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(DataConstants.MinLength)]
        [MaxLength(DataConstants.MaxLength)]
        public string LastName { get; set; }

        public List<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}
