using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Models
{
    public class Token : IDeviantArtRefreshToken
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
