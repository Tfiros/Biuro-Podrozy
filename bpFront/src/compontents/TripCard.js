import React from "react";
import "./TripCard.css";

const TripCard = ({ trip, userRole, onDelete, onEdit, onGetDetails, onSchedule }) => {

    return (
        <div className="trip-card">
            <img src={trip.imagePath} alt={trip.name} className="trip-image" />
            <div className="trip-details">
                <h3>{trip.name}</h3>
                <p>Country: {trip.country}</p>
                <p>Typ: {trip.tripCategory}</p>
                <p>Price for one participant: {trip.price}</p>
                <p>{trip.description}</p>
            </div>
            <div className="buttons">
                {userRole === "Admin" && (
                    <>
                        <button className="edit-btn" onClick={() => onEdit(trip.tripId)}>Edit</button>
                        <button className="delete-btn" onClick={() => onDelete(trip.tripId)}>Delete</button>
                        <button className="schedule-btn" onClick={() => onSchedule(trip.tripId)}>New Schedule</button>
                    </>
                )}
                    <button className="details-btn" onClick={() => onGetDetails(trip.tripId)}>Details</button>
            </div>
        </div>
    );
};

export default TripCard;
