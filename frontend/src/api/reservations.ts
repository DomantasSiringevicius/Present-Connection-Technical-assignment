import type {
  CreateReservationRequest,
  Reservation,
  ReservationFilters,
} from "../types/reservation";
import { apiRequest } from "./client";

export function getReservations(
  filters: ReservationFilters = {},
): Promise<Reservation[]> {
  const params = new URLSearchParams();

  if (filters.roomId !== undefined)
    params.append("roomId", String(filters.roomId));
  if (filters.organizerId !== undefined)
    params.append("organizerId", String(filters.organizerId));
  if (filters.status !== undefined)
    params.append("status", String(filters.status));
  if (filters.startDate !== undefined)
    params.append("startDate", String(filters.startDate));
  if (filters.endDate !== undefined)
    params.append("endDate", String(filters.endDate));

  const query = params.toString();
  return apiRequest<Reservation[]>(`/reservations${query ? `?${query}` : ""}`);
}

export function createReservation(
  request: CreateReservationRequest,
): Promise<Reservation> {
  return apiRequest<Reservation>("/reservations", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export function cancelReservation(id: string): Promise<Reservation> {
  return apiRequest<Reservation>(`/reservations/${id}/cancel`, {
    method: "PATCH",
  });
}

export function endReservationEarly(id: string): Promise<Reservation> {
  return apiRequest<Reservation>(`/reservations/${id}/end-early`, {
    method: "PATCH",
  });
}
