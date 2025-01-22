import React, { useState } from 'react';
import './SortMenu.css';

function SortMenu({page, onSortOptionChange, onScopeChange}) {

    return (
        <div className="sort-menu">
            <section>
                <h5 className='m-3'>Sorting options</h5> 
                <BrowseSection page={page} onSortOptionChangeClick={onSortOptionChange} />
            </section>
            <hr/>
            <section>
                <ul>
                    <li>
                        <label>Show only verified</label>
                        <input className='m-3' type='checkbox' defaultChecked={true} onClick={(event) => onScopeChange()}/>
                    </li>
                </ul>
            </section>
        </div>
    );
};

function BrowseSection({page, onSortOptionChangeClick}) {
    const [selectedOption, setSelectedOption] = useState(0);

    const optionsActivities = [
        { name: "la", displayName:"Latest added" },
        { name: "lh", displayName:"Latest" },
        { name: "mr", displayName:"Most respected" },
        { name: "lr", displayName:"Least respected" },
        { name: "tr", displayName:"Trending" },
        //  todo { name: "bm", displayName:"Best matching" },
    ];

    const optionsPersons = [
        { name: "mr", displayName:"Most Respected" },
        { name: "lr" , displayName:"Worst" },
        { name: "la" , displayName:"Latest added" },
        { name: "Az", displayName:"Alphabetical" },
        { name: "tr", displayName:"Trending" },
    ];

    const options = page === "Activities" ? optionsActivities : optionsPersons;
    const onOptionClick = (options, index) => {
        console.log("   BrowseSection: onOptionClick(" + index + ")");
        onSortOptionChangeClick(options[index].name, options[index].displayName);
        setSelectedOption(index); 
    };

    return (
        <ul>
            {options.map((option, index) => (
                <li
                    key={index}
                    className={selectedOption === index  ? "selected-option" : ""}
                    onClick={() => onOptionClick(options, index)}
                >
                    {option.displayName}
                </li>
            ))}
        </ul>
    );
}

export default SortMenu;