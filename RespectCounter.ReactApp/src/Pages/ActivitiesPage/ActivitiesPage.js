import './ActivitiesPage.css';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import SortMenu from '../../components/SortMenu/SortMenu';
import TagsMenu from '../../components/TagsMenu/TagsMenu';
import Activity from './Activity/Activity';
import Loading from '../../components/Loading/Loading';
import { useAuth } from '../../utils/AuthProvider/AuthProvider';

function ActivitiesPage(props) {
    const [loading, setLoading] = useState(true);
    const [title, setTitle] = useState("Activities - Latest added");
    const [sortOption, setSortOption] = useState("la");
    const [onlyVerifiedOption, setOnlyVerifiedOption] = useState(true);
    const [tagsSelected, setTagsSelected] = useState([]);
    const [activities, setActivities] = useState([]);
    const navigate = useNavigate();
    const { isLoggedIn, openLoginPopup } = useAuth();

    useEffect(() => {
        setLoading(true);
        loadActivities();
    }, [sortOption, onlyVerifiedOption]);

    useEffect(() => {
        setLoading(true);
        loadActivities();
    }, [tagsSelected]);

    const handleSortChange = (sortOptionName, sortOptionDiplay) => {
        console.log("ActivitiesPage: handleSortChange(" + sortOptionName + ", " + sortOptionDiplay + ")")
        setTitle("Activities - " + sortOptionDiplay);
        setSortOption(sortOptionName);
    };
    
    const handleScopeChange = () => {
        console.log("ActivitiesPage: handleScopeChange()");
        setOnlyVerifiedOption(!onlyVerifiedOption);
    };

    const handleCreateButton = () => {
        if(isLoggedIn) {
            navigate("/activity/create");
        } else {
            openLoginPopup()
        };
    };

    const loadActivities = () => {
        console.log("ActivitiesPage: loadActivities()");
        setActivities([]);
    
        const targetUrl = onlyVerifiedOption ? "activities" : "activities/all";
        let requestParams;
        if( tagsSelected.length === 0) {
            requestParams = { params: { order: sortOption }}
        } else {
            requestParams = { params: { order: sortOption, tags: tagsSelected.map(ts => ts.name).join(",") }};
        };
    
        axios.get(`/api/${targetUrl}`, requestParams)
        .then(response => {
            setActivities(response.data);
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
            <TagsMenu countMode="countActivities" tagsSelected={tagsSelected} setTagsSelected={setTagsSelected}/>
            <div className='act-list'>
                <div className='text-end'>
                    <button className='btn btn-outline-primary' title="Post a public activity" onClick={handleCreateButton}>
                        <span className='me-2'>Post a public activity</span>
                        <i className="bi bi-file-earmark-plus-fill"></i>
                    </button>
                </div>
                <h2>{title}</h2>
                <Loading loading={loading}/>
                {activities.map((act) => 
                    <Activity key={"Act_" + act.id} a={act}/>
                )}
            </div>
            <SortMenu page="Activities" onSortOptionChange={handleSortChange} onScopeChange={handleScopeChange}/>
        </>
    );
}

export default ActivitiesPage;