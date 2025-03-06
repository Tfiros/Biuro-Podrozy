import React, { useState, useEffect } from "react";
import { useAuth } from '../AuthContext';
import { useNavigate } from "react-router-dom";
import './MyOrders.css'
const MyOrders = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const { logoutUser } = useAuth();
    const navigate = useNavigate();

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
                const resData = await response.json();
                sessionStorage.setItem("accessToken", resData.token);
                sessionStorage.setItem("refreshToken", resData.refreshToken);
                return resData.token;
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
        const fetchOrders = async () => {
            try {
                const token = sessionStorage.getItem("accessToken");
                if (!token) {
                    setError("Brak tokenu dostępu. Zaloguj się ponownie.");
                    setLoading(false);
                    return;
                }

                const response = await fetch("http://localhost:5029/api/Orders", {
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`,
                    },
                });
                if (response.status === 401) {
                    const accessToken = await getNewAccessToken();
                    if (accessToken) {
                        navigate('/my-orders');
                    } else {
                        setError("Sesja wygasła. Proszę się zalogować ponownie.");
                        logoutUser();
                        return;
                    }
                }
                if (!response.ok) {
                    throw new Error(`Błąd ${response.status}: ${response.statusText}`);
                }

                const data = await response.json();
                setOrders(data);
            } catch (err) {
                setError(err.message || "Błąd podczas pobierania zamówień.");
            } finally {
                setLoading(false);
            }
        };

        fetchOrders();
    }, []);

    if (loading) return <p>Ładowanie zamówień...</p>;
    if (error) return <p className="error">{error}</p>;

    return (
        <div className="my-orders-container">
            <h2>Moje zamówienia</h2>
            {orders.length === 0 ? (
                <p>Nie masz jeszcze żadnych zamówień.</p>
            ) : (
                <ul>
                    {orders.map((order) => (
                        <li key={order.tripId}>
                            <h3>{order.name}</h3>
                            <img src={order.imagePath} alt={order.name} className="trip-image"/>
                            <p>{order.country}</p>
                            <p>{order.tripCategory}</p>
                            <p>{new Date(order.startDate).toLocaleDateString()} - {new Date(order.endDate).toLocaleDateString()}</p>
                            <p>{order.price} PLN</p>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default MyOrders;
