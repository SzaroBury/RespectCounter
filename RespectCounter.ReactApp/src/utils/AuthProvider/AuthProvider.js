import React, { createContext, useContext, useState, useEffect } from 'react';
import LoginPopup from '../../components/LoginPopup/LoginPopup';
import axios from "axios";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [user, setUser] = useState(null);
    const [showLoginPopup, setShowLoginPopup] = useState(false);

    const openLoginPopup = () => setShowLoginPopup(true);
    const closeLoginPopup = () => setShowLoginPopup(false);

    useEffect(() => {
        const storedUser = localStorage.getItem("user");
        if (storedUser) {
            setIsLoggedIn(true);
            setUser(JSON.parse(storedUser));
        }
    }, []);

    const login = (userData) => {
        setIsLoggedIn(true);
        setUser(userData);
        localStorage.setItem("user", JSON.stringify(userData));
    };

    const checkIfLogged = async () => {
        if(!isLoggedIn)
        {
            return true;
        } else {
            try {
                console.log('AuthProvider: sending check request:');
                const response = await axios.post('/manage/info');
                console.log('AuthProvider: success ', response.data);
            } catch (error) {
                if (error.response && error.response.status === 401) {
                    console.warn('Unauthorized! Redirecting to login...');
                } else {
                    console.error('An error occurred:', error);
                }
            } 
        }
    };

    const logout = () => {
        setIsLoggedIn(false);
        setUser(null);
        localStorage.removeItem("user");
    };

    return (
        <AuthContext.Provider value={{ isLoggedIn, user, login, logout, openLoginPopup, closeLoginPopup, checkIfLogged }}>
            {children}
            {showLoginPopup && <LoginPopup/>} 
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);