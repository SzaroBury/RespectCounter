import './Loading.css';

function Loading({loading}) { 
    return (
        <> { loading &&
            <div className='d-flex justify-content-center mt-4 mb-4'>
                <div className="spinner-grow text-danger spinner-1" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
                <div className="spinner-grow text-success spinner-2" role="status"></div>
                <div className="spinner-grow text-primary spinner-3" role="status"></div>   
            </div>
        } </>
    )
}

export default Loading;