namespace BetINK.DataAccess.Models
{
    using BetINK.Common.Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Season
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string Name { get; set; }

        public List<Round> Rounds { get; set; } = new List<Round>();
    }
}
