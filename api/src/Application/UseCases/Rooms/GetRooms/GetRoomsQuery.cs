using Application.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Rooms.GetRooms;

public class GetRoomsQuery : IRequest<List<RoomDto>>
{
    public int? MinCapacity { get; set; }
    
    public int? Floor { get; set; }
    
    public List<Equipment>? Equipment { get; set; }
}