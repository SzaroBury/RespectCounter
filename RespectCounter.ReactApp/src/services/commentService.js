import api from "../utils/refreshInterceptor/refreshInterceptor";

export const getCommentsByPersonId = async (personId, { order = "" } = {}) => {
    const params = {};
    if (order) params.order = order;
    const response = await api.get(`/api/person/${personId}/comments`);
    return response.data;
};

export const getCommentsByActivityId = (actId) => {
    return api.get(`/api/activity/${actId}/comments`);
};

export const postComment = async (targetUrl, content) => {
    return await api.post(targetUrl, content, { headers: { "Content-Type": "application/json" } });
};