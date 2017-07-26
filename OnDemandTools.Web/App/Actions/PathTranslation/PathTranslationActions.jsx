import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';

/****************** Actions ************************/
/// <summary>
/// Invoke 'FETCH_PATHTRANSLATION_SUCCESS' action
/// which will be handled by the appropriate reducer
/// </sumamry>
export const fetchPathTranslation = (pathTranslations) => {
    return {
        type: actionTypes.FETCH_PATHTRANSLATION_SUCCESS,
        pathTranslations
    }
};





/******************* Helper methods *******************/
/// <summary>
/// Asynchronously retrieve path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const fetchPathTranslations = () => {
    return (dispatch) => {
        return Axios.get('/api/pathtranslation')  // To work on
            .then(response => {
                dispatch(fetchPathTranslation(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};