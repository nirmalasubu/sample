export const StatusReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_STATUS_SUCCESS':
            return action.statuses;
        case 'FILTER_STATUS_SUCCESS':       // Required to obtain status object state 
            const assignState = Object.assign([], state);
            return assignState;
        case 'DELETE_STATUS_SUCCESS':
            const newState = Object.assign([], state);
            const indexOfStatus = state.findIndex(obj => {
                return obj.id == action.objectId
            })
            newState.splice(indexOfStatus, 1);
            return newState;
        default:
            return state;
    }
};


export const FilterStatusDataReducer = (state = [], action) => {
    switch (action.type) {
        case 'FILTER_STATUS_SUCCESS':
            return action.filterStatus;   // search  criteria for status
        default:
            return state;
    }
};