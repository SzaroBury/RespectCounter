import api from './apiService';

export const getPersons = (params) => api.get('/persons', { params });
export const getPersonById = (id) => api.get(`/persons/${id}`);
export const addPerson = (data) => api.post('/persons', data);
export const updatePerson = (id, data) => api.put(`/persons/${id}`, data);
export const deletePerson = (id) => api.delete(`/persons/${id}`);