import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { choreManager } from "../managers/ChoreManager"; // Assuming you have a chore manager for API calls

const ChoreDetails = () => {
  const { id } = useParams();
  const [chore, setChore] = useState(null);

  useEffect(() => {
    choreManager.getChoreDetails(id).then(setChore).catch(console.error);
  }, [id]);

  if (!chore) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>Chore Details</h1>
      <p>Name: {chore.name}</p>
      <p>Frequency: {chore.choreFrequencyDays} days</p>
      <p>Difficulty: {chore.difficulty}</p>
      
      <h2>Assignees</h2>
      <ul>
        {chore.choreAssignments.map(assignment => (
          <li key={assignment.id}>
            {assignment.userProfile.firstName} {assignment.userProfile.lastName}
          </li>
        ))}
      </ul>
      
      <h2>Most Recent Completion</h2>
      {chore.choreCompletions.length > 0 ? (
        <p>
          {new Date(chore.choreCompletions[0].completedOn).toLocaleString()} by {chore.choreAssignments.find(a => a.userProfileId === chore.choreCompletions[0].userProfileId)?.userProfile.firstName} {chore.choreAssignments.find(a => a.userProfileId === chore.choreCompletions[0].userProfileId)?.userProfile.lastName}
        </p>
      ) : (
        <p>No completions yet</p>
      )}
    </div>
  );
};

export default ChoreDetails;