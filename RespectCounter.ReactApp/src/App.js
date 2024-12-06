import { BrowserRouter as Router} from 'react-router-dom';
import { useState } from 'react';
import PageRenderer from './utils/PageRenderer/PageRenderer';
import Header from './components/Header/Header';
import LoginPopup from './components/LoginPopup/LoginPopup';
import './App.css'

function App(props) {
    const [showLoginPopup, setShowLoginPopup] = useState(false);
    const [userLogged, setUserLogged] = useState(false);
    const [userName, setUserName] = useState("");
    const handleOpenLoginPopup = () => setShowLoginPopup(true);
    const handleCloseLoginPopup = () => setShowLoginPopup(false);
    const handleUserLoggedIn = (username) => { 
        setUserLogged(true);
        setUserName(username);
        console.log(`App: User logged in: ${username}`); 
    }
    const handleUserLoggedOut = () => { 
        setUserLogged(false);
        setUserName("");
        console.log("App: User logged out"); 
    }

    return ( 
        <div className='app'>
            <Header onLoginClick={handleOpenLoginPopup} onLogoutClick={handleUserLoggedOut} logged={userLogged} user={userName}/>
            <Router>
                <PageRenderer/>
            </Router>
            <div className='footer'>
                <div>Made by Mateusz Kubicki</div>
            </div>
            {showLoginPopup && <LoginPopup onCancel={handleCloseLoginPopup} onSuccessfullLoggingIn={handleUserLoggedIn}/>}
        </div>
    );
};

export default App;
