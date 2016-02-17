using System;
using System.ComponentModel.DataAnnotations;

namespace TCWAdminPortalWeb.ViewModels
{
    public class AgentViewModel
    {
        public int ID { get; set; }
        
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public Boolean Enabled { get; set; }
    }
}
