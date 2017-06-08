﻿export const DestinationReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_DESTINATIONS_SUCCESS':
            return action.destinations;
        case 'DELETE_DESTINATION_SUCCESS':
            var dIndex = state.findIndex((obj => obj.id == action.objectId));
            if (dIndex >= 0)
                state.splice(dIndex,1);
            return state;
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