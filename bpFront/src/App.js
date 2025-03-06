import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./AuthContext";
import Navbar from "./compontents/Navbar";
import Home from "./pages/Home";
import Trips from "./pages/Trips";
import Login from "./pages/Login";
import './App.css';
import Register from "./pages/Register";
import AddTrip from "./pages/AddTrip";
import EditTrip from "./pages/EditTrip";
import ScheduleTrip from "./pages/ScheduleTrip";
import TripDetails from "./pages/TripDetails";
import BuyTrip from "./pages/BuyTrip";
import MyOrders from "./pages/MyOrders";
import About from "./pages/About";
import ContactForm from "./pages/ContactForm";

const App = () => {
  return (
      <Router>
          <AuthProvider>
              <Navbar />
              <Routes>
                  <Route path="/" element={<Home />} />
                  <Route path="/login" element={<Login />} />
                  <Route path="/trips" element={<Trips />} />
                  <Route path="/register" element={<Register />} />
                  <Route path="/add-trip" element={<AddTrip />} />
                  <Route path="/edit-trip/:id" element={<EditTrip />} />
                  <Route path="/schedule-trip/:id" element={<ScheduleTrip />} />
                  <Route path="/trip-details/:id" element={<TripDetails />} />
                  <Route path="/buy-trip/:id" element={<BuyTrip />} />
                  <Route path="/my-orders" element={<MyOrders />} />
                  <Route path="/about" element={<About />} />
                  <Route path="/contact-us" element ={<ContactForm/>} />
              </Routes>
          </AuthProvider>
      </Router>
  );
};

export default App;