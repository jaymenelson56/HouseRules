export const userProfileManager = {
    async getAllUserProfiles() {
      const response = await fetch('/api/userprofile');
      if (!response.ok) {
        throw new Error('Failed to fetch user profiles');
      }
      return await response.json();
    },
  
    async getUserProfileById(id) {
      const response = await fetch(`/api/userprofile/${id}`);
      if (!response.ok) {
        throw new Error('Failed to fetch user profile');
      }
      return await response.json();
    }
    
  };