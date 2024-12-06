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
                <p className="m-5 text-justify">&#9;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque rhoncus eros et nibh tristique, ac cursus libero sagittis. Morbi iaculis sapien luctus lectus viverra hendrerit. Sed fermentum viverra mauris eu ullamcorper. Quisque vestibulum massa eros, ac interdum lectus tristique vel. Sed sem est, condimentum eget scelerisque vitae, aliquam nec sem. Aliquam finibus iaculis dignissim. Suspendisse sagittis ligula at viverra cursus. Etiam molestie, dui nec feugiat porttitor, ipsum libero accumsan justo, id dictum ipsum ligula ut mauris. Praesent lacus libero, cursus dignissim cursus porta, volutpat sit amet odio. Nam interdum bibendum dolor nec facilisis. Morbi ut tortor sed massa elementum semper non ultrices ante. Duis faucibus nunc at sapien maximus vestibulum</p>

                <p>To-do list: </p>
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