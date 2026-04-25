using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateUserDTO
    {
        public string GoogleId { get; set; } = null!;
        public string GoogleToken { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateUserDTO
    {
        public string GoogleToken { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
    }
}
