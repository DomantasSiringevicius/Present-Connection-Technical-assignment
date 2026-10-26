import type { Room, RoomFilters } from "../types/room";
import { apiRequest } from "./client";

export function getRooms(filters: RoomFilters = {}): Promise<Room[]> {
  const params = new URLSearchParams();

  if (filters.minCapacity !== undefined)
    params.append("minCapacity", String(filters.minCapacity));
  if (filters.floor !== undefined) params.append("floor", String(filters.floor));
  filters.equipment?.forEach((item) => params.append("equipment", item));

  const query = params.toString();
  return apiRequest<Room[]>(`/rooms${query ? `?${query}` : ""}`);
}
