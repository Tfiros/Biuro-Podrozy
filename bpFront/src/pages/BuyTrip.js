import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth } from '../AuthContext';
import './BuyTrip.css'

const BuyTrip = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { logoutUser } = useAuth();
    const [trip, setTrip] = useState({});
    const [numOfParticipants, setNumOfParticipants] = useState(1);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [responseMessage, setResponseMessage] = useState("");

    useEffect(() => {
        fetch(`http://localhost:5029/api/Trips/${id}`)
            .then((response) => response.json())
            .then((data) => {
                setTrip(data);
                setLoading(false);
            })
            .catch(() => {
                setError("Błąd podczas ładowania szczegółów wycieczki.");
                setLoading(false);
            });
    }, [id]);
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
    const handleBuy = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");
        setResponseMessage("");

        const purchaseData = {
            tripID: id,
            startDate: trip.startDate,
            endDate: trip.endDate,
            numOfParticipants: parseInt(numOfParticipants, 10),
        };

        await fetch(`http://localhost:5029/api/Orders`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${sessionStorage.getItem("accessToken")}`,
            },
            body: JSON.stringify(purchaseData),
        })
            .then(async (response) => {
                if (!response.ok) {
                    if (response.status === 400) {
                        return response.text().then((text) => {
                            throw new Error(text || "Brak dostępnych miejsc dla podanej liczby uczestników.");
                        });
                    }
                    if (response.status === 401) {
                        const accessToken = await getNewAccessToken();
                        if (accessToken) {
                            return handleBuy(e);
                        } else {
                            setError("Sesja wygasła. Proszę się zalogować ponownie.");
                            logoutUser();
                            return;
                        }
                    }
                    throw new Error("Wystąpił błąd podczas przetwarzania żądania.");
                }
                return response.text();
            })
            .then(() => {
                setLoading(false);
                setResponseMessage("Wycieczka została zakupiona pomyślnie!");
                navigate(`/trip/${id}`);
            })
            .catch((err) => {
                setError(err.message || "Błąd podczas zakupu wycieczki.");
                setLoading(false);
            });
    };

    if (loading) return <p>Ładowanie...</p>;

    return (
        <div className="buy-trip-container">
            {error && <p className="error">{error}</p>}
            {responseMessage && <p className="success">{responseMessage}</p>}
            
            <h2>Kup wycieczkę: {trip.name}</h2>
            <p>Data rozpoczęcia: {new Date(trip.startDate).toLocaleString()}</p>
            <p>Data zakończenia: {new Date(trip.endDate).toLocaleString()}</p>
            <p>Dostępne miejsca: {trip.availableSlots}</p>

            <form onSubmit={handleBuy}>
                <div className="form-group">
                    <label htmlFor="numOfParticipants">Liczba uczestników:</label>
                    <input
                        type="number"
                        id="numOfParticipants"
                        name="numOfParticipants"
                        value={numOfParticipants}
                        onChange={(e) => setNumOfParticipants(e.target.value)}
                        min="1"
                        required
                    />
                </div>
                <button type="submit" disabled={loading}>
                    {loading ? "Przetwarzanie..." : "Kup wycieczkę"}
                </button>
            </form>
        </div>
    );
};

export default BuyTrip;
