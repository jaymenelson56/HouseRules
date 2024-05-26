import React, { useEffect, useState } from "react";
import { userProfileManager } from "../managers/UserProfileManager";
import { Link } from "react-router-dom";


const UserProfileList = () => {
  const [userProfiles, setUserProfiles] = useState([]);

  useEffect(() => {
    userProfileManager.getAllUserProfiles().then(setUserProfiles).catch(console.error);
  }, []);

  return (
    <div>
      <h1>User Profiles</h1>
      <ul>
        {userProfiles.map(profile => (
          <li key={profile.id}>
            {profile.firstName} {profile.lastName} - {profile.email}
            <Link to={`/userprofiles/${profile.id}`}>Details</Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default UserProfileList;