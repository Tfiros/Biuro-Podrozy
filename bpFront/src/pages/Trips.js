import React, { useState, useEffect } from "react";
import TripCard from "../compontents/TripCard"
import "./Trips.css";
import {useNavigate} from "react-router-dom";

const Trips = () => {
    const [trips, setTrips] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [page, setPage] = useState(1);
    const navigate = useNavigate();
    const [filters, setFilters] = useState({
        category: "",
        country: "Polska",
        priceMin: "",
        priceMax: "",
        startDate: "",
        endDate: "",
    });

    const fetchTrips = async () => {
        setLoading(true);
        setError("");

        const query = new URLSearchParams({
            pageNum: page,
            pageSize: 10,
            country: filters.country || "",
            categoryId: filters.category || "",
            priceMin: filters.priceMin || "",
            priceMax: filters.priceMax || "",
            startDate: filters.startDate || "",
            endDate: filters.endDate || "",
        });

        try {
            const response = await fetch(`http://localhost:5029/api/Trips?${query.toString()}`);
            if (!response.ok) {
                throw new Error("Błąd podczas pobierania danych");
            }
            const resJson = await response.json();
            setTrips(resJson.trips || []);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchTrips();
    }, [page, filters]);

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleResetFilters = () => {
        setFilters({
            category: "",
            country: "Polska",
            priceMin: "",
            priceMax: "",
            startDate: "",
            endDate: "",
        });
        setPage(1);
    };

    const handleAddTrip = () => {
        navigate("/add-trip");
    };

    const handleDelete = (id) => {
        fetch(`http://localhost:5029/api/Trips/${id}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${sessionStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.ok) {
                setTrips(trips.filter((trip) => trip.id !== id));
            }
        });
    };

    const handleEdit = (id) => {
        navigate(`/edit-trip/${id}`);
    };

    const handleSchedule = (id) => {
        navigate(`/schedule-trip/${id}`);
    }

    const handleGetDetails = (id) => {
       navigate(`/trip-details/${id}`);
    };

    return (
        <div className="trips-container">
            <div className="filters">
                <h2>Filtry</h2>
                <select
                    name="category"
                    value={filters.category}
                    onChange={handleFilterChange}
                >
                    <option value="">Wybierz kategorię</option>
                    <option value="1">Jednodniowa</option>
                    <option value="2">Objazdowa</option>
                    <option value="3">Pobyt stały</option>
                </select>
                <select
                    name="country"
                    value={filters.country}
                    onChange={handleFilterChange}
                >
                    <option value="Polska">Polska</option>
                </select>
                <input
                    type="number"
                    name="priceMin"
                    placeholder="Cena minimalna"
                    value={filters.priceMin}
                    onChange={handleFilterChange}
                />
                <input
                    type="number"
                    name="priceMax"
                    placeholder="Cena maksymalna"
                    value={filters.priceMax}
                    onChange={handleFilterChange}
                />
                <input
                    type="date"
                    name="startDate"
                    value={filters.startDate}
                    onChange={handleFilterChange}
                />
                <input
                    type="date"
                    name="endDate"
                    value={filters.endDate}
                    onChange={handleFilterChange}
                />
                <button onClick={handleResetFilters}>Resetuj filtry</button>
            </div>

            {sessionStorage.getItem("role") === "Admin" && (
                <button onClick={handleAddTrip} className="add-trip-btn">
                    Add New Trip
                </button>
            )}

            {loading && <p>Ładowanie...</p>}
            {error && <p className="error">{error}</p>}

            <div className="trips-list">
                {trips.length > 0 ? (
                    trips.map((trip) => (
                        <TripCard
                            key={trip.tripId}
                            trip={trip}
                            userRole={sessionStorage.getItem("role")}
                            onDelete={handleDelete}
                            onEdit={handleEdit}
                            onGetDetails={handleGetDetails}
                            onSchedule={handleSchedule}
                        />
                    ))
                ) : (
                    <p>Brak wycieczek do wyświetlenia</p>
                )}
            </div>

            <div className="pagination">
                <button
                    onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
                    disabled={page === 1}
                >
                    Poprzednia
                </button>
                <span>Strona {page}</span>
                <button
                    onClick={() => setPage((prev) => prev + 1)}
                    disabled={trips.length < 10}
                >
                    Następna
                </button>
            </div>
        </div>
    );
};

export default Trips;
