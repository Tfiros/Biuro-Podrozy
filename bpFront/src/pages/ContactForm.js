import React, { useState } from "react";
import "./ContactForm.css";

const ContactForm = () => {
    const [formData, setFormData] = useState({
        name: "",
        email: "",
        message: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        alert("Dziękujemy za wiadomość!");
        setFormData({ name: "", email: "", message: "" });
    };

    return (
        <div className="contact-form-container">
            <h2>Skontaktuj się z nami</h2>
            <form onSubmit={handleSubmit} className="contact-form">
                <div className="form-group">
                    <label htmlFor="name">Imię i nazwisko</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        placeholder="Wpisz swoje imię i nazwisko"
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        placeholder="Wpisz swój email"
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="message">Wiadomość</label>
                    <textarea
                        id="message"
                        name="message"
                        value={formData.message}
                        onChange={handleChange}
                        placeholder="Wpisz swoją wiadomość"
                        rows="5"
                        required
                    ></textarea>
                </div>
                <button type="submit" className="submit-btn">Wyślij</button>
            </form>
        </div>
    );
};

export default ContactForm;
