import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth } from '../AuthContext';
import './ScheduleTrip.css'
const ScheduleTrip = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { logoutUser } = useAuth();
    const [tripData, setTripData] = useState({});
    const [schedule, setSchedule] = useState({
        startDate: "",
        endDate: "",
        placesAvailable: "",
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        fetch(`http://localhost:5029/api/Trips/${id}`)
            .then((response) => response.json())
            .then((data) => {
                setTripData(data);
                setLoading(false);
            })
            .catch(() => {
                setError("Błąd podczas ładowania danych wycieczki.");
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
    const handleChange = (e) => {
        const { name, value } = e.target;
        setSchedule((prevSchedule) => ({
            ...prevSchedule,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        if (!schedule.startDate || !schedule.endDate || !schedule.placesAvailable) {
            setError("Wszystkie pola są wymagane!");
            setLoading(false);
            return;
        }

        const newSchedule = {
            tripId: id,
            startDate: schedule.startDate,
            endDate: schedule.endDate,
            placesAvailable: schedule.placesAvailable,
        };

        try {
            let accessToken = sessionStorage.getItem("accessToken");
            let response = await fetch(`http://localhost:5029/api/TripSchedule`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${accessToken}`,
                },
                body: JSON.stringify(newSchedule),
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

            if (response.ok) {
                setLoading(false);
                navigate(`/trips`);
            } else {
                setError("Błąd podczas zapisywania terminu.");
                setLoading(false);
            }
        } catch (err) {
            setError("Błąd podczas zapisywania terminu.");
            setLoading(false);
        }
    };

    if (loading) return <p>Ładowanie...</p>;

    return (
        <div className="schedule-trip-container">
            <h2>Dodaj termin dla wycieczki: {tripData.name}</h2>
            {error && <p className="error">{error}</p>}

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="startDate">Data rozpoczęcia</label>
                    <input
                        type="datetime-local"
                        id="startDate"
                        name="startDate"
                        value={schedule.startDate}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="endDate">Data zakończenia</label>
                    <input
                        type="datetime-local"
                        id="endDate"
                        name="endDate"
                        value={schedule.endDate}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="placesAvailable">Dostępne miejsca</label>
                    <input
                        type="number"
                        id="placesAvailable"
                        name="placesAvailable"
                        value={schedule.placesAvailable}
                        onChange={handleChange}
                        required
                    />
                </div>

                <button type="submit" disabled={loading}>
                    {loading ? "Zapisuję..." : "Zapisz termin"}
                </button>
            </form>
        </div>
    );
};

export default ScheduleTrip;
