import { BrowserRouter as Router} from 'react-router-dom';
import Container from 'react-bootstrap/Container'
import PageRenderer from './utils/PageRenderer/PageRenderer';
import Header from './components/Header/Header';

function App(props) {
    return ( 
        <Container className="p-3">
            <Header/>
            <Router>
                <PageRenderer/>
            </Router>
        </Container>
    );
};

export default App;
