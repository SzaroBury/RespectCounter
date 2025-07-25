import { useState } from "react";
import Comment from "../Comment";

function Replies({replies, count}) {
    const [showReplies, setShowReplies] = useState(false);

    return(
        <>
        {
            showReplies ? (
                <div className='mt-2'>
                    <span onClick={() => {setShowReplies(false)}}>
                        <i className="bi bi-caret-up-fill me-2"></i>
                        Hide replies
                    </span>
                    {replies.map(reply => 
                        <div className='comment-child' key={"Comment_" + reply.id}>
                            <Comment comment={reply} 
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
                                <i className="bi bi-caret-down-fill me-2"></i>
                                {count === 1 ? "Show reply" : `Show ${count} replies`}
                            </span> 
                        </div>
                    }
                </>
            )
        }
        </>
    );
}

export default Replies;