import React from 'react';
import Container from 'react-bootstrap/Container'

class HomePage extends React.Component {
    static displayName = HomePage.name;

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    render() {
        return (
            <Container>
                <h1 className="header text-center">Welcome To Public Figures</h1>
                <p className="m-5 text-justify">&#9;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque rhoncus eros et nibh tristique, ac cursus libero sagittis. Morbi iaculis sapien luctus lectus viverra hendrerit. Sed fermentum viverra mauris eu ullamcorper. Quisque vestibulum massa eros, ac interdum lectus tristique vel. Sed sem est, condimentum eget scelerisque vitae, aliquam nec sem. Aliquam finibus iaculis dignissim. Suspendisse sagittis ligula at viverra cursus. Etiam molestie, dui nec feugiat porttitor, ipsum libero accumsan justo, id dictum ipsum ligula ut mauris. Praesent lacus libero, cursus dignissim cursus porta, volutpat sit amet odio. Nam interdum bibendum dolor nec facilisis. Morbi ut tortor sed massa elementum semper non ultrices ante. Duis faucibus nunc at sapien maximus vestibulum</p>
            </Container>
        );
    }
}

export default HomePage;

