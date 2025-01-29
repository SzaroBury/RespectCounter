import './CreatePersonPage.css';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

function CreatePersonPage() {
    const [formData, setFormData] = useState({firstName: '', lastName: '', nickName: '', decs: '', nationality: '', birthDate: '', deathDate: '', tags: ''});
    const navigate = useNavigate();
    
    const handleDataChange = (e) => {
        const { name, value } = e.target;
        setFormData({...formData, [name]: value});
    };

    const handleCreateButton = async () => {
        try {
            console.log('CreatePersonPage: handleCreateButton()', formData);
            const response = await axios.post('/api/person', formData);
            console.log('CreatePersonPage: success ', response.data);
            navigate(`/person/${response.data.id}`);
        } catch (error) {
            if (error.response && error.response.status === 401) {
                console.warn('Unauthorized! Redirecting to login...');
            } else {
                console.error('An error occurred:', error);
            }
        }   
    };

    return(
        <>
            <div></div>
            <div>
                <h2 className='mt-5'>Create a new public figure</h2>
                <div className="input-group w-75 mb-3">
                    <span className='input-group-text'>Names<span className='text-danger'>*</span></span>
                    <input className='form-control' name='firstName' placeholder='First name...' onChange={handleDataChange}/>
                    <input className='form-control' name='lastName' placeholder='Last name...' onChange={handleDataChange}/>
                    <input className='form-control' name='nickName' placeholder='Nick name...' onChange={handleDataChange}/>
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">
                        Description
                    </span>
                    <textarea className="form-control" name="desc" placeholder="" onChange={handleDataChange}/>
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">Birth date:</span>
                    <input className="form-control" name="birthDate" type="date"/>
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">Death date:</span>
                    <input className="form-control" name="deathDate" type="date"/>
                </div>
                <div className="input-group w-75 mb-3">
                    <input className="form-control" name="nationality" placeholder='Nationality'/>
                </div>
                <span>Add some tags:</span>
                <div className="w-75 mb-3">
                    <TagDropdown />
                </div>
                
                <div className="mt-3 w-75 text-end">
                    <span>
                    Fields marked with <span className="text-danger">*</span> are required. 
                    </span>
                </div>
                <div className="mt-3 w-75 text-end">
                    <button className="btn btn-outline-primary" onClick={handleCreateButton}>Create</button>
                </div>


            </div>
            <div></div>
        </>
    );
}

function TagDropdown() {
    const [ inputValue, setInputValue ] = useState("");
    const [ showDropdown, setShowDropdown ] = useState(false);
    const [ allTags, setAllTags ] = useState([]);
    const [ selectedTags, setSelectedTags ] = useState([]);
    const [ filteredTags, setFilteredTags ] = useState([]);

    useEffect(() => {
        loadTags();
    }, []);

    useEffect(() => {
        setFilteredTags(
            allTags.filter((tag) =>
                tag.name.toLowerCase().includes(inputValue.toLowerCase())
            )
        );
    }, [inputValue, allTags]);

    useEffect(() => {
        if(filteredTags.length === 0)
            setShowDropdown(false);
    }, [filteredTags]);

    const loadTags = () => {
        console.log('TagDropdown: loadTags()');
        setAllTags([]);
    
        axios.get('/api/tags/simple')
        .then(response => {
            setAllTags(response.data);
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
    };

    const handleTagSelect = (tag) => {
        if (!selectedTags.some((t) => t.id === tag.id)) {
            setSelectedTags([...selectedTags, tag]);
        }
        setInputValue("");
        setShowDropdown(false);
    };

    const handleTagUnselected = (tag) => {
        const updatedTags = selectedTags.filter((t) => t.id !== tag.id);
        setSelectedTags(updatedTags);
    };

    return(
        <div className="position-relative">
            <div className="form-control">
            { 
                selectedTags.map((tag) => 
                    <button key={'btn_tag_' + tag.name} className="btn btn-outline-primary me-1" onClick={() => handleTagUnselected(tag)}>{ tag.name }</button>
                )
            }
            <input className="border-0 p-2" 
                value={inputValue}
                onChange={(e) => setInputValue(e.target.value)}
                onFocus={() => setShowDropdown(true)}
                onBlur={() => setTimeout(() => setShowDropdown(false), 250)}
                type='text' 
                placeholder="Type to search..." 
            />
            { showDropdown && (
                <ul className="dropdown-list">
                {
                    filteredTags.map((tag) => (
                        <li 
                            className="tag-dropdown-option"
                            key={tag.id} 
                            onClick={() => handleTagSelect(tag)} 
                        >
                            {tag.name}
                        </li>
                    ))
                }
                </ul>
            )}
            </div>
        </div>
    );
}

export default CreatePersonPage;