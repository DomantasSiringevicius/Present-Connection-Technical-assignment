import { useState, type ChangeEvent } from "react";
import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  MenuItem,
  Stack,
  TextField,
} from "@mui/material";
import { createReservation } from "../api/reservations";
import { errorMessage } from "../pages/ReservationsPage";
import type { Room } from "../types/room";
import type { User } from "../types/user";

interface CreateReservationDialogProps {
  open: boolean;
  rooms: Room[];
  users: User[];
  onClose: () => void;
  onCreated: () => void;
}

const emptyForm = {
  roomId: "",
  organizerId: "",
  meetingTitle: "",
  startDate: "",
  endDate: "",
  numberOfAttendees: "1",
};

type FormField = keyof typeof emptyForm;

export default function CreateReservationDialog({ open, rooms, users, onClose, onCreated }: CreateReservationDialogProps) {
  const [form, setForm] = useState(emptyForm);
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);

  const isComplete = Object.values(form).every((value) => value.trim() !== "");

  const update = (field: FormField) => (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) =>
    setForm({ ...form, [field]: e.target.value });

  function handleClose() {
    setForm(emptyForm);
    setError(null);
    onClose();
  }

  async function handleSubmit() {
    setSaving(true);
    setError(null);

    try {
      await createReservation({
        roomId: form.roomId,
        organizerId: form.organizerId,
        meetingTitle: form.meetingTitle.trim(),
        // datetime-local values are local time; toISOString converts them to UTC for the API
        startDate: new Date(form.startDate).toISOString(),
        endDate: new Date(form.endDate).toISOString(),
        numberOfAttendees: Number(form.numberOfAttendees),
      });
      setForm(emptyForm);
      onCreated();
    } catch (err) {
      setError(errorMessage(err));
    } finally {
      setSaving(false);
    }
  }

  return (
    <Dialog open={open} onClose={handleClose} fullWidth maxWidth="sm">
      <DialogTitle>New reservation</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ pt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}

          <TextField select label="Room" value={form.roomId} onChange={update("roomId")}>
            {rooms.map((room) => (
              <MenuItem key={room.id} value={room.id}>
                {room.name} (capacity {room.capacity}){room.isActive ? "" : " – inactive"}
              </MenuItem>
            ))}
          </TextField>

          <TextField select label="Organizer" value={form.organizerId} onChange={update("organizerId")}>
            {users.map((user) => (
              <MenuItem key={user.id} value={user.id}>
                {user.name} {user.lastName}
              </MenuItem>
            ))}
          </TextField>

          <TextField label="Meeting title" value={form.meetingTitle} onChange={update("meetingTitle")} />

          <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
            <TextField
              label="Start"
              type="datetime-local"
              value={form.startDate}
              onChange={update("startDate")}
              slotProps={{ inputLabel: { shrink: true } }}
              fullWidth
            />
            <TextField
              label="End"
              type="datetime-local"
              value={form.endDate}
              onChange={update("endDate")}
              slotProps={{ inputLabel: { shrink: true } }}
              fullWidth
            />
          </Stack>

          <TextField
            label="Number of attendees"
            type="number"
            value={form.numberOfAttendees}
            onChange={update("numberOfAttendees")}
            slotProps={{ htmlInput: { min: 1 } }}
          />
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button variant="contained" onClick={handleSubmit} disabled={!isComplete || saving}>
          {saving ? "Saving..." : "Create"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
