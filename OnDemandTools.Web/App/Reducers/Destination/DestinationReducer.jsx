export const DestinationReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_DESTINATIONS_SUCCESS':
            return action.destinations;
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