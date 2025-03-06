import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Login.css";

const Register = () => {
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [address, setAddress] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const validateEmail = (email) => {
        const regex = /^[^\s@]+@[^\s@]+.[^\s@]+$/;
        return regex.test(email);
    };

    const validatePassword = (password) => {
        return password.length >= 6;
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (!validateEmail(email)) {
            setErrorMessage("Proszę podać poprawny adres email.");
            return;
        }

        if (!validatePassword(password)) {
            setErrorMessage("Hasło musi mieć co najmniej 6 znaków.");
            return;
        }

        if (!login || !firstName || !lastName || !address) {
            setErrorMessage("Wszystkie pola są wymagane.");
            return;
        }

        const userData = {
            login: login,
            password: password,
            firstName: firstName,
            lastName: lastName,
            email: email,
            address: address,
        };

        fetch("http://localhost:5029/api/User/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(userData),
        })
            .then((response) => {
                if (!response.ok) {
                    return response.json().then((errorData) => {
                        setErrorMessage(errorData.error || "Błąd podczas rejestracji.");
                        throw new Error("Rejestracja nieudana");
                    });
                }
                return response;
            })
            .then((data) => {
                console.log("Zarejestrowano pomyślnie", data);
                navigate("/login");
            })
            .catch((error) => {
                console.error("Błąd podczas rejestracji:", error);
            });
    };

    return (
        <form onSubmit={handleSubmit} className="login-form">
            <h2>Rejestracja</h2>
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
                placeholder="Hasło"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
            />
            <input
                type="text"
                placeholder="Imię"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                required
            />
            <input
                type="text"
                placeholder="Nazwisko"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                required
            />
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
            />
            <input
                type="text"
                placeholder="Adres"
                value={address}
                onChange={(e) => setAddress(e.target.value)}
                required
            />
            <button type="submit" className="login-button">Zarejestruj</button>
        </form>
    );
};

export default Register;
