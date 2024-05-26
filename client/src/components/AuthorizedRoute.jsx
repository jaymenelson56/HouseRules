import React from 'react';
import { Navigate } from 'react-router-dom';

export function AuthorizedRoute({ element, isAuthenticated }) {
  return isAuthenticated ? element : <Navigate to="/login" />;
}