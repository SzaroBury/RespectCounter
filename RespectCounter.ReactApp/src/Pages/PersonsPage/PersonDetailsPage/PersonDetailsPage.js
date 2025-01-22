import "./PersonDetailsPage.css";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { useAuth } from "../../../utils/AuthProvider/AuthProvider";
import axios from 'axios';
import Person from "../Person/Person";
import TagsMenu from "../../../components/TagsMenu/TagsMenu";
import Comment from "../../../components/Comment/Comment";
import CommentForm from "../../../components/CommentForm/CommentForm";
import Loading from "../../../components/Loading/Loading";


function PersonDetailsPage() {
    const { id } = useParams();
    const [ person, setPerson ] = useState(null);
    const [ comments, setComments ] = useState([]);
    const { isLoggedIn } = useAuth();
    const navigate = useNavigate();
    const handleGoBack = (nav) => { nav(-1); }

    useEffect(() => {
        loadPerson(id, setPerson);
        loadComments(id, setComments);
      }, []);

    const loadPerson = () => {
        console.log("PersonDetailsPage: loadPerson(id: '" + id + "')");

        axios.get(`/api/person/${id}`)
        .then(response => {
            setPerson(response.data);
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
        console.log("PersonDetailsPage: loadComments()");

        axios.get(`/api/person/${id}/comments`)
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
                <Loading loading={ person === null } />
                {person && 
                <>
                    <Person person={person} showCommentsButton={false}/>
                    <div className="ps-3 pe-3">
                        {isLoggedIn && <CommentForm targetActivityId={id} onCommentAdded={loadComments}/>}
                        <div className="comment-container ms-5 me-5">
                            <Loading loading={person.commentsCount > 1 && comments.length === 0} />
                            {comments.map(com =>
                                <Comment key={"Comment_" + com.id} comment={com} onReplyAdded={loadComments}/>
                            )}
                        </div>
                    </div>
                </>}
            </div>
            <TagsMenu currentTags={person ? person.tags.map(t => t.name) : null} countMode="countPersons" tagsSelected={[]} setTagsSelected={() => {}}/>
        </>
    );
}

export default PersonDetailsPage;