import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./TripDetails.css";
const TripDetails = () => {
    const { id } = useParams();
    const navigate = useNavigate();

    const [trip, setTrip] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [userRole, setUserRole] = useState("");

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

        const role = sessionStorage.getItem("role");
        if (role) {
            setUserRole(role);
        }
    }, [id]);

    const handleBuy = () => {
        navigate(`/buy-trip/${id}`);
    };

    if (loading) return <p>Ładowanie...</p>;
    if (error) return <p className="error">{error}</p>;

    return (
        <div className="trip-details-container">
            <h2>Szczegóły wycieczki: {trip.name}</h2>
            <img src={trip.imagePath} alt={trip.name} className="trip-image"/>
            <p>Opis: {trip.description}</p>
            <p>Kategoria: {trip.tripCategory}</p>
            <p>Kraj: {trip.country}</p>
            <p>Cena: {trip.price} PLN</p>
            <p>Data rozpoczęcia: {new Date(trip.startDate).toLocaleString()}</p>
            <p>Data zakończenia: {new Date(trip.endDate).toLocaleString()}</p>
            <p>Dostępne miejsca: {trip.availableSlots}</p>

            {trip.availableSlots > 0 ? (
                (userRole === "Normal_user" || userRole === "Admin") && (
                    <button onClick={handleBuy}>Kup wycieczkę</button>
                )
            ) : (
                <p>Brak dostępnych miejsc.</p>
            )}
        </div>
    );
};

export default TripDetails;
