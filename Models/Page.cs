using System.ComponentModel.DataAnnotations;

namespace LiquefiedPetroleumGas.Models
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Content { get; set; }
    }
}
