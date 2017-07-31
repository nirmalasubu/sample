export const ContentTierReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_CONTENT_TIER_SUCCESS':
            return action.contentTiers;
        case 'SAVE_CONTENT_TIER_SUCCESS':
            var contentTierIndex = state.findIndex((obj => obj.id == action.contentTier.id));
            if (contentTierIndex < 0) {                
                //state.push(action.destination);
                return [
                    ...state.filter(obj => obj.id !== action.contentTier.id),
                    Object.assign({}, action.contentTier)
                ]
            }
            else {
                state[contentTierIndex] = action.contentTier;
                return state;
            } 
        case 'FILTER_CONTENT_TIER_SUCCESS':       // Required to obtain  contentTier object state 
            const assignState = Object.assign([], state);
            return assignState;
        case 'DELETE_CONTENT_TIER_SUCCESS':
            const newState = Object.assign([], state); 
            var dIndex = state.findIndex((obj => obj.name == action.name));
            if (dIndex >= 0)
            {
                newState.splice(dIndex,1);
            }
            return newState;
        default:
            return state;
    }
};

    export const FilterContentTierDataReducer = (state = [], action) => {
        switch (action.type) {
            case 'FILTER_CONTENT_TIER_SUCCESS':
                return action.filterContentTier;   // search  criteria for contentTier
            default:
                return state;
        }
};