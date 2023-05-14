using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyAppWeb.Models
{
    public class Category
    {
        [Key] 
        public int id  { get; set; }

        [Required] 
        public string Name { get; set; }
        [DisplayName ("Display Order")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;


    }
}
