import { ShoppingCart } from "@mui/icons-material";
import { AppBar, Badge, Box, IconButton, List, ListItem, Switch, Toolbar, Typography } from "@mui/material";
import { Link, NavLink } from "react-router-dom";
import { useAppSelector } from "../store/configureStore";
import SignedInMenu from "./SignedInMenu";

interface Props {
  darkMode: boolean;
  handleThemeChange: () => void;
}

const midLinks = [
  { title: "catalog", path: "/catalog" },
  { title: "about", path: "/about" },
  { title: "contact", path: "/contact" },
];

const rightLinks = [
  { title: "login", path: "/login" },
  { title: "register", path: "/register" },
];

const navStyles = {
  color: "inherit",
  textDecoration: "none",
  typography: "h6",
  "&:active": {
    color: "text.secondary"
  },
  "&:hover": {
    color: "grey.500"
  }
};

export default function Header({ darkMode, handleThemeChange }: Props) {
  const { basket } = useAppSelector(state => state.basket);
  const { user } = useAppSelector(state => state.account);
  const itemCount = basket?.items.reduce((sum, item) => sum + item.quantity, 0);


  return (
    <AppBar position="static">
      <Toolbar sx={{ alignItems: "center", display: "flex", justifyContent: "space-between" }}>
        <Box alignItems="center" display="flex">
          <Typography component={NavLink} exact to="/" variant="h6" sx={navStyles}>RE-STORE</Typography>
          <Switch checked={darkMode} onChange={handleThemeChange} />
        </Box>
        <List sx={{ display: "flex" }}>
          {midLinks.map(({ title, path }) => (
            <ListItem component={NavLink} key={path} to={path} sx={navStyles}>
              {title.toUpperCase()}
            </ListItem>
          ))}
          {
            user && user.roles?.includes("Admin") &&
            <ListItem component={NavLink} to={"/inventory"} sx={navStyles}>
              INVENTORY
            </ListItem>
          }
        </List>
        <Box alignItems="center" display="flex">
          <IconButton component={Link} to="/basket" size="large" sx={{ color: "inherit" }}>
            <Badge badgeContent={itemCount} color="secondary">
              <ShoppingCart />
            </Badge>
          </IconButton>
          {user ? (<SignedInMenu />) : (
            <List sx={{ display: "flex" }}>
              {rightLinks.map(({ title, path }) => (
                <ListItem component={NavLink} key={path} to={path} sx={navStyles}>
                  {title.toUpperCase()}
                </ListItem>
              ))}
            </List>)
          }
        </Box>
      </Toolbar>
    </AppBar>
  );
}