using System;
using System.ComponentModel.DataAnnotations;

namespace TCWAdminPortalWeb.ViewModels
{
    public class FeaturedPropertyViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Title { get; set; }
        public string ImageURL { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string StreetAddress { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string City { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string State { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Zip { get; set; }
        public int BedRooms { get; set; }
        public int FullBaths { get; set; }
        public int HalfBaths { get; set; }
        public int Garage { get; set; }
        public int BuildingSQFeet { get; set; }
        public int LotSQFeet { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public Boolean Enabled { get; set; }
    }
}
