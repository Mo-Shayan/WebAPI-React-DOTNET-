using System.ComponentModel;

namespace WebAPIconsume.Models
{
    public class BrandViewModel
    {

        public int ID { get; set; }
        [DisplayName("Brand Name")]
        public string Name { get; set; }
        public string Category { get; set; }
        public int IsActive { get; set; }
    }
}
