import api from "../utils/refreshInterceptor/refreshInterceptor";

export const getPersons = async ({ tags = [], order = "", onlyVerified = false } = {}) => {
    const params = {};
    if (tags && tags.length > 0) params.tags = tags.map(ts => ts.name).join(",");
    if (order) params.order = order;
    const targetUrl = onlyVerified ? '' : '/all'

    return api.get(`/api/persons${targetUrl}`, { params });
};

export const getPerson = (personId) => {
    return api.get(`/api/person/${personId}`);
};

export const getPersonsNames = () => {
    return api.get(`/api/person/names`);
};

export const postPerson = (person) => {
    api.post('/api/person', person);
};

export const hidePerson = (personId) => {
    api.post('/api/person/hide', { personId });
};

export const verifyPerson = (personId) => {
    axios.defaults.withCredentials = true;
    api.put(`/api/person/${personId}/verify`);
};