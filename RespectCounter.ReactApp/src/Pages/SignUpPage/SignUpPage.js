import React, { useState } from 'react';
import axios from 'axios';
import "./SignUpPage.css";
import Loading from '../../components/Loading/Loading';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../utils/AuthProvider/AuthProvider';

function SignUpPage(props) {
    const [formData, setFormData] = useState({ username: '', email: '', password: '' });
    const [confirmationPassword, setConfirmationPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmationPassword, setShowConfirmationPassword] = useState(false);
    const navigate = useNavigate();
    const { login } = useAuth();
    const [loading, setLoading] = useState(false);
    const [validationMessage, setValidationMessage] = useState();
    
    const handleSignUp = async (e) => {
        e.preventDefault();
        setValidationMessage("");
        setLoading(true);
        if(formData.password !== confirmationPassword)
        {
            setLoading(false);
            setValidationMessage("Passwords don't match.");
        }
        try {
            console.log('SignUpPage: sending sign up request:', formData);
            await axios.post('api/auth/register', formData);
            handleLogin();            
        } catch (error) {
            setLoading(false);
            console.warn(error);
            if (error.response && error.response.status === 400) {
                console.warn(`${error.response.data.title} - ${error.response.data.message}`);
                setValidationMessage(error.response.data.message);
            } else {
                console.error('An error occurred:', error.response.message);
            }
        } 
    };

    const handleLogin = async () => {
        try {
            console.log('SignUpPage: sending login request:', formData);
            const response = await axios.post('api/auth/login', { username: formData.username, password: formData.password });
            console.log('SignUpPage: success ', response.data);
            login(formData.username);
            setLoading(false);
            navigate("/");
        } catch (error) {
            setLoading(false);
            if (error.response && error.status === 40) {
                console.warn('Unauthorized! Redirecting to login...');
            } else {
                console.error('An error occurred:', error);
            }
        }        
    }

    const handleDataChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    return (
        <>
            <div className="signup">
                <h2 className='signup-header'>Sign Up</h2>
                <form className='signup-form needs-validation' onSubmit={handleSignUp}>
                    <div className="input-group has-validation mb-3">
                        <input type="text" className="form-control has-validation" placeholder="Username" name="username" required minlength="3" id="validationCustomUsername"
                            onChange={handleDataChange}
                        />   
                        <div class="invalid-feedback">
                            Please choose a username.
                        </div>                    
                    </div>
                    <div className="input-group mb-3">
                        <input type="email" className="form-control" placeholder="Email" name="email" required 
                            onChange={handleDataChange}/>                        
                    </div>
                    <div className="input-group mb-3">
                        <input 
                            type={showPassword ? "text" : "password"} 
                            className="form-control" 
                            placeholder="Password" 
                            name="password" 
                            required
                            onChange={handleDataChange}/>
                        <button 
                            className="btn btn-outline-primary" 
                            type="button" 
                            onClick={() => setShowPassword(!showPassword)}
                        >
                            <i className={showPassword ? "bi bi-eye-slash" : "bi bi-eye"}></i>
                        </button>
                    </div>
                    <div className="input-group mb-3">
                        <input 
                            type={showConfirmationPassword ? "text" : "password"} 
                            className="form-control" 
                            placeholder="Confirm Password" 
                            name="confirm_password" 
                            required
                            value={confirmationPassword}
                            onChange={(e) => setConfirmationPassword(e.target.value)}
                        />
                        <button 
                            className="btn btn-outline-primary" 
                            type="button"
                            onClick={() => setShowConfirmationPassword(!showConfirmationPassword)}
                        >
                            <i className={showConfirmationPassword ? "bi bi-eye-slash" : "bi bi-eye"}></i>
                        </button>
                    </div>
                    <div className="popup-actions">
                        <button className="btn btn-outline-primary" type="submit">Sign up</button>
                    </div>
                    <div>
                        <Loading loading={loading}></Loading>
                        <p className='text-danger text-center'>{validationMessage}</p>
                    </div>
                </form>
            </div>
        </>
    );
}

export default SignUpPage;