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
export const savePermissionComplete = (permission) => {
    return {
        type: actionTypes.SAVE_PERMISSION_SUCCESS,
        permission
    }
};



/******************* Helper methods *******************/
/// <summary>
/// Asynchronously retrieve path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const fetchPermissionRecords = (type) => {
    return (dispatch) => {        
        return Axios.get('/api/userpermission/' + type)
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

/// <summary>
/// Asynchronously retrieve user contact from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const fetchContactForRecords = (id) => {
        return Axios.get('/api/userpermission/getcontactforbyuserid/' + id)
            .then(response => {
                return (response.data);
            })
            .catch(error => {
                dispatch(configActions.handleApplicationAPIError(error));
            });
   
    
};
export const getNewUserPermission = () => {
    return Axios.get('/api/userpermission/newuserpermission')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};

