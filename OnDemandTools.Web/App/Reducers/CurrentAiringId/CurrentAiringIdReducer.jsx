export const CurrentAiringIdReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_AIRINGID_SUCCESS':
            return action.currentAiringIds;
        case 'FILTER_AIRINGID_SUCCESS':       // Required to obtain current airing id object state 
            const assignState = Object.assign([], state);
            return assignState;
        case 'DELETE_AIRINGID_SUCCESS':
            const newState = Object.assign([], state);
            const indexOfObject = state.findIndex(obj => {
                return obj.id == action.objectId
            })
            newState.splice(indexOfObject, 1);
            return newState;
        default:
            return state;
    }
};

export const FilterAiringIdDataReducer = (state = [], action) => {
    switch (action.type) {
        case 'FILTER_AIRINGID_SUCCESS':
            return action.filterDistribution;   // search  criteria for airing id
        default:
            return state;
    }
};