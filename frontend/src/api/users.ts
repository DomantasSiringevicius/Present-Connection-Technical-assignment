import type { User } from "../types/user";
import { apiRequest } from "./client";

export function getUsers(): Promise<User[]> {
  return apiRequest<User[]>("/users");
}
