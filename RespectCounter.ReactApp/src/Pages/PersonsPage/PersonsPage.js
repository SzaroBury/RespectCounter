import './PersonsPage.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import SortMenu from '../../components/SortMenu/SortMenu';
import TagsMenu from '../../components/TagsMenu/TagsMenu';
import Loading from '../../components/Loading/Loading';
import axios from 'axios';
import Person from './Person/Person';
import { useAuth } from '../../utils/AuthProvider/AuthProvider';

function PersonsPage({props}) {
    const [loading, setLoading] = useState(true);
    const [title, setTitle] = useState('Persons - Most Respected');
    const [persons, setPersons] = useState([]);
    const [sortOption, setSortOption] = useState("mr");
    const [scopeOption, setScopeOption] = useState(true);
    const [tagsSelected, setTagsSelected] = useState([]);
    const navigate = useNavigate();
    const {isLoggedIn, openLoginPopup} = useAuth();

    useEffect(() => {
        setLoading(true);
        loadPersons(sortOption, scopeOption);
    }, [sortOption, scopeOption]);

    const handleSortChange = (sortOptionName, sortOptionDiplay) => {
        console.log("PersonsPage: handleSortChange(" + sortOptionName + ", " + sortOptionDiplay + ")")
        setTitle("Persons - " + sortOptionDiplay);
        setSortOption(sortOptionName);
    };
    
    const handleScopeChange = () => {
        console.log("PersonsPage: handleScopeChange()");
        const targetValue = !scopeOption;
        setScopeOption(targetValue);
    };
    const handleTagSelected = () => {
        console.log('PersonsPage: handleTagSelected()')
    }

    const handleCreateButton = () => {
        console.log('PersonsPage: handleCreateButton()');
        if(isLoggedIn)
        {
            navigate('/person/create');
        }
        else
        {
            openLoginPopup();
        }
    }

    const loadPersons = (sortOption, onlyVerified) => {
        console.log('PersonsPage: loadPersons(sortOption: "' + sortOption + '", onlyVerified: "' + onlyVerified + '")')
        setPersons([]);
    
        const ifVerifiedUrl = onlyVerified ? "" : "/all";
    
        axios.get(`/api/persons${ifVerifiedUrl}`, {
            params: {
                order: sortOption
            }
        })
        .then(response => {
            setPersons(response.data);
            setLoading(false);
        })
        .catch(error => {
            setLoading(false);

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
            <SortMenu page="Persons" onSortOptionChange={handleSortChange} onScopeChange={handleScopeChange}/>
            <div></div>
            <div>
                <div className='text-end'>
                    <button className='btn btn-outline-primary' title="Propose a public figure" onClick={handleCreateButton}>
                        <span className='me-2'>Propose public figure</span> 
                        <i className="bi bi-person-plus-fill"></i>
                    </button>
                </div>
                <h2>{title}</h2>
                <Loading loading={loading}/>
                {persons.map((per, index) => 
                    <Person key={"Person_" + per.id} person={per} index={index}/>
                )}
            </div>
            <div></div>
            <TagsMenu countMode="countPersons" tagsSelected={tagsSelected} setTagsSelected={setTagsSelected}/>
        </>
    );
}

export default PersonsPage;

