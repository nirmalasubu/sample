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
            console.log("reducer error"+action.error)
            //TODO: add code to report error back to server   
           return action.error;
        default:
            return state;
    }
};


/// <summary>
/// Timer for session time out handling
/// from within whole application
/// </sumamry>
export const TimerReducer = (state = "", action) => {
    switch (action.type) {
        case actionTypes.TIMER_SUCCESS:
            var currentdate = new Date(); 
            const newState = currentdate 
            return newState
        default:
            return state;
    }
};