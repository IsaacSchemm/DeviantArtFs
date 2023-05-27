using DeviantArtFs;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExampleWebApp.Models
{
    public class Token
    {
        public Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public Guid UserId { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
