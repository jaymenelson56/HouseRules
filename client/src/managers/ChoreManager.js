export const choreManager = {
    async getAllChores() {
        const response = await fetch('/api/chore');
        if (!response.ok) {
            throw new Error('Failed to fetch chores');
        }
        return await response.json();
    },
    async getChoreDetails(id) {
        const response = await fetch(`/api/chore/${id}`);
        if (!response.ok) {
            throw new Error('Failed to fetch chore details');
        }
        return await response.json();
    },
    async createChore(choreData) {
        const response = await fetch('/api/chore', {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(choreData)
        });
        if (!response.ok) {
            throw new Error("Failed to create chore");
        }
    },
    async deleteChore(choreId) {
        const response = await fetch(`/api/chore/${choreId}`, {
            method: 'DELETE',
        });
        if (!response.ok) {
            throw new Error('Failed to delete chore');
        }
    }
};