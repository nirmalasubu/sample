﻿import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';


// Future  we get data through Axios or fetch

export const fetchAiringIdSuccess = (currentAiringIds) => {
    return {
        type: actionTypes.FETCH_AIRINGID_SUCCESS,
        currentAiringIds
    }
};

export const filterAiringIdSuccess= (filterDistribution) => {
    return {
        type: actionTypes.FILTER_AIRINGID_SUCCESS,
        filterDistribution
    }
};

export const fetchCurrentAiringId = () => {
    return (dispatch) => {
        return Axios.get('/api/distribution')
            .then(response => {
                dispatch(fetchAiringIdSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const getNewAiringId = () => {
    return Axios.get('/api/distribution/newcurrentairingid')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};

export const saveAiringIdSuccess = (currentAiringId) => {
    return {
        type: actionTypes.SAVE_AIRINGID_SUCCESS,
        currentAiringId
    }
};

export const saveCurrentAiringId = (model) => {
    return (dispatch) => {        
        return Axios.post('/api/distribution', model)
            .then(response => {
                dispatch(saveProductSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const deleteCurrentAiringIdSuccess = (objectId) => {
    return {
        type: actionTypes.DELETE_AIRINGID_SUCCESS,
        objectId
    }
};

export const deleteCurrentAiringId = (id) => {
    return (dispatch) => {        
        return Axios.delete('/api/distribution/'+ id)
            .then(response => {
                dispatch(deleteCurrentAiringIdSuccess(id))
            })
            .catch(error => {
                throw (error);
            });
    };
};

