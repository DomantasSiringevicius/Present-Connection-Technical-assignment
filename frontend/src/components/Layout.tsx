import { NavLink, Outlet } from "react-router-dom";
import { AppBar, Box, Button, Container, Toolbar, Typography } from "@mui/material";

const navItems = [
  { to: "/rooms", label: "Rooms" },
  { to: "/users", label: "Users" },
  { to: "/reservations", label: "Reservations" },
];

export default function Layout() {
  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Room Reservation
          </Typography>
          <Box sx={{ display: "flex", gap: 1 }}>
            {navItems.map((item) => (
              <Button
                key={item.to}
                component={NavLink}
                to={item.to}
                color="inherit"
                sx={{ "&.active": { bgcolor: "rgba(255, 255, 255, 0.15)" } }}
              >
                {item.label}
              </Button>
            ))}
          </Box>
        </Toolbar>
      </AppBar>
      <Container sx={{ py: 4 }}>
        <Outlet />
      </Container>
    </>
  );
}
