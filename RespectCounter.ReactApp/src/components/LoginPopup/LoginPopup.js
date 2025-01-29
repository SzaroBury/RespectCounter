import "./LoginPopup.css";
import { useState } from "react";
import axios from 'axios';
import { useAuth } from "../../utils/AuthProvider/AuthProvider";
import { useNavigate } from "react-router-dom";

function LoginPopup() {
    const [formData, setFormData] = useState({username: '', password: ''});
    const [showPassword, setShowPassword] = useState(false);
    const [validationMessage, setValidationMessage] = useState("");
    const {login, closeLoginPopup} = useAuth();
    const navigate = useNavigate();

    const handleDataChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleLogin = async (e) => {
        e.preventDefault();
        axios.defaults.withCredentials = true;
        try {
            console.log('LoginPopup: sending login request:', formData);
            setValidationMessage("");
            const response = await axios.post('api/auth/login', formData );
            console.log('LoginPopup: success ', response.data);
            login( formData.username );
            closeLoginPopup();
        } catch (error) {
            if (error.response && error.response.status === 401) {
                console.warn('Unauthorized! Redirecting to login...');
                setValidationMessage(`Failed to login. ${error.response.data.message} Please try again.`);
            } else {
                console.error('An error occurred:', error);
            }
        }        
    };

    const handleSignUpClick = () => {
        navigate("/signup");
        closeLoginPopup();
    }

    return (
        <div className="popup-backdrop">
            <div className="popup-container border">
                <h2>Log In</h2>
                <div className="popup-cancel">
                    <button type="button" className="btn btn-outline-primary" onClick={closeLoginPopup}><i className="bi bi-x-lg"></i></button>
                </div>
                <form onSubmit={handleLogin}>
                    <div className="input-group mb-3">
                        <input type="text" className="form-control" placeholder="Username" name="username" required
                            onChange={handleDataChange}/>                        
                    </div>
                    <div className="input-group mb-3">
                        <input 
                            type={showPassword ? "text" : "password"} 
                            className="form-control" 
                            placeholder="Password" 
                            name="password" 
                            required 
                            onChange={handleDataChange} 
                        />
                        <button 
                            className="btn btn-outline-primary" 
                            type="button" 
                            id="button-addon2" 
                            onClick={() => setShowPassword((prev) => !prev)}
                        >
                            <i className={showPassword ? "bi bi-eye-slash" : "bi bi-eye"}></i>
                        </button>
                    </div>
                    <div className="popup-actions">
                        <button type="submit" className="btn btn-outline-primary">Log In</button>
                    </div>
                    <div className="text-center">
                        <span className="text-danger">{validationMessage}</span>
                    </div>
                </form>
                <div className="popup-signup">
                    <button className="btn me-2" onClick={handleSignUpClick}>
                        Sign up
                    </button>
                    <label className="popup-signup-label">if you don't have an account yet.</label>
                </div>
                
            </div>
        </div>
    );
}

export default LoginPopup;
