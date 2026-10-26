import {
  Alert,
  Box,
  Chip,
  CircularProgress,
  FormControl,
  InputLabel,
  MenuItem,
  OutlinedInput,
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
import { useEffect, useState } from "react";
import { EQUIPMENT, type Equipment, type Room } from "../types/room";
import { getRooms } from "../api/rooms";

export default function RoomsPage() {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [minCapacity, setMinCapacity] = useState("");
  const [floor, setFloor] = useState("");
  const [equipment, setEquipment] = useState<Equipment[]>([]);
  const minCapacityInvalid = minCapacity !== "" && Number(minCapacity) < 1;
  const floorInvalid = floor !== "" && Number(floor) < 1;

  useEffect(() => {
    let ignore = false;

    setLoading(true);
    setError(null);

    getRooms({
      minCapacity: minCapacity && !minCapacityInvalid ? Number(minCapacity) : undefined,
      floor: floor && !floorInvalid ? Number(floor) : undefined,
      equipment,
    })
      .then((data) => {
        if (!ignore) setRooms(data);
      })
      .catch((err: Error) => {
        if (!ignore) setError(err.message);
      })
      .finally(() => {
        if (!ignore) setLoading(false);
      });

    return () => {
      ignore = true;
    };
  }, [minCapacity, floor, equipment, minCapacityInvalid, floorInvalid]);

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Rooms</Typography>

      <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
        <TextField
          label="Min capacity"
          type="number"
          value={minCapacity}
          onChange={(e) => setMinCapacity(e.target.value)}
          error={minCapacityInvalid}
          helperText={minCapacityInvalid ? "Must be at least 1" : " "}
          slotProps={{ htmlInput: { min: 1 } }}
        />
        <TextField
          label="Floor"
          type="number"
          value={floor}
          onChange={(e) => setFloor(e.target.value)}
          error={floorInvalid}
          helperText={floorInvalid ? "Must be at least 1" : " "}
          slotProps={{ htmlInput: { min: 1 } }}
        />
        <FormControl sx={{ minWidth: 260 }}>
          <InputLabel id="equipment-label">Equipment</InputLabel>
          <Select
            labelId="equipment-label"
            multiple
            value={equipment}
            onChange={(e) => setEquipment(e.target.value as Equipment[])}
            input={<OutlinedInput label="Equipment" />}
            renderValue={(selected) => (
              <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                {selected.map((item) => (
                  <Chip key={item} label={item} size="small" />
                ))}
              </Box>
            )}
          >
            {EQUIPMENT.map((item) => (
              <MenuItem key={item} value={item}>
                {item}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
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
                <TableCell>Name</TableCell>
                <TableCell>ID</TableCell>
                <TableCell align="right">Capacity</TableCell>
                <TableCell align="right">Floor</TableCell>
                <TableCell>Equipment</TableCell>
                <TableCell>Status</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {rooms.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} align="center">
                    No rooms match the selected filters.
                  </TableCell>
                </TableRow>
              ) : (
                rooms.map((room) => (
                  <TableRow key={room.id}>
                    <TableCell>{room.name}</TableCell>
                    <TableCell sx={{ fontFamily: "monospace", fontSize: 12 }}>
                      {room.id}
                    </TableCell>
                    <TableCell align="right">{room.capacity}</TableCell>
                    <TableCell align="right">{room.floor}</TableCell>
                    <TableCell>
                      <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                        {room.equipment.length === 0
                          ? "—"
                          : room.equipment.map((item) => (
                              <Chip key={item} label={item} size="small" />
                            ))}
                      </Box>
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={room.isActive ? "Active" : "Inactive"}
                        color={room.isActive ? "success" : "default"}
                        size="small"
                      />
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </Stack>
  );
}
