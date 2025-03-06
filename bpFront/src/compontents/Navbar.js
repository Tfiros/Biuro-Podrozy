import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../AuthContext';
import './Navbar.css';

const Navbar = () => {
    const { login, logout } = useAuth();
    const [displayLogin, setDisplayLogin] = useState(login);

    useEffect(() => {
        setDisplayLogin(login);
    }, [login]);

    const handleLogout = () => {
        logout();
    };

    return (
        <nav className="navbar">
            <div className="nav-links">
                <Link to="/">Strona główna</Link>
                <Link to="/trips">Oferta wycieczek</Link>
                <Link to="/about">O nas</Link>
                <Link to="/contact-us">Kontakt</Link>
                {displayLogin && <Link to="/my-orders">Moje zamówienia</Link>}
            </div>
            <div className="logged-as">
                <span>Zalogowany jako: {displayLogin || 'gość'}</span>
            </div>
            <div className="login-icon">
                {!displayLogin ? (
                    <Link to="/login">
                        <img src="/login-icon.png" alt="Login" />
                    </Link>
                ) : (
                    <button onClick={handleLogout}>Wyloguj</button>
                )}
            </div>
        </nav>
    );
};

export default Navbar;
