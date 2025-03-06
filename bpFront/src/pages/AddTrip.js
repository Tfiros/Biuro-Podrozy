import React, { useState } from "react";
import {useNavigate} from "react-router-dom";
import { useAuth } from '../AuthContext';

const AddTrip = () => {
    const navigate = useNavigate();
    const { logoutUser } = useAuth();
    const [tripData, setTripData] = useState({
        name: "",
        description: "",
        price: "",
        categoryId: "",
        country: "",
        imageLink: ""
    });
    const [error, setError] = useState("");

    const handleChange = (e) => {
        const { name, value } = e.target;
        setTripData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };
    const getNewAccessToken = async () => {
        try {
            const refreshToken = sessionStorage.getItem("refreshToken");
            const response = await fetch("http://localhost:5029/api/User/refresh", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${sessionStorage.getItem("accessToken")}`
                },
                body: JSON.stringify({ refreshToken }),
            });

            if (response.ok) {
                const data = await response.json();
                sessionStorage.setItem("accessToken", data.token);
                sessionStorage.setItem("refreshToken", data.refreshToken);
                return data.token;
            } else {
                setError("Błąd odświeżania tokenu. Zaloguj się ponownie.");
                logoutUser();
                return null;
            }
        } catch (err) {
            setError("Błąd podczas próby odświeżenia tokenu.");
            logoutUser();
            return null;
        }
    };
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        if (!tripData.title || !tripData.description || !tripData.price || !tripData.category || !tripData.country) {
            setError("Wszystkie pola muszą być wypełnione.");
            return;
        }

        try {
            const response = await fetch("http://localhost:5029/api/Trips", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${sessionStorage.getItem("accessToken")}`,
                },
                body: JSON.stringify(tripData),
            });
            if (response.status === 401) {
                const accessToken = await getNewAccessToken();
                if (accessToken) {
                    return handleSubmit(e);
                } else {
                    setError("Sesja wygasła. Proszę się zalogować ponownie.");
                    logoutUser();
                    return;
                }
            }

            if (!response.ok) {
                throw new Error("Błąd podczas dodawania wycieczki");
            }

            navigate("/trips");
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="add-trip-container">
            <h2>Dodaj nową wycieczkę</h2>
            {error && <p className="error">{error}</p>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="title">Nazwa</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        value={tripData.name}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="description">Opis</label>
                    <textarea
                        id="description"
                        name="description"
                        value={tripData.description}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="price">Cena</label>
                    <input
                        type="number"
                        id="price"
                        name="price"
                        value={tripData.price}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="categoryId">Kategoria</label>
                    <select
                        id="categoryId"
                        name="categoryId"
                        value={tripData.categoryId}
                        onChange={handleChange}
                        required
                    >
                        <option value="">Wybierz kategorię</option>
                        <option value="1">Jednodniowa</option>
                        <option value="2">Objazdowa</option>
                        <option value="3">Pobyt stały</option>
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="country">Kraj</label>
                    <input
                        type="text"
                        id="country"
                        name="country"
                        value={tripData.country}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="imageLink">Ściezka do zdjecia</label>
                    <input
                        type="text"
                        id="imageLink"
                        name="imageLink"
                        value={tripData.imageLink}
                        onChange={handleChange}
                        required
                    />
                </div>
                <button type="submit">Dodaj wycieczkę</button>
            </form>
        </div>
    );
};

export default AddTrip;
