import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { userProfileManager } from "../managers/UserProfileManager";


const UserProfileDetails = () => {
  const { id } = useParams();
  const [userProfile, setUserProfile] = useState(null);

  useEffect(() => {
    userProfileManager.getUserProfileById(id)
      .then(setUserProfile)
      .catch(console.error);
  }, [id]);

  if (!userProfile) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>User Profile Details</h1>
      <p><strong>First Name:</strong> {userProfile.firstName}</p>
      <p><strong>Last Name:</strong> {userProfile.lastName}</p>
      <p><strong>Address:</strong> {userProfile.address}</p>
      <p><strong>Email:</strong> {userProfile.email}</p>
      <p><strong>User Name:</strong> {userProfile.userName}</p>

      <h2>Assigned Chores</h2>
      <ul>
        {userProfile.choreAssignments.map(assignment => (
          <li key={assignment.chore.id}>
            {assignment.chore.name}
          </li>
        ))}
      </ul>

      <h2>Completed Chores</h2>
      <ul>
        {userProfile.choreCompletions.map(completion => (
          <li key={completion.id}>
            {completion.chore.name} - {new Date(completion.completedOn).toLocaleDateString()}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default UserProfileDetails;