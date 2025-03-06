import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../AuthContext";
import "./Login.css";

const Login = () => {
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();
    const { setLogin: setAuthLogin} = useAuth();

    const handleSubmit = (e) => {
        e.preventDefault();

        fetch("http://localhost:5029/api/User/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ login: login, password: password }),
        })
            .then((response) => {
                if (response.status === 401) {
                    setErrorMessage("Niepoprawny login lub hasło.");
                    throw new Error("Niepoprawny login lub hasło.");
                } else {
                    return response.json();
                }
            })
            .then((data) => {

                if (data.token && data.refreshToken) {
                    sessionStorage.setItem("accessToken", data.token);
                    sessionStorage.setItem("refreshToken", data.refreshToken);

                    const tokenParts = data.token.split('.');
                    if (tokenParts.length === 3) {
                        const decodedPayload = atob(tokenParts[1]);
                        const parsedPayload = JSON.parse(decodedPayload);

                        const userLogin = parsedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
                        const userRole = parsedPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]

                        sessionStorage.setItem("login", userLogin);
                        sessionStorage.setItem("role",userRole)
                        console.log("Zalogowano pomyslnie");

                        setAuthLogin(userLogin);
                        navigate("/");
                    } else {
                        console.error("Token JWT jest niepoprawny.");
                    }
                } else {
                    console.error("Brak tokenu lub refreshTokenu w odpowiedzi");
                }
            })
            .catch((error) => {
                console.error("Błąd podczas logowania:", error);
            });
    };

    return (
        <form onSubmit={handleSubmit} className="login-form">
            <h2>Login</h2>
            {errorMessage && <p className="error-message">{errorMessage}</p>}
            <input
                type="text"
                placeholder="Login"
                value={login}
                onChange={(e) => setLogin(e.target.value)}
                required
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
            />
            <button type="submit" className="login-button">Login</button>
            <p>
                Nie masz konta?{" "}
                <button onClick={() => navigate("/register")} className="toggle-button">
                    Zarejestruj się
                </button>
            </p>
        </form>

    );
};

export default Login;
