import { useState } from 'react';
import "./Header.css"

function Header({onLoginClick, onLogoutClick, logged, user}) {

    return (
        <div className="header">
            <a href="/" className="d-flex align-items-center mb-2 mb-lg-0 text-dark text-decoration-none">
                {/*<svg className="bi me-2" width="40" height="32" role="img" aria-label="Bootstrap"><use xlink:href="#bootstrap"></use></svg>*/}
                <span className="fs-4">Respect Counter</span>
            </a>

            <ul className="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                <li><a href="/events" className="nav-link link-dark">Events</a></li>
                <li><a href="/persons" className="nav-link link-dark">Persons</a></li>
                <li><a href="/about" className="nav-link link-dark">About</a></li>
            </ul>

            {/* <form className="col-12 col-lg-auto mb-3 mb-lg-0 me-lg-3">
            <input type="search" className="form-control" placeholder="Search..." aria-label="Search"/ >
            </form> */}

            <div className="d-flex align-items-center gap-3">
                <span><i className="bi bi-moon-stars-fill"></i></span>
                {/* <div className="dropdown text-end">
                    <a className="d-block link-dark text-decoration-none dropdown-toggle" id="dropdownUser1" data-bs-toggle="dropdown" aria-expanded="false">
                        <img src="https://github.com/mdo.png" alt="mdo" width="32" height="32" className="rounded-circle"/>
                    </a>
                    <ul className="dropdown-menu text-small" aria-labelledby="dropdownUser1">
                        <li><a className="dropdown-item" href="/#">New project...</a></li>
                        <li><a className="dropdown-item" href="/#">Settings</a></li>
                        <li><a className="dropdown-item" href="/#">Profile</a></li>
                        <li><hr className="dropdown-divider"/></li>
                        <li><a className="dropdown-item" href="/#">Sign out</a></li>
                    </ul>
                </div> */}

                {
                    logged ? (
                        <div>
                            <span className='m-3'>Hi {user}!</span>
                            <button className='btn btn-secondary' onClick={onLogoutClick}>Log out</button>    
                        </div>  
                        
                    ) : (
                        <div>
                            <button className='btn btn-secondary' onClick={onLoginClick}>Log in</button>
                        </div>
                    )
                }
            </div>
            
        </div>
    );
};

export default Header;





