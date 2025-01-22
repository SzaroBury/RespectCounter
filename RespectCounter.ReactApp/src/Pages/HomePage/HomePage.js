import React from 'react';
import "./HomePage.css";
import { Link } from 'react-router-dom';


class HomePage extends React.Component {
    static displayName = HomePage.name;

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    render() {
        return (
            <>
                <div className='home-page'>
                    <h1 className="text-center">Welcome To Respect Counter</h1>
                    <p className="text-center">See what it is <Link to="/about">about</Link>.</p>

                    <h2>Trending Activities</h2>
                    <Link to="/activities">See more</Link>

                    <br/>
                    
                    <h2 className='mt-5'>Best Persons</h2>
                    <Link href='/persons'>See more</Link>

                </div>
            </>
        );
    }
}

export default HomePage;