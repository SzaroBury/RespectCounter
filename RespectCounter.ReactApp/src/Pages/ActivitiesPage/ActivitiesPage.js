import React from 'react';
import SortMenu from '../../components/SortMenu/SortMenu';
import './ActivitiesPage.css';
import TagsMenu from '../../components/TagsMenu/TagsMenu';
import axios from 'axios';

class ActivitiesPage extends React.Component {
    static displayName = "RespectCounter - Events";
    
    constructor(props) {
        super(props);
        this.state = { 
            loading: true,
            first: true,
            title: "Events - Latest added",
            sortOption: "la",
            scopeOption: true,
            activities: [],
        };

        this.handleSortChange = (sortOptionName, sortOptionDiplay) => {
            console.log("ActivitiesPage: handleSortChange(" + sortOptionName + ", " + sortOptionDiplay + ")")
            // const targetName = sortOptionName;
            this.setState({ 
                title: "Events - " + sortOptionDiplay,
                sortOption: sortOptionName,
            });
            console.log("   targetName: " + sortOptionName)
            this.loadActivities(sortOptionName, this.state.scopeOption);
        };
        
        this.handleScopeChange = () => {
            console.log("ActivitiesPage: handleScopeChange()");
            const targetValue = !this.state.scopeOption;
            this.setState({ scopeOption: targetValue });
            this.loadActivities(this.state.sortOption, targetValue);
        };

        this.handleTagSelected = () => {
            console.log("ActivitiesPage: handleTagSelected()");
            this.setState({
                title: "Events - Latest added",
                sortOption: "la",
                scopeOption: true,
            });

            this.loadActivities("la", false);
        };
    }

    componentDidMount() {
        console.log("ActivitiesPage: componentDidMount()")
        this.loadActivities("la", this.state.scopeOption);
    }

    render() {
        return (
            <>
                <SortMenu page="Activities" onSortOptionChange={this.handleSortChange} onScopeChange={this.handleScopeChange}/>
                <div className='act-list'>
                    <h2>{this.state.title}</h2>
                    <ActivityList list={this.state.activities}/>
                </div>
                <TagsMenu onTagSelected={this.handleTagSelected}/>
            </>
        );
    }

    loadActivities(sortOption, onlyVerified) {
        console.log("ActivitiesPage: loadActivities(" + sortOption + ", " + onlyVerified + ")");
        this.setState({ activities: [] });
    
        const targetUrl = onlyVerified ? "activities" : "activities/all";
    
        axios.get(`/api/${targetUrl}`, {
            params: {
                order: sortOption
            }
        })
        .then(response => {
            // Automatyczne parsowanie JSON w Axios
            this.setState({ activities: response.data });
        })
        .catch(error => {
            if (error.response) {
                // Błąd odpowiedzi HTTP (np. 404, 500)
                console.error(`HTTP error! Status: ${error.response.status}`);
            } else if (error.request) {
                // Żądanie zostało wysłane, ale brak odpowiedzi
                console.error("No response received: ", error.request);
            } else {
                // Inne błędy
                console.error("Error setting up the request: ", error.message);
            }
        });
    }
}

function ActivityList({list}) {   
    return (
        list.map((act) => 
            <div key={"activity_" + act.id} className='activity m-3 p-3 border'>
                <div className='person'>
                    <div className='imageContainer'>
                        <img className='image' src={act.personImagePath} alt={act.author}/>
                        <p className='caption'><b>{act.personName}</b></p>
                    </div>
                    <span className='personRate'>
                        <span className='form-control text-center'>{act.personRespect}</span>
                    </span>
                </div>
                <div className='contentContainer'>
                    <p>{act.value}</p>
                    <span className='source'>Source: {act.source}</span>
                </div>
                
                <div className='buttons'>
                    <div></div>
                    <div className='input-group'>
                        <button className='btn btn-secondary'>
                            <i className="bi bi-hand-thumbs-down-fill"></i>
                            <i className="bi bi-hand-thumbs-down-fill"></i>
                        </button>
                        <button className='btn btn-secondary'>
                            <i className="bi bi-hand-thumbs-down-fill"></i>
                        </button>
                        <span className='rating form-control text-center'>
                            {act.respect}
                        </span>
                        <button className='btn btn-secondary'>                                
                            <i className="bi bi-hand-thumbs-up-fill"></i>
                        </button>
                        <button className='btn btn-secondary'>
                            <i className="bi bi-hand-thumbs-up-fill"></i>
                            <i className="bi bi-hand-thumbs-up-fill"></i>
                        </button>
                    </div>
                    <div>

                    </div>
                    <button className='btn btn-secondary p-2'>
                        <span>
                            {act.comments} Comments
                        </span>
                        <i className="bi bi-chat-left m-2"></i>
                    </button>
                </div>
            </div>
        )
    )
}



export default ActivitiesPage;

