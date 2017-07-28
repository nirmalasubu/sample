import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';
import PathTranslationModel from 'Components/PathTranslations/PathTranslationModel';
import * as configActions from 'Actions/Config/ConfigActions'

/****************** Actions ************************/
/// <summary>
/// Invoke 'FETCH_PATHTRANSLATION_SUCCESS' action
/// which will be handled by the appropriate reducer
/// </sumamry>
export const fetchPathTranslationComplete = (pathTranslationObj) => {
    return {
        type: actionTypes.FETCH_PATHTRANSLATION_SUCCESS,
        pathTranslationObj
    }
};




/******************* Helper methods *******************/
/// <summary>
/// Asynchronously retrieve path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const fetchPathTranslation = (object) => {
    return (dispatch) => {
       console.log('here');
        return Axios.get('/api/destinatsion')  
            .then(response => {
                let obj = new PathTranslationModel(response.data);              
                dispatch(fetchPathTranslationComplete(obj));
            })
            .catch(error => {            
                dispatch(configActions.handleApplicationAPIError(error));               
            });
    };
};