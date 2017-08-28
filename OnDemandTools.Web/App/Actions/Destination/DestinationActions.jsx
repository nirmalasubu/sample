import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';


// Future  we get data through Axios or fetch

export const fetchDestinationSuccess = (destinations) => {
    return {
        type: actionTypes.FETCH_DESTINATIONS_SUCCESS,
        destinations
    }
};

export const saveDestinationSuccess = (destination) => {
    return {
        type: actionTypes.SAVE_DESTINATION_SUCCESS,
        destination
    }
};

export const deleteDestinationSuccess = (objectId) => {
    return {
        type: actionTypes.DELETE_DESTINATION_SUCCESS,
        objectId
    }
};

export const fetchDestinations = () => {
    return (dispatch) => {
        return Axios.get('/api/destination')
            .then(response => {
                dispatch(fetchDestinationSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const getNewDestination = () => {
    return Axios.get('/api/destination/newdestination')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};

export const getDestinations = () => {
    return Axios.get('/api/destination/getdestinations')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};

export const saveDestination = (model) => {
    return (dispatch) => {        
        return Axios.post('/api/destination', model)
            .then(response => {
                dispatch(saveDestinationSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const deleteDestination = (id) => {
    return (dispatch) => {        
        return Axios.delete('/api/destination/' + id)
            .then(response => {
                dispatch(deleteDestinationSuccess(id))
            })
            .catch(error => {
                throw (error);
            });
    };
};


