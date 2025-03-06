import React, { createContext, useState, useContext, useEffect } from 'react';
import {useNavigate} from "react-router-dom";

const AuthContext = createContext();

export const useAuth = () => {
    return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
    const [login, setLogin] = useState(sessionStorage.getItem('login') || '');
    const navigate = useNavigate();
    const logoutUser = () => {
        sessionStorage.removeItem("accessToken");
        sessionStorage.removeItem("refreshToken");
        sessionStorage.removeItem("role");
        sessionStorage.removeItem("login")
        setLogin(null);
        navigate('/');
    };
    useEffect(() => {
        const handleStorageChange = () => {
            setLogin(sessionStorage.getItem('login') || '');
        };

        window.addEventListener('storage', handleStorageChange);

        return () => {
            window.removeEventListener('storage', handleStorageChange);
        };
    }, []);

    return (
        <AuthContext.Provider value={{ login, setLogin, logout: logoutUser }}>
            {children}
        </AuthContext.Provider>
    );
};
