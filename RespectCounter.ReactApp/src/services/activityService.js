import api from "../utils/interceptors/refreshInterceptor";

export const getActivities = async ({ tags = [], order = '', onlyVerified, page, pageSize } = {}) => {
    const params = {};
    if (tags && tags.length > 0) params.tags = tags.map(ts => ts.name).join(",");
    if (order) params.order = order;
    if (page) params.page = page;
    if (pageSize) params.pageSize = pageSize;
    const targetUrl = onlyVerified ? '' : '/all'
    return api.get(`/api/activities${targetUrl}`, { params });
};

export const getActivityById = async (actId) => {
    return api.get(`/api/activity/${actId}`)
};

export const getActivitiesByPersonId = async (personId, { type = "", order = "", onlyVerified = false } = {}) => {
    const params = {};
    if (type) params.type = type;
    if (order) params.order = order;
    if (onlyVerified !== undefined) params.onlyVerified = onlyVerified;
    return api.get(`/api/person/${personId}/activities`, { params });
};

export const postActivity = async (activityForm) => {
    return api.post('/api/activity', activityForm)
};

export const hideActivity = async (actId) => {
    return api.post(`/api/activity/${actId}/hide`, formData);
};

export const verifyActivity = async (actId) => {
    return api.put(`/api/activity/${actId}/verify`);
};