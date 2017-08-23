import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';
import * as configActions from 'Actions/Config/ConfigActions'

/****************** Actions ************************/
/// <summary>
/// Invoke 'FETCH_PERMISSION_SUCCESS' action
/// which will be handled by the appropriate reducer
/// </summary>
export const fetchPermissionComplete = (permissionRecords) => {
    return {
        type: actionTypes.FETCH_PERMISSION_SUCCESS,
        permissionRecords
    }
};

/// <summary>
/// Invoke 'SAVE_PERMISSION_SUCCESS' action
/// which will be handled by the appropriate reducer
/// </summary>
export const savePermissionComplete = (permissionObj) => {
    return {
        type: actionTypes.SAVE_PERMISSION_SUCCESS,
        permissionObj
    }
};


/******************* Helper methods *******************/
/// <summary>
/// Asynchronously retrieve path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const fetchPermissionRecords = () => {
    return (dispatch) => {        
        return Axios.get('/api/userpermission')
            .then(response => {
                dispatch(fetchPermissionComplete(response.data));
            })
            .catch(error => {
                dispatch(configActions.handleApplicationAPIError(error));
            });
    };
};


/// <summary>
/// Asynchronously add/update path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const savePermission = (object) => {
    return (dispatch) => {       
        return Axios.post('/api/userpermission/', object)
            .then(response => {
                dispatch(savePermissionComplete(response.data));               
            })
            .catch(error => {
                dispatch(configActions.handleApplicationAPIError(error));
                throw (error);
            });
    };
};

