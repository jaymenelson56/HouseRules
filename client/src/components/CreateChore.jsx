import React, { useState } from "react";
import { choreManager } from "../managers/ChoreManager"; // Assuming you have a chore manager for API calls

const CreateChore = () => {
  const [formData, setFormData] = useState({
    name: "",
    difficulty: "",
    choreFrequencyDays: ""
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    choreManager.createChore(formData)
      .then(() => {
        // Handle success, such as displaying a success message or redirecting to another page
        console.log("Chore created successfully");
      })
      .catch(error => {
        // Handle error, such as displaying an error message
        console.error("Error creating chore:", error);
      });
  };

  return (
    <div>
      <h1>Create Chore</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Name:</label>
          <input type="text" name="name" value={formData.name} onChange={handleChange} required />
        </div>
        <div>
          <label>Difficulty:</label>
          <input type="number" name="difficulty" value={formData.difficulty} onChange={handleChange} required />
        </div>
        <div>
          <label>Chore Frequency (Days):</label>
          <input type="number" name="choreFrequencyDays" value={formData.choreFrequencyDays} onChange={handleChange} required />
        </div>
        <button type="submit">Create Chore</button>
      </form>
    </div>
  );
};

export default CreateChore;