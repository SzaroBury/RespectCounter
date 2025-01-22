import axios from "axios";
import { useAuth } from "../../utils/AuthProvider/AuthProvider";

function ReactionButtons({respect, targetActivityId = "", targetPersonId = "", targetParentId = ""}) {
    const {isLoggedIn, logout, openLoginPopup} = useAuth();
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

        try {
            console.log('ReactionButtons: sending reaction request:', reaction);
            const response = await axios.post(targetUrl + reaction.toString() );
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

    return (
        <div className="input-group">
            <button className='btn btn-outline-primary' onClick={() => handleClick(1)}>
                <i className="bi bi-hand-thumbs-down-fill"></i>
                <i className="bi bi-hand-thumbs-down-fill"></i>
            </button>
            <button className='btn btn-outline-primary' onClick={() => handleClick(2)}>
                <i className="bi bi-hand-thumbs-down-fill"></i>
            </button>
            <span className='rating form-control text-center'>
                {respect}
            </span>
            <button className='btn btn-outline-primary' onClick={() => handleClick(3)}>
                <i className="bi bi-hand-thumbs-up-fill"></i>
            </button>
            <button className='btn btn-outline-primary' onClick={() => handleClick(4)}>
                <i className="bi bi-hand-thumbs-up-fill"></i>
                <i className="bi bi-hand-thumbs-up-fill"></i>
            </button>
        </div>
    );
}

export default ReactionButtons;