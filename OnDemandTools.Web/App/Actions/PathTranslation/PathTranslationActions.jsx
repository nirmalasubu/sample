import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';
import * as configActions from 'Actions/Config/ConfigActions'

/****************** Actions ************************/
/// <summary>
/// Invoke 'FETCH_PATHTRANSLATION_SUCCESS' action
/// which will be handled by the appropriate reducer
/// </sumamry>
export const fetchPathTranslationComplete = (pathTranslationRecords) => {
    return {
        type: actionTypes.FETCH_PATHTRANSLATION_SUCCESS,
        pathTranslationRecords
    }
};

/// <summary>
/// Invoke 'DELETE_PATHTRANSLATION_SUCCESS' action
/// which will be handled by the appropriate reducer
/// </sumamry>
export const deletePathTranslationComplete = (pathTranslationObjId) => {
    return {
        type: actionTypes.DELETE_PATHTRANSLATION_SUCCESS,
        pathTranslationObjId
    }
};

/// <summary>
/// Invoke 'DELETE_PATHTRANSLATION_SUCCESS' action
/// which will be handled by the appropriate reducer
/// </sumamry>
export const savePathTranslationComplete = (pathTranslationObj) => {
    return {
        type: actionTypes.SAVE_PATHTRANSLATION_SUCCESS,
        pathTranslationObj
    }
};


/******************* Helper methods *******************/
/// <summary>
/// Asynchronously retrieve path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const fetchPathTranslationRecords = () => {
    return (dispatch) => {        
        return Axios.get('/api/pathtranslation')
            .then(response => {
                dispatch(fetchPathTranslationComplete(response.data));
            })
            .catch(error => {
                dispatch(configActions.handleApplicationAPIError(error));
            });
    };
};

/// <summary>
/// Asynchronously delete path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const deletePathTranslation = (id) => {
    return (dispatch) => {
        return Axios.delete('/api/pathtranslation/' + id)
            .then(response => {
                dispatch(deletePathTranslationComplete(id))
            })
            .catch(error => {
                dispatch(configActions.handleApplicationAPIError(error));
                throw (error);
            });
    };
};


/// <summary>
/// Asynchronously add/update path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const savePathTranslation = (object) => {
    return (dispatch) => {       
        return Axios.post('/api/pathtranslation/', object)
            .then(response => {
                // No need for this currently, uncomment if need be
                // dispatch(savePathTranslationComplete(response.data))
                dispatch(fetchPathTranslationRecords());               
            })
            .catch(error => {
                dispatch(configActions.handleApplicationAPIError(error));
                throw (error);
            });
    };
};

