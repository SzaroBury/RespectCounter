import api from "../utils/refreshInterceptor/refreshInterceptor";

export const getCommentsByPersonId = async (personId, { order = "" } = {}) => {
    const params = {};
    if (order) params.order = order;
    const response = await api.get(`/api/person/${personId}/comments`);
    return response.data;
};

export const getCommentsByActivityId = async (actId) => {
    try {
        const response = await api.get(`/api/activity/${actId}/comments`);
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error(`HTTP error! Status: ${error.response.status}`);
        } else if (error.request) {
            console.error("No response received: ", error.request);
        } else {
            console.error("Error setting up the request: ", error.message);
        }
        throw error;
    }
};

export const postComment = async (targetUrl, content) => {
    return await api.post(targetUrl, content, { headers: { "Content-Type": "application/json" } });
};