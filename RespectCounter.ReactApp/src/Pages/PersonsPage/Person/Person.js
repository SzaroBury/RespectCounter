import "./Person.css";
import { useNavigate as useNavigate } from "react-router-dom";
import ReactionButtons from "../../../components/ReactionButtons/ReactionButtons";

function Person({person, index}) {
    const imagePath = person.imagePath === "" ? "default" : person.imagePath;
    const navigate = useNavigate();
    const onImgError = (e) => {
        e.target.onerror = null;
        e.target.src = "/persons/default.jpg";
    } 

    return(        
    <div className='person border'>
        <div className='person-header' onClick={() => navigate(`/person/${person.id}`)}>
            {index !== undefined && 
            <>
                <div className='person-index'>
                    {index + 1}.
                </div>
            </>
            }
            <div className='person-image-container'>
                <img className='person-image' src={`/persons/person_${person.lastName.toLowerCase()}.jpg`} onError={(e) => onImgError(e)} alt={person.fullName}/>
            </div>
            <div className="person-info">
                <span className='person-name'>{person.fullName}</span>
                <span className='person-respect'>{person.respect} respect</span>
            </div>
        </div>
        <div className='person-content'>
            <p>{person.tags.map(tag => tag.name).join(", ")}</p>
        </div>
        {index === undefined && 
            <div>
                <p>{person.description}</p>
            </div>
        }
        <div className="person-actions">
            <div className="person-reaction-buttons">
                <ReactionButtons respect={person.respect} targetPersonId={person.id} />
            </div>
        </div>
        
    </div>);
}

export default Person;