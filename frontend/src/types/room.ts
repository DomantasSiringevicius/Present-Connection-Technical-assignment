export const EQUIPMENT = [
  "Projector",
  "Whiteboard",
  "TvScreen",
  "VideoConferencing",
] as const;

export type Equipment = (typeof EQUIPMENT)[number];

export interface Room {
  id: string;
  name: string;
  capacity: number;
  floor: number;
  equipment: Equipment[];
  isActive: boolean;
}

export interface RoomFilters {
  minCapacity?: number;
  floor?: number;
  equipment?: Equipment[];
}
