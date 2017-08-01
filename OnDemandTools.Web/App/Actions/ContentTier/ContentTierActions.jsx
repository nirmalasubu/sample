import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';


export const filterContentTierSuccess = (filterContentTier) => {
    return {
        type: actionTypes.FILTER_CONTENT_TIER_SUCCESS,
        filterContentTier
    }
};

export const fetchContentTierSuccess = (contentTiers) => {
    return {
        type: actionTypes.FETCH_CONTENT_TIER_SUCCESS,
        contentTiers
    }
};

export const saveContentTierSuccess = (contentTier) => {
    return {
        type: actionTypes.SAVE_CONTENT_TIER_SUCCESS,
        contentTier
    }
};

export const fetchContentTiers = () => {
    return (dispatch) => {
        return Axios.get('/api/ContentTier')
            .then(response => {
                dispatch(fetchContentTierSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const saveContentTier = (model) => {
    return (dispatch) => {
        return Axios.post('/api/ContentTier', model)
            .then(response => {
                dispatch(saveContentTierSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const getNewContentTier = () => {
    return Axios.get('/api/ContentTier/newContentTier')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};

export const deleteContentTierSuccess = (name) => {
    return {
        type: actionTypes.DELETE_CONTENT_TIER_SUCCESS,
        name
    }
};

export const deleteContentTier = (contentTier) => {
    return (dispatch) => {
        return Axios.delete('/api/ContentTier', { data: contentTier })
            .then(response => {
                dispatch(deleteContentTierSuccess(contentTier.name))
            })
            .catch(error => {
                throw (error);
            });
    };
};

