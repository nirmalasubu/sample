import * as actionTypes from 'Actions/ActionTypes';

export const ConfigReducer = (state = {}, action) => {
    switch (action.type) {
        case actionTypes.FETCH_CONFIG_SUCCESS:
            return action.config;
        default:
            return state;
    }
};


/// <summary>
/// Error sink reducer for Error actions orginating 
/// from within whole application
/// </sumamry>
export const ApplicationErrorReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.APPLICATION_API_ERROR:  
            //TODO: add code to report error back to server   
           return action.error;
        default:
            return state;
    }
};