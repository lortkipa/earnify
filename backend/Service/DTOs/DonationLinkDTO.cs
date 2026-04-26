using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.DTOs
{
    public class DonationLinkDTO
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Message { get; set; } = null!;
    }

    public class CreateDonationLinkDTO
    {
        public int CreatorId { get; set; }
        public string Message { get; set; } = null!;
    }

    public class UpdateDonationLinkDTO
    {
        public int CreatorId { get; set; }
        public string Message { get; set; } = null!;
    }
}
