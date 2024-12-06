import React from 'react';
import { Routes, Route } from 'react-router-dom';
import AboutPage from '../../Pages/AboutPage/AboutPage';
import HomePage from '../../Pages/HomePage/HomePage';
import ActivitiesPage from '../../Pages/ActivitiesPage/ActivitiesPage';
import PersonsPage from '../../Pages/PersonsPage/PersonsPage';
import SignInPage from '../../Pages/SignInPage/SignInPage';

import "./PageRenderer.css";

const PageRenderer = () => {
    return (
        <div className='page'>
            <Routes>
                <Route exact path="/" element={<HomePage /> } />
                <Route exact path="/events" element={<ActivitiesPage /> } />
                <Route exact path="/persons" element={<PersonsPage /> } />
                <Route exact path="/about" element={<AboutPage />} />
                <Route exact path="/signin" element={<SignInPage />} />
                {/* <Route path="*" element={<Navigate to="/" />} /> */}
            </Routes>
        </div>
    );
};

export default PageRenderer;