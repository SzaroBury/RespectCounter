import api from "../utils/refreshInterceptor/refreshInterceptor";

export const getPerson = async (personId) => {
    const response = await api.get(`/api/person/${personId}`);
    return response.data;
};

export const hidePerson = async (personId) => {
    await api.post('/api/person/hide', { personId });
};

export const verifyPerson = async (personId) => {
    axios.defaults.withCredentials = true;
    await api.put(`/api/person/${personId}/verify`);
};