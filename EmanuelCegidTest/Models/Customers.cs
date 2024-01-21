using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmanuelCegidTest.Models
{
    public class Customers
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? Country { get; set; }
        [Required]
        [StringLength(20)]
        public string? TaxID { get; set; }
        [Required]
        [JsonIgnore]
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public DateTime? LastUpdate { get; set; }
    }
}
