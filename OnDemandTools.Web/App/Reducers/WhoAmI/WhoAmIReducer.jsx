import * as actionTypes from 'Actions/ActionTypes';

export const WhoAmIReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_WHOAMI_SUCCESS:
            return action.whoami;
        default:
            return state;
    }
};