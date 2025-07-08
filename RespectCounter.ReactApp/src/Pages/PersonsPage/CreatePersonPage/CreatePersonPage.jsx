import './CreatePersonPage.css';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { postPerson } from '../../../services/personService';

function CreatePersonPage() {
    const [formData, setFormData] = useState({ firstName: '', lastName: '', nickName: '', decs: '', nationality: '', birthDate: '', deathDate: '', tags: '' });
    const navigate = useNavigate();

    const handleDataChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleCreateButton = async () => {
        console.log('CreatePersonPage: handleCreateButton()', formData);
        postPerson(formData)
            .then(() => {
                navigate(`/person/${response.data.id}`);
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

    const handleCancelButton = async () => {
        navigate(-1);
    }

    return (
        <>
            <div></div>
            <div>
                <h2 className='mt-5'>Create a new person</h2>
                <div className="input-group w-75 mb-3">
                    <span className='input-group-text'><span className='text-danger'>* </span> Names:</span>
                    <input className='form-control' name='firstName' placeholder='First name...' onChange={handleDataChange} />
                    <input className='form-control' name='lastName' placeholder='Last name...' onChange={handleDataChange} />
                </div>
                <div class="input-group w-75 mb-3">
                    <span className='input-group-text'>Nickname:</span>
                    <input className='form-control' name='nickName' placeholder='Nick name...' onChange={handleDataChange} />
                </div>
                <div class="input-group w-75 mb-3">
                    <span className='input-group-text'><span className='text-danger'>*</span>Profession:</span>
                    <input className='form-control' name='profession' placeholder='Nick name...' onChange={handleDataChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">
                        Description:
                    </span>
                    <textarea className="form-control" name="desc" placeholder="" onChange={handleDataChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">Birth date:</span>
                    <input className="form-control" name="birthDate" type="date" />
                    <span className="input-group-text">Death date:</span>
                    <input className="form-control" name="deathDate" type="date" />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className='input-group-text'>Nationality:</span>
                    <input className="form-control" name="nationality" />
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
                    <button className="btn btn-outline-primary m-2" onClick={handleCancelButton}>
                        <h6 className='m-2'>Back</h6>
                    </button>
                    <button className="btn btn-outline-primary" onClick={handleCreateButton}>
                        <h4 className='m-3'>Create</h4>
                    </button>
                </div>
            </div>
            <div></div>
        </>
    );
}

function TagDropdown() {
    const [inputValue, setInputValue] = useState("");
    const [showDropdown, setShowDropdown] = useState(false);
    const [allTags, setAllTags] = useState([]);
    const [selectedTags, setSelectedTags] = useState([]);
    const [filteredTags, setFilteredTags] = useState([]);

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
        if (filteredTags.length === 0)
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

    return (
        <div className="position-relative">
            <div className="form-control">
                {
                    selectedTags.map((tag) =>
                        <button key={'btn_tag_' + tag.name} className="btn btn-outline-primary me-1" onClick={() => handleTagUnselected(tag)}>{tag.name}</button>
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
                {showDropdown && (
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