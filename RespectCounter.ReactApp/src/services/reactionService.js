import api from "../utils/refreshInterceptor/refreshInterceptor";

export const postReaction = async ({ targetType, targetId, reaction }) => {
    let targetUrl = "";
    if (targetType === "activity") {
        targetUrl = `/api/activity/${targetId}/reaction/`;
    } else if (targetType === "person") {
        targetUrl = `/api/person/${targetId}/reaction/`;
    } else if (targetType === "comment") {
        targetUrl = `/api/comment/${targetId}/reaction/`;
    } else {
        throw new Error("Invalid targetType for reaction");
    }
    const response = await api.post(targetUrl + reaction.toString());
    return response.data;
};