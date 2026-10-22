using Domain.Enums;

namespace Application.Dtos;

public class RoomFilter
{
    public int? MinCapacity { get; set; }
    
    public int? Floor { get; set; }
    
    public List<Equipment>? Equipment { get; set; }
}