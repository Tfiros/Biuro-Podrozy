import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { useAuth } from '../AuthContext';
import './EditTrip.css'
const TripEdit = () => {
    const { logoutUser } = useAuth();
    const [tripData, setTripData] = useState({
        name: "",
        description: "",
        price: "",
        categoryId: "",
        country: "",
        imagePath: "",
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const { id } = useParams();
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
    useEffect(() => {
        fetch(`http://localhost:5029/api/Trips/${id}`)
            .then((response) => response.json())
            .then((data) => {
                setTripData({
                    name: data.name,
                    description: data.description,
                    price: data.price,
                    categoryId: data.tripCategory,
                    country: data.country,
                    imagePath: data.imagePath,
                });
                setLoading(false);
            })
            .catch(() => {
                setError("Błąd podczas ładowania danych wycieczki.");
                setLoading(false);
            });
    }, [id]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setTripData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        if (!tripData.name || !tripData.price || !tripData.country || !tripData.imagePath) {
            setError("Wszystkie pola są wymagane!");
            setLoading(false);
            return;
        }

        await fetch(`http://localhost:5029/api/Trips/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${sessionStorage.getItem("accessToken")}`,
            },
            body: JSON.stringify(tripData),
        })
            .then(async (response) => {
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
                return response.text();
            })
            .then(() => {
                console.log("Dane zapisane pomyślnie:");
                setLoading(false);
            })
            .catch((err) => {
                console.error("Błąd podczas zapisywania danych:", err);
                setError("Błąd podczas zapisywania danych.");
                setLoading(false);
            });
    };

    if (loading) return <p>Ładowanie...</p>;

    return (
        <div className="edit-trip-container">
            <h2>Edytuj wycieczkę</h2>
            {error && <p className="error">{error}</p>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="name">Nazwa</label>
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
                    <label htmlFor="imagePath">Ścieżka do zdjęcia</label>
                    <input
                        type="text"
                        id="imagePath"
                        name="imagePath"
                        value={tripData.imagePath}
                        onChange={handleChange}
                        required
                    />
                </div>

                <button type="submit" disabled={loading}>
                    {loading ? "Zapisuję..." : "Zapisz zmiany"}
                </button>
            </form>
        </div>
    );
};

export default TripEdit;
