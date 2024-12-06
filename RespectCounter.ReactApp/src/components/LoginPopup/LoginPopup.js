import { useState } from "react";
import axios from 'axios';
import "./LoginPopup.css";

function LoginPopup({onCancel, onSuccessfullLoggingIn}) {
    const [formData, setFormData] = useState({email: '', password: ''});
    const [showPassword, setShowPassword] = useState(false);

    const handleDataChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleLogin = async (e) => {
        e.preventDefault();
        // Obsługa logowania (np. wyślij dane do API)

        try {
            console.log('LoginPopup: sending login request:', formData);
            const response = await axios.post('/login', formData, { params: { useCookies: true } } );
            console.log('LoginPopup: success ', response.data);
            onSuccessfullLoggingIn(formData.email)
        } catch (error) {
            console.error('Error:', error);
        }

        console.log("Login submitted");
        onCancel(); // Zamknij popup po zalogowaniu
    };

    return (
        <div className="popup-backdrop">
            <div className="popup-container">
                <h2>Log In</h2>
                <div className="popup-cancel">
                    <button type="button" className="btn btn-secondary" onClick={onCancel}><i class="bi bi-x-lg"></i></button>
                </div>
                <form onSubmit={handleLogin}>
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
                                    <button className="btn btn-secondary" type="button" id="button-addon2" onClick={() => setShowPassword(false)}><i class="bi bi-eye-slash"></i></button>
                                </>
                            ) : (
                                <>
                                    <input type="password" class="form-control" placeholder="Password" name="password" required
                                        onChange={handleDataChange}/>
                                    <button className="btn btn-secondary" type="button" id="button-addon2" onClick={() => setShowPassword(true)}><i class="bi bi-eye"></i></button>
                                </>
                            )
                        }
                    </div>
                    <div className="popup-actions">
                        <button type="submit" className="btn btn-secondary">Log In</button>
                    </div>
                </form>
                <label className="popup-signin-label">If you don't have an account yet</label>
                
                <div className="popup-signin">
                    <a href="/signin">
                        <button className="btn btn-secondary" onClick={() => onCancel()}>Sign in</button>
                    </a>
                </div>
            </div>
        </div>
    );
}

export default LoginPopup;
