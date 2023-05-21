﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CRUDTasknWeave.Models.DTOs
{
    public class UserRegisteration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
