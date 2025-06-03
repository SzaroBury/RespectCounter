import "./Activity.css";
import { Link } from "react-router-dom";
import ReactionButtons from "../../../components/ReactionButtons/ReactionButtons";

function Activity({a, showCommentsButton=true}) {

    return (
        <div key={"activity_" + a.id} className='activity border'>
            <div className='activity-header'>
                <div className="d-flex">
                    <div className='activity-person'>
                        <img className='activity-person-image' src={`/persons/${a.personImagePath.toLowerCase()}`} alt={a.author}/>
                    </div>
                    <div className="activity-person-info">
                        <span className="activity-person-name">{a.personFullName}</span>
                        <span className='activity-person-respect'>{a.personRespect} respect</span>
                    </div>
                </div>
                <span>{a.type === 1 ? ("Quotation") : ("Act")}</span>
            </div>
            <div className='activity-content'>
                <p><strong>{a.value}</strong></p>
                <p>{a.description}</p>
                <span className='activity-source'>Source: {a.source}</span>
            </div>
            
            <div className="activity-actions">
                {showCommentsButton && <CommentsButton act={a} />}
                <div className="activity-reaction-buttons">
                    <ReactionButtons respect={a.respect} reaction={a.currentUsersReaction} targetActivityId={a.id} />
                </div>
            </div>
        </div>
    )    
}

function CommentsButton({act}) {
    return (
        <Link className="btn btn-outline-primary p-2" to={"/act/" + String(act.id)}>
            <span>
                {act.commentsCount} {act.commentsCount === 1 ? "Comment" : "Comments"}
            </span>
            <i className="bi bi-chat-left m-2"></i>
        </Link>
    );
};

export default Activity;