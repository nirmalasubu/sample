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

export const filterDestinationSuccess= (filterDestination) => {
    return {
        type: actionTypes.FILTER_DESTINATION_SUCCESS,
        filterDestination
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

export const filterDestinations = (filterDestination) => {
    return (dispatch) => {
       dispatch(filterDestinationSuccess(filterDestination))
    };
};

export const getNewDestination = () => {
    return Axios.get('/api/deliveryqueue/newqueue')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
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


