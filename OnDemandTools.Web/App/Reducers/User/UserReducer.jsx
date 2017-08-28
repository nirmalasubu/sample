import * as actionTypes from 'Actions/ActionTypes';

export const UserReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_USERDETAILS_SUCCESS:
            return action.user;
        default:
            return state;
    }
};