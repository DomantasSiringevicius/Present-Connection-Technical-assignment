using Application.Dtos.ReservationDtos;
using Application.Mappers;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Reservations.GetReservations;

public class GetReservationsQueryHandler(IReservationRepository reservationRepository) : IRequestHandler<GetReservationsQuery, List<ReservationDto>>
{
    public async Task<List<ReservationDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await reservationRepository.GetReservationsAsync(
            request.RoomId,
            request.OrganizerId,
            request.Status,
            request.StartDate,
            request.EndDate
        );          
        
        return reservations.Select(r => r.ToDto()).ToList();
    }
}