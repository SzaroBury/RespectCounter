import "./ReactionButtons.css";
import { useAuth } from "../../utils/AuthProvider/AuthProvider";
import { useEffect, useState } from "react";
import { postReaction } from "../../services/reactionService";

function ReactionButtons({respect, reaction, targetType = "", targetId = "", showButtons = true}) {
    const {isLoggedIn, logout, openLoginPopup} = useAuth();
    const [displayedRespect, setDisplayedRespect] = useState(respect);
    const [clickedReaction, setClickedReaction] = useState(reaction);

    const handleClick = async (reaction) => {
        if(!isLoggedIn)
        {
            openLoginPopup();
            return;
        }
        // to do: if reaction === clickedReaction -> remove reaction

        if (targetType === "") {
            throw Error("");
        }

        try {
            const response = await postReaction({ targetType, targetId, reaction });
            setDisplayedRespect(response);
            setClickedReaction(reaction);
            console.log('ReactionButtons: success ', response);
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
        setClickedReaction(reaction) 
    }, [reaction]);

    return (
        <div className="input-group">
            {showButtons && 
                <>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === -2 ? 'active': '')} onClick={() => handleClick(-2)}>
                        <i className="bi bi-hand-thumbs-down-fill"></i>
                        <i className="bi bi-hand-thumbs-down-fill"></i>
                    </button>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === -1 ? 'active': '')} onClick={() => handleClick(-1)}>
                        <i className="bi bi-hand-thumbs-down-fill"></i>
                    </button>
                </>
            }
            <span className='form-control text-center reaction-buttons-rating'>
                {displayedRespect}
            </span>
            {showButtons && 
                <>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === 1 ? 'active': '')} onClick={() => handleClick(1)}>
                        <i className="bi bi-hand-thumbs-up-fill"></i>
                    </button>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === 2 ? 'active': '')} onClick={() => handleClick(2)}>
                        <i className="bi bi-hand-thumbs-up-fill"></i>
                        <i className="bi bi-hand-thumbs-up-fill"></i>
                    </button>
                </>
            }
        </div>
    );
}

export default ReactionButtons;