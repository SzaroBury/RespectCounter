import api from "../utils/refreshInterceptor/refreshInterceptor";


export const getActivities = async ({ tags = [], order = '', onlyVerified } = {}) => {
    const params = {};
    if (tags && tags.length > 0) params.tags = tags.map(ts => ts.name).join(",");
    if (order) params.order = order;
    console.log('test');
    console.log('onlyVerified', onlyVerified);
    const targetUrl = onlyVerified ? '' : '/all'
    console.log('targetUrl', targetUrl);
    
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