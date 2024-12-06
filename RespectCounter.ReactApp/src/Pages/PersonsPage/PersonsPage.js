import React from 'react';
import Container from 'react-bootstrap/Container'
import SortMenu from '../../components/SortMenu/SortMenu';

class PersonsPage extends React.Component {
    static displayName = "RespectCounter - Persons";

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    render() {
        return (
            <Container>
                <SortMenu page="Persons"/>
                <h2>Tag name</h2>
            </Container>
        );
    }
}

export default PersonsPage;

