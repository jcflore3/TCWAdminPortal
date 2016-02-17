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

        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }
        
        [Required]
        [Display(Name = "MLS Listing URL")]
        [Url]
        public string MLSListingURL { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [Display(Name = "Street Address")]
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

        [Display(Name = "Bed Rooms")]
        public int BedRooms { get; set; }

        [Display(Name = "Full Baths")]
        public int FullBaths { get; set; }

        [Display(Name = "Half Baths")]
        public int HalfBaths { get; set; }

        public int Garage { get; set; }

        [Display(Name = "Building Sq Feet")]
        public int BuildingSQFeet { get; set; }

        [Display(Name = "Lot Sq Feet")]

        public int LotSQFeet { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public Boolean Enabled { get; set; }
    }
}
