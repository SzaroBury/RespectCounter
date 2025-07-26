import axios from "axios";
import { callHandleLogout } from "../providers/AuthProvider";

const api = axios.create({ withCredentials: true });

api.interceptors.response.use(
  response => response,
  async error => {
    if (error.response?.status === 401) {
        console.log("Refreshing tokens...")
      try {
        await api.post("/api/auth/refresh");
        return api(error.config);
      } catch (refreshError) {
        callHandleLogout
        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);

export default api;