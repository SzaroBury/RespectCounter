import "./PageRenderer.css";
import React from 'react';
import { Routes, Route } from 'react-router-dom';
import HomePage from '../../Pages/HomePage/HomePage';
import AboutPage from '../../Pages/AboutPage/AboutPage';
import SignUpPage from '../../Pages/SignUpPage/SignUpPage';
import ActivitiesPage from '../../Pages/ActivitiesPage/ActivitiesPage';
import ActivityDetailsPage from '../../Pages/ActivitiesPage/ActivityDetailsPage/ActivityDetailsPage';
import CreateActivityPage from "../../Pages/ActivitiesPage/CreateActivityPage/CreateActivityPage";
import PersonsPage from '../../Pages/PersonsPage/PersonsPage';
import CreatePersonPage from "../../Pages/PersonsPage/CreatePersonPage/CreatePersonPage";
import PersonDetailsPage from "../../Pages/PersonsPage/PersonDetailsPage/PersonDetailsPage";
import NotFoundPage from "../../Pages/NotFoundPage/NotFoundPage";
import AdminPage from "../../Pages/AdminPage/AdminPage";


const PageRenderer = () => {
    return (
        <div className='page'>
            <Routes>
                <Route exact path="/" element={<HomePage /> } />
                <Route exact path="/activities" element={<ActivitiesPage /> } />
                <Route exact path="/act/:id" element={<ActivityDetailsPage />} />
                <Route exact path="/activity/create" element={<CreateActivityPage /> } />
                <Route exact path="/persons" element={<PersonsPage /> } />
                <Route exact path="/person/create" element={<CreatePersonPage />} />
                <Route exact path="/person/:id" element={<PersonDetailsPage />} />
                <Route exact path="/about" element={<AboutPage />} />
                <Route exact path="/signup" element={<SignUpPage />} />
                <Route exact path="/admin" element={<AdminPage />} />
                <Route exact path="*" element={<NotFoundPage />} />
                {/* <Route path="*" element={<Navigate to="/" />} /> */}
            </Routes>
        </div>
    );
};

export default PageRenderer;