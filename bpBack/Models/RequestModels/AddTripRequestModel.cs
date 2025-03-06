using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinProjektBackend.Models.RequestModels;

public class AddTripRequestModel
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }    

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Country { get; set; }
    
    [Required]
    public string Description { get; set; }

    [Required]
    public string ImageLink { get; set; }
    [Required] 
    public decimal Price { get; set; }
}