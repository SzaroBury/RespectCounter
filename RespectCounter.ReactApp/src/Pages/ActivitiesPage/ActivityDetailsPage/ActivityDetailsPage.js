import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from 'axios';
import "./ActivityDetailsPage.css";
import TagsMenu from "../../../components/TagsMenu/TagsMenu";
import Activity from "../Activity/Activity";
import Comment from "../../../components/Comment/Comment";
import CommentForm from "../../../components/CommentForm/CommentForm";
import { useAuth } from "../../../utils/AuthProvider/AuthProvider";
import Loading from "../../../components/Loading/Loading";

function ActivityDetailsPage(params) {
    const { id } = useParams();
    const [ act, setAct ] = useState(null);
    const [ comments, setComments ] = useState([]);
    const { isLoggedIn, user, openLoginPopup } = useAuth();
    const navigate = useNavigate();
    const handleGoBack = (nav) => { nav(-1); }

    useEffect(() => {
        loadActivity(id, setAct);
        loadComments(id, setComments);
      }, []);

    const loadActivity = () => {
        console.log("ActivityDetailsPage: loadActivity(id: '" + id + "')");

        axios.get(`/api/activity/${id}`)
        .then(response => {
            setAct(response.data);
        })
        .catch(error => {
            if (error.response) {
                console.error(`HTTP error! Status: ${error.response.status}`);
            } else if (error.request) {
                console.error("No response received: ", error.request);
            } else {
                console.error("Error setting up the request: ", error.message);
            }
        });
    }

    const loadComments = () => {
        console.log("ActivityDetailsPage: loadComments()");

        axios.get(`/api/activity/${id}/comments`)
        .then(response => {
            setComments(response.data);
        })
        .catch(error => {
            if (error.response) {
                console.error(`HTTP error! Status: ${error.response.status}`);
            } else if (error.request) {
                console.error("No response received: ", error.request);
            } else {
                console.error("Error setting up the request: ", error.message);
            }
        });
    }

    return (
        <>
            <div></div>
            <div className="p-3">
                <div>
                    <button className="btn btn-outline-primary" onClick={() => handleGoBack(navigate)}><i className="bi bi-arrow-left"></i></button>
                </div>
                <Loading loading={act === null} />
                {act && 
                <>
                    <Activity a={act} showCommentsButton={false}/>
                    <div className="ps-3 pe-3">
                        {isLoggedIn && <CommentForm targetActivityId={id} onCommentAdded={loadComments}/>}
                        <div className="comment-container ms-5 me-5">
                            <Loading loading={act.commentsCount > 1 && comments.length === 0} />
                            {comments.map(com =>
                                <Comment key={"Comment_" + com.id} comment={com} onReplyAdded={loadComments}/>
                            )}
                        </div>
                    </div>
                </>}
            </div>
            <TagsMenu currentTags={act ? act.tags.split(",") : null} countMode="countActivities" tagsSelected={[]} setTagsSelected={() => {}}/>
        </>
    );
}

export default ActivityDetailsPage;