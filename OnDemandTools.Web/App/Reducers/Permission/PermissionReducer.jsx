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
            var newState = Object.assign([], state);
            var permissionIndex = newState.findIndex((obj => obj.id == action.permission.id));
            if (permissionIndex < 0) {
                return [
                    ...newState.filter(obj => obj.id !== action.permission.id),
                    Object.assign({}, action.permission)
                ]
            }
            else {
                newState[permissionIndex] = action.permission;
                return newState;
            }
        default:
            return state;
    }
};