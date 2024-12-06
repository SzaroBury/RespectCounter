import React, { useState } from 'react';
import axios from 'axios';
import "./SignInPage.css";

function SignInPage(props) {
    const [formData, setFormData] = useState({email: '', password: ''});
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmationPassword, setShowConfirmationPassword] = useState(false);
    const handleSignIn = async (e) => {
        e.preventDefault(); // Zapobiega domyślnemu odświeżeniu strony

        try {
            const response = await axios.post('/register', formData);
            console.log('Success:', response.data);
        } catch (error) {
            console.error('Error:', error);
        }
    };

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
                <h2>Sign In</h2>
                <form onSubmit={handleSignIn}>
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
                                    <button className="btn btn-secondary" type="button" id="button-addon2" 
                                        onClick={() => setShowPassword(false)}>
                                        <i class="bi bi-eye-slash"></i>
                                    </button>
                                </>
                            ) : (
                                <>
                                    <input type="password" class="form-control" placeholder="Password" name="password" required
                                        onChange={handleDataChange}/>
                                    <button className="btn btn-secondary" type="button" id="button-addon2" 
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
                                    <button className="btn btn-secondary" type="button" id="button-addon2" onClick={() => setShowConfirmationPassword(false)}><i class="bi bi-eye-slash"></i></button>
                                </>
                            ) : (
                                <>
                                    <input type="password" class="form-control" placeholder="Confirm Password" name="confirm_password" required/>
                                    <button className="btn btn-secondary" type="button" id="button-addon2" onClick={() => setShowConfirmationPassword(true)}><i class="bi bi-eye"></i></button>
                                </>
                            )
                        }
                    </div>
                    <div className="popup-actions">
                    <button className="btn btn-secondary" type="submit">Sign in</button>
                    </div>
                </form>
            </div>
        </>
    );
}

export default SignInPage;