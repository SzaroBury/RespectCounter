import api from "../utils/refreshInterceptor/refreshInterceptor";

export const getPersonTags = async (personId) => {
    return api.get(`/api/person/${personId}/tags`);
};

export const getSimpleTags = async () => {
    return api.get(`/api/tags/simple`);
};