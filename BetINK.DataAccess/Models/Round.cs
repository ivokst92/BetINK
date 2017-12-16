namespace BetINK.DataAccess.Models
{
    using BetINK.Common.Constants;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Round
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Range(1, 100)]
        public int Number { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [MaxLength(DataConstants.MaxLength)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int SeasonId { get; set; }

        public Season Season { get; set; }

        public List<Match> Matches { get; set; } = new List<Match>();
    }
}
