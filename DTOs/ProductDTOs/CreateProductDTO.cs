using System.ComponentModel.DataAnnotations;

namespace SkillStackCSharp.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }

}
