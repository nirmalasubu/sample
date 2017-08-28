import * as actionTypes from 'Actions/ActionTypes';

export const CurrentAiringIdReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_AIRINGID_SUCCESS:
            return action.currentAiringIds;
        case actionTypes.FILTER_AIRINGID_SUCCESS:       // Required to obtain current airing id object state 
            const assignState = Object.assign([], state);
            return assignState;
        case actionTypes.SAVE_AIRINGID_SUCCESS:            
            var elementIndex = state.findIndex((obj => obj.id == action.currentAiringId.id));
            if (elementIndex < 0) { 
                return [
                    ...state.filter(obj => obj.id !== action.currentAiringId.id),
                    Object.assign({}, action.currentAiringId)
                ]
            }
            else {
                const newState = Object.assign([], state);
                newState[elementIndex] = action.currentAiringId;
                return newState;
            }
        case actionTypes.DELETE_AIRINGID_SUCCESS:
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