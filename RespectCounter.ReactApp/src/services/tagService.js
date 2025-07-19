import api from "../utils/refreshInterceptor/refreshInterceptor";

export const getAllTags = async () => {
    return api.get(`/api/tags`);
};

export const getPersonTags = async (personId) => {
    return api.get(`/api/person/${personId}/tags`);
};

export const getSimpleTags = async () => {
    return api.get(`/api/tags/simple`);
};

export const getRecentlyBrowsedTags = async () => {
    return api.get(`/api/tags/recent`);
};

export const getFavouriteTags = async () => {
    return api.get(`/api/tags/favourite`);
};