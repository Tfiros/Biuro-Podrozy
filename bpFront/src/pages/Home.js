import React from "react";
import "./Home.css";

const Home = () => {
    return (
        <div className="home-page">
            <h1>Witamy na stronie naszego biura!</h1>
            <p>
                Nasze biuro podróży to kameralne miejsce, w którym każdy klient traktowany jest indywidualnie. Specjalizujemy się w organizacji wyjazdów dopasowanych do Twoich potrzeb – od rodzinnych wakacji, przez romantyczne wypady we dwoje, po przygody dla miłośników aktywnego wypoczynku.
                Współpracujemy z zaufanymi partnerami, oferując starannie wyselekcjonowane kierunki w przystępnych cenach. Oferujemy kompleksową obsługę – od doradztwa w wyborze najlepszej oferty, po organizację transportu, zakwaterowania i ubezpieczenia.
                Nasza misja to tworzenie niezapomnianych wspomnień i spełnianie podróżniczych marzeń. Odwiedź nas, a z przyjemnością pomożemy Ci zaplanować idealny wyjazd!
            </p>
            <img
                src="/mainpic.png"
                alt="Travel"
                className="home-image"
            />
        </div>
    );
};

export default Home;