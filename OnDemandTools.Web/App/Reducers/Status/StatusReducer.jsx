﻿import * as actionTypes from 'Actions/ActionTypes';

export const StatusReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_STATUS_SUCCESS:
            return action.statuses;
        case actionTypes.FILTER_STATUS_SUCCESS:       // Required to obtain status object state 
            const assignState = Object.assign([], state);
            return assignState;
        case actionTypes.DELETE_STATUS_SUCCESS:
            const newState = Object.assign([], state);
            const indexOfStatus = state.findIndex(obj => {
                return obj.id == action.objectId
            })
            newState.splice(indexOfStatus, 1);
            return newState;
        case actionTypes.SAVE_STATUS_SUCCESS:
            var statusIndex = state.findIndex((obj => obj.id == action.status.id));
            if (statusIndex < 0) {  
                return [
                    ...state.filter(obj => obj.id !== action.status.id),
                    Object.assign({}, action.status)
                ]
            }
            else {
                state[statusIndex] = action.status;
                return state;
            } 
        default:
            return state;
    }
};