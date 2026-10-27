/* eslint-disable react-refresh/only-export-components */
import { useEffect, useState } from "react";
import {
  Alert,
  Box,
  Button,
  Chip,
  CircularProgress,
  FormControl,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
} from "@mui/material";
import { ApiError } from "../api/client";
import { cancelReservation, endReservationEarly, getReservations } from "../api/reservations";
import { getRooms } from "../api/rooms";
import { getUsers } from "../api/users";
import CreateReservationDialog from "../components/CreateReservationDialog";
import { RESERVATION_STATUS, type Reservation, type ReservationStatus } from "../types/reservation";
import type { Room } from "../types/room";
import type { User } from "../types/user";

const statusColor: Record<ReservationStatus, "primary" | "success" | "default"> = {
  Booked: "primary",
  Completed: "success",
  Cancelled: "default",
};

function formatDate(value: string) {
  return new Date(value).toLocaleString(undefined, { dateStyle: "short", timeStyle: "short" });
}

function toUtcIso(date: string, addDays = 0) {
  const result = new Date(`${date}T00:00`);
  result.setDate(result.getDate() + addDays);
  return result.toISOString();
}

export function errorMessage(err: unknown) {
  if (err instanceof ApiError) {
    return err.code ? `[${err.code}] ${err.message}` : err.message;
  }
  return err instanceof Error ? err.message : "Unexpected error";
}

export default function ReservationsPage() {
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [rooms, setRooms] = useState<Room[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [reloadKey, setReloadKey] = useState(0);
  const [dialogOpen, setDialogOpen] = useState(false);

  const [roomId, setRoomId] = useState("");
  const [organizerId, setOrganizerId] = useState("");
  const [status, setStatus] = useState<ReservationStatus | "">("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  // Rooms and users are only needed for the dropdowns, so they are loaded once
  useEffect(() => {
    Promise.all([getRooms(), getUsers()])
      .then(([roomData, userData]) => {
        setRooms(roomData);
        setUsers(userData);
      })
      .catch((err) => setError(errorMessage(err)));
  }, []);

  useEffect(() => {
    let ignore = false;

    setLoading(true);
    setError(null);

    getReservations({
      roomId: roomId || undefined,
      organizerId: organizerId || undefined,
      status: status || undefined,
      startDate: startDate ? toUtcIso(startDate) : undefined,
      endDate: endDate ? toUtcIso(endDate, 1) : undefined,
    })
      .then((data) => {
        if (!ignore) setReservations(data);
      })
      .catch((err) => {
        if (!ignore) setError(errorMessage(err));
      })
      .finally(() => {
        if (!ignore) setLoading(false);
      });

    return () => {
      ignore = true;
    };
  }, [roomId, organizerId, status, startDate, endDate, reloadKey]);

  const reload = () => setReloadKey((key) => key + 1);

  async function runAction(action: () => Promise<unknown>) {
    setError(null);
    try {
      await action();
      reload();
    } catch (err) {
      setError(errorMessage(err));
    }
  }

  const now = new Date();

  return (
    <Stack spacing={3}>
      <Stack direction="row" sx={{ justifyContent: "space-between", alignItems: "center" }}>
        <Typography variant="h4">Reservations</Typography>
        <Button variant="contained" onClick={() => setDialogOpen(true)}>
          New reservation
        </Button>
      </Stack>

      <Stack direction={{ xs: "column", md: "row" }} spacing={2}>
        <FormControl sx={{ minWidth: 160 }}>
          <InputLabel id="room-filter-label">Room</InputLabel>
          <Select
            labelId="room-filter-label"
            label="Room"
            value={roomId}
            onChange={(e) => setRoomId(e.target.value)}
          >
            <MenuItem value="">All</MenuItem>
            {rooms.map((room) => (
              <MenuItem key={room.id} value={room.id}>
                {room.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <FormControl sx={{ minWidth: 200 }}>
          <InputLabel id="organizer-filter-label">Organizer</InputLabel>
          <Select
            labelId="organizer-filter-label"
            label="Organizer"
            value={organizerId}
            onChange={(e) => setOrganizerId(e.target.value)}
          >
            <MenuItem value="">All</MenuItem>
            {users.map((user) => (
              <MenuItem key={user.id} value={user.id}>
                {user.name} {user.lastName}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <FormControl sx={{ minWidth: 160 }}>
          <InputLabel id="status-filter-label">Status</InputLabel>
          <Select
            labelId="status-filter-label"
            label="Status"
            value={status}
            onChange={(e) => setStatus(e.target.value as ReservationStatus | "")}
          >
            <MenuItem value="">All</MenuItem>
            {RESERVATION_STATUS.map((item) => (
              <MenuItem key={item} value={item}>
                {item}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <TextField
          label="From"
          type="date"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
          slotProps={{ inputLabel: { shrink: true } }}
        />
        <TextField
          label="To"
          type="date"
          value={endDate}
          onChange={(e) => setEndDate(e.target.value)}
          slotProps={{ inputLabel: { shrink: true } }}
        />
      </Stack>

      {error && <Alert severity="error">{error}</Alert>}

      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 4 }}>
          <CircularProgress />
        </Box>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Title</TableCell>
                <TableCell>Room</TableCell>
                <TableCell>Organizer</TableCell>
                <TableCell>Start</TableCell>
                <TableCell>End</TableCell>
                <TableCell align="right">Attendees</TableCell>
                <TableCell>Status</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {reservations.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={8} align="center">
                    No reservations match the selected filters.
                  </TableCell>
                </TableRow>
              ) : (
                reservations.map((reservation) => {
                  const start = new Date(reservation.startDate);
                  const end = new Date(reservation.endDate);
                  const isBooked = reservation.reservationStatus === "Booked";
                  const canCancel = isBooked && end > now;
                  const canEndEarly = isBooked && start <= now && end > now;

                  return (
                    <TableRow key={reservation.id}>
                      <TableCell>{reservation.meetingTitle}</TableCell>
                      <TableCell>{reservation.roomName}</TableCell>
                      <TableCell>
                        {reservation.organizerName} {reservation.organizerLastName}
                      </TableCell>
                      <TableCell>{formatDate(reservation.startDate)}</TableCell>
                      <TableCell>
                        {formatDate(reservation.endDate)}
                        {reservation.actualEndDate && (
                          <Typography variant="caption" sx={{ display: "block", color: "text.secondary" }}>
                            Ended early: {formatDate(reservation.actualEndDate)}
                          </Typography>
                        )}
                      </TableCell>
                      <TableCell align="right">{reservation.numberOfAttendees}</TableCell>
                      <TableCell>
                        <Chip
                          label={reservation.reservationStatus}
                          color={statusColor[reservation.reservationStatus]}
                          size="small"
                        />
                      </TableCell>
                      <TableCell align="right">
                        <Stack direction="row" spacing={1} sx={{ justifyContent: "flex-end" }}>
                          {canEndEarly && (
                            <Button size="small" onClick={() => runAction(() => endReservationEarly(reservation.id))}>
                              End early
                            </Button>
                          )}
                          {canCancel && (
                            <Button
                              size="small"
                              color="error"
                              onClick={() => runAction(() => cancelReservation(reservation.id))}
                            >
                              Cancel
                            </Button>
                          )}
                        </Stack>
                      </TableCell>
                    </TableRow>
                  );
                })
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <CreateReservationDialog
        open={dialogOpen}
        rooms={rooms}
        users={users}
        onClose={() => setDialogOpen(false)}
        onCreated={() => {
          setDialogOpen(false);
          reload();
        }}
      />
    </Stack>
  );
}
