import React, { useEffect, useState } from "react";
import { choreManager } from "../managers/ChoreManager"; // Assuming you have a chore manager for API calls
import { Link } from "react-router-dom";

const ChoresList = ({ loggedInUser }) => {
    const [chores, setChores] = useState([]);

    useEffect(() => {
        choreManager.getAllChores().then(setChores).catch(console.error);
    }, []);

    const handleDelete = (choreId) => {
        choreManager.deleteChore(choreId)
            .then(() => setChores(chores.filter(chore => chore.id !== choreId)))
            .catch(console.error);


    };

    return (
        <div>
            <h1>Chores List</h1>
            <ul>
                {chores.map(chore => (
                    <li key={chore.id}>
                        {chore.name} - {chore.frequency} days - {chore.difficulty}
                        {loggedInUser.roles.includes('Admin') && (
                            <>
                                <button onClick={() => handleDelete(chore.id)}>Delete</button>
                                <Link to={`/chores/${chore.id}`}>Details</Link>
                            </>
                        )}
                    </li>
                ))}
            </ul>
            {loggedInUser.roles.includes('Admin') && (
                <Link to="create">Create Chore</Link>
            )}
        </div>
    );
};

export default ChoresList;