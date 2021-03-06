using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace API.Dto.Store;

public class CustomerBasketDto
{
    [Required]
    public string? Id { get; set; }
    [Required]
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
}