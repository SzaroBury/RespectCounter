import React, { useState, useEffect } from 'react';
import './TagsMenu.css';

function TagsMenu({onTagSelected}) {
    return (
        <div className="tags-menu">
            <section>
                <h5 className='m-3'>Tags</h5> 
            </section>
            <RecentBrowsedTags />
            <FavouriteTags />
            <TrendingTags />
            <AllTags onTagSelection={onTagSelected}/>
        </div>
    );
}

function TrendingTags() {
    // to do

    return null;
}

function RecentBrowsedTags() {
    const [tags, setTags] = useState([]);

    useEffect(() => {
        console.log("Fetching recent browsed tags...");
        fetch('/api/tags/recent') // to do on API side
            .then(response => response.json())
            .then(data => setTags(data))
            .catch(error => console.error("Error fetching recent tags:", error));
    }, []); // Empty dependency array ensures this runs only once, like `componentDidMount`

    if (tags.length === 0) {
        return null; // Do not render the section if no tags are available
    }

    return (
        <section>
            <h6>Recent browsed:</h6>
            <ul>
                {tags.map((tag) => (
                    <li key={"RecentTag_" + tag}>{tag.name} ({tag.count})</li>
                ))}
            </ul>
        </section>
    );
}

function FavouriteTags() {
    const [tags, setTags] = useState([]);

    useEffect(() => {
        console.log("Fetching favourite tags...");
        fetch('/api/tags/favourite') // todo on API side
            .then(response => response.json())
            .then(data => setTags(data))
            .catch(error => console.error("Error fetching favourite tags:", error));
    }, []);

    if (tags.length === 0) {
        return null;
    }

    return (
        <section>
            <h6>Favourite:</h6>
            <ul>
                {tags.map((tag) => (
                    <li key={"FavouriteTag_" + tag}>{tag.name} ({tag.count})</li>
                ))}
            </ul>
        </section>
    );
}

function AllTags({onTagSelection}) {
    const [tags, setTags] = useState(["loading..."]);
    const [selectedTagIndex, setSelectedTagIndex] = useState(null);
    const onTagClick = (tags, index) => {
        console.log("   AllTags: onTagClick(" + index + ")");
        onTagSelection(tags[index].name);
        setSelectedTagIndex(index); 
    };

    useEffect(() => {
        console.log("Fetching all tags...");
        fetch('/api/tags') // Replace with your actual API endpoint
            .then(response => response.json())
            .then(data => { 
                setTags(data); 
                console.log(data);
            })
            .catch(error => console.error("Error fetching all tags:", error));
    }, []);

    if (tags.length === 0) {
        return null;
    }
    

    return (
        <section>
            <h6>All:</h6> 
            <ul>
                {tags.map((tag, index) => (
                    <li key={"Tag_" + index} 
                        className={selectedTagIndex === index  ? "selected-tag" : ""}
                        onClick={() => onTagClick(tags, index)}
                    >
                        {tag.name} ({tag.count})
                    </li>
                ))}
            </ul>
        </section>
    );
}

function loadTags() {

}

export default TagsMenu;