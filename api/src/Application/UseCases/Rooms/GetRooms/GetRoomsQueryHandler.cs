using Application.Dtos;
using Application.Mappers;
using Application.UseCases.Rooms.GetRooms;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Rooms.GetRooms;

public class GetRoomsQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetRoomsQuery, List<RoomDto>>
{
    public async Task<List<RoomDto>> Handle(GetRoomsQuery query, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetRoomsAsync(query.MinCapacity, query.Floor, query.Equipment);
        
        return rooms.Select(rm => rm.ToDto()).ToList();
    }
}