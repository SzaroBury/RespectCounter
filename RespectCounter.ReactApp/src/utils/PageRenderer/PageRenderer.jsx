import "./PageRenderer.css";
import { Routes, Route } from 'react-router-dom';
import HomePage from '../../pages/HomePage/HomePage';
import AboutPage from '../../pages/AboutPage/AboutPage';
import SignUpPage from '../../pages/SignUpPage/SignUpPage';
import ActivitiesPage from '../../pages/ActivitiesPage/ActivitiesPage';
import ActivityDetailsPage from '../../pages/ActivitiesPage/ActivityDetailsPage/ActivityDetailsPage';
import CreateActivityPage from "../../pages/ActivitiesPage/CreateActivityPage/CreateActivityPage";
import PersonsPage from '../../pages/PersonsPage/PersonsPage';
import CreatePersonPage from "../../pages/PersonsPage/CreatePersonPage/CreatePersonPage";
import PersonDetailsPage from "../../pages/PersonsPage/PersonDetailsPage/PersonDetailsPage";
import NotFoundPage from "../../pages/NotFoundPage/NotFoundPage";
import AdminPage from "../../pages/AdminPage/AdminPage";

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