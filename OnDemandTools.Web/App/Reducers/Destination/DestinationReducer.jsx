export const DestinationReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_DESTINATIONS_SUCCESS':
            return action.destinations;
        default:
            return state;
    }
};