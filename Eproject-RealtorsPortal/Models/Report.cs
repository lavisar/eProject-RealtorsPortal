using System.ComponentModel.DataAnnotations;

namespace Eproject_RealtorsPortal.Models
{
    public class Report
    {    
        public decimal Payment { get; set; }
        public int CategoriesCount { get; set; }
        public int userCount { get; set; }
        public int listingCount { get; set; }
    }
}
