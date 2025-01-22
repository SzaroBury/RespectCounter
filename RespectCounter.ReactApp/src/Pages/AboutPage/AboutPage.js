import React from 'react';
import Container from 'react-bootstrap/Container'
import "./AboutPage.css";

class AboutPage extends React.Component {
    static displayName = AboutPage.name;

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    render() {
        return (
            <Container className="about-page">
                <h1 className="text-center">Welcome To Respect Counter</h1>
                <p className="m-5 text-center">.NET + React application that allows users to express their opinion on various public figures, their quotes and actions.</p>

                <div>
                    <h5>Target main functionalities:</h5>
                    <ul>
                        <li>Users can propose public figures.</li>
                        <li>Users can add quotes/activities and link them to stored public figures.</li>
                        <li>Users can comment and react to persons, activities and other comments.</li>
                        <li>Users can search quotes, activities and persons by tags.</li>
                        <li>Users can report activities, comments.</li>
                        <li>Moderators can verify public figures, quotes and activities added by users.</li>
                        <li>Moderators can create verified public figures, quotes and activities.</li>
                        <li>Moderators can hide comments and activities.</li>
                    </ul>
                </div>
                <h5>To-do list: </h5>
                    <ul>
                        <li>User:
                            <ul>
                                <li>Singing up</li>
                                <li>Singing in</li>
                            </ul>
                        </li>
                        <li>Events:
                            <ul>
                                <li>List of events:
                                    <ul>
                                        <li>Get list from API - DONE</li>
                                        <li>Sort list - DONE</li>
                                        <li>Change scope of list - DONE</li>
                                        <li>Tags:
                                            <ul>
                                                <li>Get list of tags - DONE</li>
                                                <li>Get event with the tag</li>
                                            </ul>
                                        </li>
                                    </ul>
                                </li>
                                <li>Detailed view</li>
                                <li>Rating</li>
                            </ul>
                        </li>
                        <li>Persons:
                            <ul>
                                <li>List of the persons</li>
                                <li>Detailed view</li>
                                <li>Images</li>
                            </ul>
                        </li>
                        <li>Home page:
                            <ul>

                            </ul>
                        </li>
                    </ul>
            </Container>
        );
    }
}

export default AboutPage;