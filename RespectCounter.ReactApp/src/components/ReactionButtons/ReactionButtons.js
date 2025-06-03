import "./ReactionButtons.css";
import axios from "axios";
import { useAuth } from "../../utils/AuthProvider/AuthProvider";
import { useEffect, useState } from "react";

function ReactionButtons({respect, reaction, targetActivityId = "", targetPersonId = "", targetParentId = ""}) {
    const {isLoggedIn, logout, openLoginPopup} = useAuth();
    const [displayedRespect, setDisplayedRespect] = useState(respect);
    const [clickedReaction, setClickedReaction] = useState(reaction);
    let targetUrl = "";

    if (targetActivityId !== "") 
    {
        targetUrl = `/api/activity/${targetActivityId}/reaction/`;
    } 
    else if (targetPersonId !== "") 
    {
        targetUrl = `/api/person/${targetPersonId}/reaction/`;
    } 
    else if (targetParentId !== "") 
    {
        targetUrl = `/api/comment/${targetParentId}/reaction/`;
    }
    
    const handleClick = async (reaction) => {
        if(!isLoggedIn)
        {
            openLoginPopup();
            return;
        }
        // to do: if reaction === clickedReaction -> remove reaction

        try {
            console.log('ReactionButtons: sending reaction request:', reaction);
            const response = await axios.post(targetUrl + reaction.toString() );
            setDisplayedRespect(response.data);
            setClickedReaction(reaction);
            console.log('ReactionButtons: success ', response.data);
        } catch (error) {
            if (error.response && error.response.status === 401) {
                console.warn('Unauthorized! Redirecting to login...');
                logout();
                openLoginPopup();
            } else {
                console.error('An error occurred:', error);
            }
        }
    };

    useEffect(() => {
        if(!isLoggedIn) {
            setClickedReaction(-1);
        };
    }, [isLoggedIn]);

    useEffect(() => {
        console.log(reaction);
        setClickedReaction(reaction)
    }, []);

    return (
        <div className="input-group">
            <button className={'btn btn-outline-primary ' + (clickedReaction === 0 ? 'active': '')} onClick={() => handleClick(0)}>
                <i className="bi bi-hand-thumbs-down-fill"></i>
                <i className="bi bi-hand-thumbs-down-fill"></i>
            </button>
            <button className={'btn btn-outline-primary ' + (clickedReaction === 1 ? 'active': '')} onClick={() => handleClick(1)}>
                <i className="bi bi-hand-thumbs-down-fill"></i>
            </button>
            <span className='form-control text-center reaction-buttons-rating'>
                {displayedRespect}
            </span>
            <button className={'btn btn-outline-primary ' + (clickedReaction === 2 ? 'active': '')} onClick={() => handleClick(2)}>
                <i className="bi bi-hand-thumbs-up-fill"></i>
            </button>
            <button className={'btn btn-outline-primary ' + (clickedReaction === 3 ? 'active': '')} onClick={() => handleClick(3)}>
                <i className="bi bi-hand-thumbs-up-fill"></i>
                <i className="bi bi-hand-thumbs-up-fill"></i>
            </button>
        </div>
    );
}

export default ReactionButtons;