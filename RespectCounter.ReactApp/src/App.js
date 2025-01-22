import './App.css'
import { BrowserRouter as Router} from 'react-router-dom';
import PageRenderer from './utils/PageRenderer/PageRenderer';
import { AuthProvider } from './utils/AuthProvider/AuthProvider';
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';

function App(props) {

    return ( 
        <div className='app'>
            <Router>
                <AuthProvider>
                    <Header />
                    <PageRenderer/>
                    <Footer/>
                </AuthProvider>
            </Router>
        </div>
    
    );
};

export default App;
