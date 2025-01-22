import React, { useState } from 'react';
import axios from 'axios';
import "./SignInPage.css";
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../utils/AuthProvider/AuthProvider';

function SignInPage(props) {
    const [formData, setFormData] = useState({ email: '', password: '' });
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmationPassword, setShowConfirmationPassword] = useState(false);
    const navigate = useNavigate();
    const { login } = useAuth();
    
    const handleSignIn = async (e) => {
        try {
            const response = await axios.post('/register', formData);
            handleLogin();            
        } catch (error) {
            console.error('Error:', error);
        }
    };

    const handleLogin = async () => {
        try {
            console.log('SignInPage: sending login request:', formData);
            const response = await axios.post('/login', formData, { params: { useCookies: true } } );
            console.log('SignInPage: success ', response.data);
            login(formData.email);
            navigate("/");
        } catch (error) {
            if (error.response && error.response.status === 401) {
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
            <div className="signin">
                <h2 className='signin-header'>Sign In</h2>
                <div className='signin-form'>

                    <div className="input-group mb-3">
                        <input type="email" class="form-control" placeholder="Email" name="email" required 
                            onChange={handleDataChange}/>                        
                    </div>
                    <div className="input-group mb-3">
                        {
                            showPassword ? (
                                <>
                                    <input type="text" class="form-control" placeholder="Password" name="password" required
                                        onChange={handleDataChange}/>
                                    <button className="btn btn-outline-primary" type="button" id="button-addon2" 
                                        onClick={() => setShowPassword(false)}>
                                        <i class="bi bi-eye-slash"></i>
                                    </button>
                                </>
                            ) : (
                                <>
                                    <input type="password" class="form-control" placeholder="Password" name="password" required
                                        onChange={handleDataChange}/>
                                    <button className="btn btn-outline-primary" type="button" id="button-addon2" 
                                        onClick={() => setShowPassword(true)}>
                                        <i class="bi bi-eye"></i>
                                    </button>
                                </>
                            )
                        }
                    </div>
                    <div className="input-group mb-3">
                        {
                            showConfirmationPassword ? (
                                <>
                                    <input type="text" class="form-control" placeholder="Confirm Password" name="confirm_password" required/>
                                    <button className="btn btn-outline-primary" type="button" id="button-addon2" onClick={() => setShowConfirmationPassword(false)}><i class="bi bi-eye-slash"></i></button>
                                </>
                            ) : (
                                <>
                                    <input type="password" class="form-control" placeholder="Confirm Password" name="confirm_password" required/>
                                    <button className="btn btn-outline-primary" type="button" id="button-addon2" onClick={() => setShowConfirmationPassword(true)}><i class="bi bi-eye"></i></button>
                                </>
                            )
                        }
                    </div>
                    <div className="popup-actions">
                        <button className="btn btn-outline-primary" onClick={handleSignIn}>Sign in</button>
                    </div>
                </div>
            </div>
        </>
    );
}

export default SignInPage;