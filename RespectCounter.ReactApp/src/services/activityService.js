import api from "../utils/refreshInterceptor/refreshInterceptor";


export const getActivities = async ({ tags = [], order = "", onlyVerified = false } = {}) => {
    const params = {};
    if (tags && tags.length > 0) params.tags = tags.map(ts => ts.name).join(",");
    if (order) params.order = order;
    const targetUrl = onlyVerified ? '' : '/all'
    
    const result = await api.get(`/api/activities${targetUrl}`, { params });
    return result.data;
};

export const getActivityById = async (actId) => {
    return await api.get(`/api/activity/${actId}`)
};

export const getActivitiesByPersonId = async (personId, { type = "", order = "", onlyVerified = false } = {}) => {
    const params = {};
    if (type) params.type = type;
    if (order) params.order = order;
    if (onlyVerified !== undefined) params.onlyVerified = onlyVerified;
    return await api.get(`/api/person/${personId}/activities`, { params });
};

export const hideActivity = async (actId) => {
    await api.post(`/api/activity/${actId}/hide`, formData);
    localStorage.setItem("user", JSON.stringify({ userName: formData.username }));
    return { userName: formData.username };
};

export const verifyActivity = async (actId) => {
    await api.put(`/api/activity/${actId}/verify`);
};