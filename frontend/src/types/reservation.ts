export const RESERVATION_STATUS = ["Booked", "Completed", "Cancelled"] as const;

export type ReservationStatus = (typeof RESERVATION_STATUS)[number];

export interface Reservation {
  id: string;
  organizerName: string;
  organizerLastName: string;
  roomName: string;
  roomId: string;
  organizerId: string;
  meetingTitle: string;
  startDate: string;
  endDate: string;
  actualEndDate: string | null;
  numberOfAttendees: number;
  reservationStatus: ReservationStatus;
}

export interface ReservationFilters {
  roomId?: string;
  organizerId?: string;
  status?: ReservationStatus;
  startDate?: string;
  endDate?: string;
}

export interface CreateReservationRequest {
  roomId: string;
  organizerId: string;
  meetingTitle: string;
  startDate: string;
  endDate: string;
  numberOfAttendees: number;
}
