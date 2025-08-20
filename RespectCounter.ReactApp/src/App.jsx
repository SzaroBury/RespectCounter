import './App.css'
import { BrowserRouter as Router} from 'react-router-dom';
import { AuthProvider } from './utils/providers/AuthProvider';
import { NotificationProvider } from './utils/providers/NotificationProvider/NotificationProvider';
import Header from './components/Header/Header';
import PageRenderer from './utils/pageRenderer/PageRenderer';
import Footer from './components/Footer/Footer';

function App(props) {

    return ( 
        <div className='app'>
            <Router>
                <AuthProvider>
                    <NotificationProvider>
                        <Header />
                        <PageRenderer/>
                        <Footer/>
                    </NotificationProvider>
                </AuthProvider>
            </Router>
        </div>
    );
};

export default App;