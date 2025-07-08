import api from "../utils/refreshInterceptor/refreshInterceptor";

export const login = (formData) => {
    return api.post('/api/auth/login', formData);
};

export const logout = () => {
    api.post('/api/auth/logout');
};

export const getClaims = async () => {
    console.log("authService: checkIfLogged()")
    try {
        return await api.get('/api/auth/claims');
    } catch (error) {
        if (error.response && error.response.status === 401) {
            return false;
        } else {
            console.error('An error occurred:', error);
            return false;
        }
    }
};

export const register = (formData) => {
    return api.post('/api/auth/register', formData);
};

export const getCurrentUser = () => {
    const storedUser = localStorage.getItem("user");
    return storedUser ? JSON.parse(storedUser) : null;
};