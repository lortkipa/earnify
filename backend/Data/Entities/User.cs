using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string GoogleId { get; set; } = null!;
        [Required]
        public string GoogleToken { get; set; } = null!;
        public string? PaypalClientId { get; set; }
        public string? PaypalClientSecret { get; set; }
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string AvatarUrl { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set; }

        public ICollection<DonationLink> DonationLinks { get; set; } = new List<DonationLink>();
    }
}
