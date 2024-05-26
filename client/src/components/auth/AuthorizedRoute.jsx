import { Navigate } from "react-router-dom";

// This component returns a Route that either displays the prop element
// or navigates to the login. If roles are provided, the route will require
// all of the roles when all is true, or any of the roles when all is false
export const AuthorizedRoute = ({ children, loggedInUser, roles }) => {
  let authed = false;
  if (loggedInUser) {
    if (roles && roles.length) {
      authed = roles.some((r) => loggedInUser.roles.includes(r));
    } else {
      authed = true; // If no roles are specified, just check for authentication
    }
  }

  return authed ? children : <Navigate to="/login" />;
};