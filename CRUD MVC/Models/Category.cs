using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUD_MVC.Models
{
    public class Category
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [DisplayName("Name Description")]
        public string? Description { get; set; }

        public string? FileUpload { get; set; }

    }
}
