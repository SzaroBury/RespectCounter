import "./HomePage.css";
import { Link } from 'react-router-dom';
import Loading from '../../components/Loading/Loading';
import { useState, useEffect } from 'react';
import axios from 'axios';
import Person from '../PersonsPage/Person/Person';

function HomePage() {
    const displayName = HomePage.name;
    const [loadingPersons, setLoadingPersons] = useState(true);
    const [persons, setPersons] = useState([]);
    const [loadingActivities, setLoadingActivities] = useState(true);
    const [activities, setActivities] = useState([]);

    const loadPersons = () => {
        console.log('HomePage: loadPersons()')
        setPersons([]);

        axios.get(`/api/persons`, {
            params: {
                order: "MostRespected"
            }
        })
        .then(response => {
            setPersons(response.data);
            setLoadingPersons(false);
        })
        .catch(error => {
            setLoadingPersons(false);

            if (error.response) {
                console.error(`HTTP error! Status: ${error.response.status}`);
            } else if (error.request) {
                console.error("No response received: ", error.request);
            } else {
                console.error("Error setting up the request: ", error.message);
            }
        });
    }

    useEffect(() => {
        setLoadingPersons(true);
        loadPersons();
    }, []);

    return (
        <>
            <div className='home-page'>
                <h1 className="text-center">Welcome To Respect Counter</h1>
                <p className="text-center">See what it is <Link to="/about">about</Link>.</p>

                <div className="home-sections">
                    <div className="home-section">
                        <h2>Trending Activities</h2>
                        <Loading loading={loadingActivities} />
                        {/* {activities.map((act, index) => 
                            <Person key={"Person_" + per.id} person={per} index={index}/>
                        )} */}
                        <div className="text-center">
                            <Link to="/activities">See more</Link>
                        </div>
                    </div>
                    <div className="home-section">
                        <h2 className="text-center">Most respected persons:</h2>
                        <Loading loading={loadingPersons} />
                        {persons.map((per, index) =>
                            <Person key={"Person_" + per.id} person={per} index={index} showReactionButtons={false} />
                        )}
                        <div className="text-center">
                            <Link to="/persons">See more</Link>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default HomePage;