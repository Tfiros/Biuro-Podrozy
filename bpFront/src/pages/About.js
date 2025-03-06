import React from "react";
import "./About.css";

const About = () => {
    return (
        <div className="about-container">
            <h1 className="about-title">O nas</h1>
            <p className="about-text">
                Witaj w naszym biurze podróży! Jesteśmy zespołem pasjonatów, dla których podróże to nie tylko praca,
                ale przede wszystkim sposób na życie. Naszym celem jest pomaganie w odkrywaniu najpiękniejszych zakątków
                świata i spełnianiu podróżniczych marzeń naszych klientów.
            </p>
            <p className="about-text">
                Każda oferta, którą tworzymy, jest wynikiem staranności, doświadczenia i zaangażowania. Wierzymy, że każda
                podróż to wyjątkowa przygoda, dlatego stawiamy na indywidualne podejście, elastyczność i najwyższą jakość obsługi.
            </p>
            <p className="about-text">
                Bez względu na to, czy marzysz o relaksie na słonecznej plaży, zwiedzaniu zabytkowych miast, czy aktywnym
                wypoczynku w górach – znajdziemy coś idealnego dla Ciebie. Zapraszamy do współpracy i wspólnego tworzenia
                niezapomnianych wspomnień!
            </p>
        </div>
    );
};

export default About;
