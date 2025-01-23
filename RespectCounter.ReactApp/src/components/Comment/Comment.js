import './Comment.css';
import { useEffect, useState } from "react";
import ReactionButtons from '../ReactionButtons/ReactionButtons';
import { useAuth } from '../../utils/AuthProvider/AuthProvider';
import axios from 'axios';

function Comment({comment, onReplyAdded}) {
    const imagePath = comment.imagePath ?? "/users/default.png";
    const [showReplyForm, setShowReplyForm] = useState(false);
    const [timestamp, setTimestamp] = useState('');
    const {isLoggedIn, openLoginPopup} = useAuth();

    useEffect(() => {
        calculateTimestamp();
    }, []);

    const handleReplyAdded = () => {
        setShowReplyForm(false);
        onReplyAdded();
    };

    const handleReplyButton = () => {
        if(isLoggedIn) {
            setShowReplyForm((prev) => !prev)
        } else {
            openLoginPopup();
        };
    }

    const calculateTimestamp = () => {
        const currentTime = new Date();
        const commentCreated = Date.parse(comment.created);
        const diffTime = Math.abs(currentTime - commentCreated);
        
        const diffMinutes = Math.floor(diffTime / (1000 * 60)); 
        const diffHours = Math.floor(diffMinutes / 60); 
        const diffDays = Math.floor(diffHours / 24);
        
        if (diffDays > 0) {
            setTimestamp(`${diffDays} day${diffDays > 1 ? 's' : ''} ago`);
        } else if (diffHours > 0) {
            setTimestamp(`${diffHours} hour${diffHours > 1 ? 's' : ''} ago`);
        } else if (diffMinutes > 0) {
            setTimestamp(`${diffMinutes} minute${diffMinutes > 1 ? 's' : ''} ago`);
        } else {
            setTimestamp('Just now');
        }
    };

    return(
        <>
            <div className={"comment border" + (showReplyForm ? ' comment-opened-reply-form' : '')}>
                <div className="comment-header">
                    <img className='comment-user-image' src={imagePath} alt={comment.createdBy}/>
                    <div className='comment-info'>
                        <span className='comment-username'>{comment.createdBy} </span>
                        <span className="comment-timestamp">{timestamp}</span>
                    </div>
                </div>
                <p className="comment-content">{comment.content}</p>
                <div className='comment-actions'>
                    <button className='comment-reply-btn btn btn-outline-primary' onClick={handleReplyButton}>
                        <i className="bi bi-reply-fill me-2"></i>Reply
                    </button>
                    <div className="reaction-buttons-container">
                        <ReactionButtons 
                            respect={comment.respect} 
                            targetParentId={comment.id}
                        />
                    </div>
                </div>
            </div>
            { 
                showReplyForm && 
                <ReplyForm 
                    commentId={comment.id} 
                    onCancel={() => setShowReplyForm(false)} 
                    onReplyAdded={handleReplyAdded}
                /> 
            }
            <Replies 
                replies={comment.children} 
                count={comment.childrenCount} 
            />
        </>
    );
}

function ReplyForm({commentId, onReplyAdded, onCancel}) {
    const [ inputValue, setInputValue ] = useState('');
    const { isLoggedIn, openLoginPopup, logout } = useAuth();

    const handleInputValueChange = (e) => {
        setInputValue(e.target.value);
    };

    const handleAddCommentClick = async () => {
        if(!isLoggedIn) 
        {
            openLoginPopup();
            return;
        }
        
        try {
            console.log('ReplyForm: sending a reply request.');
            const response = await axios.post(`/api/comment/${commentId}`, inputValue, {
                headers: { 
                    "Content-Type": "application/json" 
                }
            });
            console.log('ReplyForm: success ', response.data);
            if(onReplyAdded) onReplyAdded();
            setInputValue('');
        } catch (error) {
            if(error.status === 401) {
                logout();
                onCancel();
            } else {
                console.error('Error:', error);
            };
        }
    };

    return(
        <div className="comment-reply-form input-group">
            <span className="input-group-text">Reply: </span>
            <input className="form-control" value={inputValue} onChange={handleInputValueChange}/>
            <button className="btn btn-outline-primary" onClick={onCancel}><i className="bi bi-x-square-fill"></i></button>
            <button className="btn btn-outline-primary" onClick={handleAddCommentClick}><i className="bi bi-caret-right-fill"></i></button>
        </div>
    );
}

function Replies({replies, count}) {
    const [showReplies, setShowReplies] = useState(false);

    return(
        <div>
        {
            showReplies ? (
                <div className='mt-2'>
                    <span onClick={() => {setShowReplies(false)}}>
                        <i class="bi bi-caret-up-fill"></i>
                        Hide replies
                    </span>
                    {replies.map(reply => 
                        <div className='comment-child'>
                            <Comment 
                                key={"Comment_" + reply.id} 
                                comment={reply} 
                            />
                        </div>
                    )}
                </div>
            ) : (
                <>
                    {
                        count > 0 &&
                        <div className='mt-2'>
                            <span onClick={() => { setShowReplies(true) }}>
                                <i class="bi bi-caret-down-fill"></i>
                                {count === 1 ? "Show reply" : `Show ${count} replies`}
                            </span> 
                        </div>
                    }
                </>
            )
        }
        </div>
    );
}

export default Comment;