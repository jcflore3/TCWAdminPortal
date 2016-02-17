using System;
using System.ComponentModel.DataAnnotations;

namespace TCWAdminPortalWeb.ViewModels
{
    public class ContactInfoViewModel
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string StreetAddress { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string City { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string State { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Zip { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
