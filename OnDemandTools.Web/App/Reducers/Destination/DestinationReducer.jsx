export const DestinationReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_DESTINATIONS_SUCCESS':
            return action.destinations;
        case 'SAVE_DESTINATION_SUCCESS':
            var destinationIndex = state.findIndex((obj => obj.id == action.destination.id));
            if (destinationIndex < 0) {                
                state.push(action.destination);
            }
            else {
                state[destinationIndex] = action.destination;
            }
            return state;
        case 'DELETE_DESTINATION_SUCCESS':
            const newState = Object.assign([], state);
            const indexOfDestination = state.findIndex(obj => {
                return obj.id == action.objectId
            })
            newState.splice(indexOfDestination, 1);
            return newState;
        default:
            return state;
    }
};

export const FilterDestinationDataReducer = (state = [], action) => {
    switch (action.type) {
        case 'FILTER_DESTINATION_SUCCESS':
            return action.filterDestination;
        default:
            return state;
    }
};