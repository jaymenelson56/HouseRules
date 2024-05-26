
import { Route, Routes } from 'react-router-dom';
import { AuthorizedRoute } from './auth/AuthorizedRoute';
import Login from './auth/Login';
import Register from './auth/Register';
import Home from './Home';
import UserProfileList from './UserProfileList';
import UserProfileDetails from './UserProfileDetails';
import ChoresList from './ChoreList';
import ChoreDetails from './ChoreDetails';
import CreateChore from './CreateChore';

export default function ApplicationViews({ loggedInUser, setLoggedInUser }) {

  return (
    <Routes>
      <Route path="/" element={<AuthorizedRoute loggedInUser={loggedInUser}><Home /></AuthorizedRoute>} index />
      <Route path="login" element={<Login setLoggedInUser={setLoggedInUser} />} />
      <Route path="register" element={<Register setLoggedInUser={setLoggedInUser} />} />
      <Route path="*" element={<p>Whoops, nothing here...</p>} />

      <Route path="/userprofiles" >
        <Route index element={<AuthorizedRoute loggedInUser={loggedInUser} roles={['Admin']} ><UserProfileList />
        </AuthorizedRoute>} />
        <Route path=":id" element={<UserProfileDetails />} />
      </Route>
      <Route path="/chores" >
        <Route index element={<AuthorizedRoute loggedInUser={loggedInUser} roles={[]}><ChoresList loggedInUser={loggedInUser}/>
        </AuthorizedRoute>} />
        <Route path="create" element={<AuthorizedRoute loggedInUser={loggedInUser} roles={['Admin']}><CreateChore /></AuthorizedRoute>} />
        <Route path=":id" element={<AuthorizedRoute loggedInUser={loggedInUser} roles={['Admin']}><ChoreDetails /></AuthorizedRoute>} />
        
          </Route>
        
        
    </Routes>
  );
}