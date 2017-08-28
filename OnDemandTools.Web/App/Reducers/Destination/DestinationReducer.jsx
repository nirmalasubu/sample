export const DestinationReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_DESTINATIONS_SUCCESS':
            return action.destinations;
        case 'SAVE_DESTINATION_SUCCESS':
            var destinationIndex = state.findIndex((obj => obj.id == action.destination.id));
            if (destinationIndex < 0) {                
                //state.push(action.destination);
                return [
                    ...state.filter(obj => obj.id !== action.destination.id),
                    Object.assign({}, action.destination)
                ]
            }
            else {
                state[destinationIndex] = action.destination;
                return state;
            }            
        case 'DELETE_DESTINATION_SUCCESS':
            const newState = Object.assign([], state);
            const indexOfDestination = state.findIndex(obj => {
                return obj.id == action.objectId
            })
            newState.splice(indexOfDestination, 1);
            return newState;
        case 'FILTER_DESTINATION_SUCCESS':
            const assignState = Object.assign([], state);
            return assignState;
        default:
            return state;
    }
};