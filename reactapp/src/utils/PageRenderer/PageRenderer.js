import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Container from 'react-bootstrap/Container';
import WeatherPage from '../../Pages/WeatherPage/WeatherPage';
import HomePage from '../../Pages/HomePage/HomePage';

const PageRenderer = () => {
    return (
        <Container className="p-5 bg-light rounded-3">
            <Routes>
                <Route exact path="/" element={<HomePage /> } />
                <Route path="/weather" element={<WeatherPage />} />
                {/*<Route path="*" element={<Navigate to="/" />} />*/}
            </Routes>
        </Container>
    );
};

export default PageRenderer;