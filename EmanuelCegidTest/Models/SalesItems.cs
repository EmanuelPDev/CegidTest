using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmanuelCegidTest.Models
{
    public class SalesItems
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        [Range(0, 999999)]
        public int SalesUnit { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        [Required]
        [JsonIgnore]
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public DateTime? LastUpdate { get; set; }
    }
}
