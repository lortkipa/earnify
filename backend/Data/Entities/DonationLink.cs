using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    [Table("DonationLinks")]
    public class DonationLink
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CreatorId { get; set; }
        [Required]
        [MaxLength(400)]
        public string Message { get; set; } = null!;

        public User? User { get; set; }
    }
}