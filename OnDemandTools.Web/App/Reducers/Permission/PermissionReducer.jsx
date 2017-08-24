import * as actionTypes from 'Actions/ActionTypes';

/// <summary>
/// Reducer definitions for Permissions actions
/// </summary>
export const PermissionReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_PERMISSION_SUCCESS:
            // Return full list of path translation records
            return action.permissionRecords;
        case actionTypes.SAVE_PERMISSION_SUCCESS:            
            return state;            
        default:
            return state;
    }
};