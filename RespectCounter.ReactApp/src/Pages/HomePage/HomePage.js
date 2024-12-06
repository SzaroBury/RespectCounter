import React from 'react';
import "./HomePage.css";


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
                    <p className="text-center">See what it is <a href="/about">about</a>.</p>

                    <h2>Trending Events</h2>
                    <a href="/events">See more</a>

                    <br/>
                    
                    <h2>Trending Persons</h2>
                    <a href='/persons'>See more</a>

                </div>
            </>
        );
    }
}

export default HomePage;