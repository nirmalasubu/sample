import * as actionTypes from 'Actions/ActionTypes';


/// <summary>
/// Reducer definitions for path translation actions
/// </summary>
export const PathTranslationReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_PATHTRANSLATION_SUCCESS:
            console.log(action.pathTranslationRecords);
            return action.pathTranslationRecords;
        case actionTypes.SAVE_PATHTRANSLATION_SUCCESS:
            return;
        case actionTypes.DELETE_PATHTRANSLATION_SUCCESS: 
            // remove the deleted path translation and return new state           
            const newState = Object.assign([], state);
            const indexOfPathTranslation = state.findIndex(obj => {
                return obj.id == action.pathTranslationObjId
            })
            newState.splice(indexOfPathTranslation, 1);         
            return newState;           
        case actionTypes.DELETE_PATHTRANSLATION_SUCCESS:
            return;
        default:
            return state;
    }
};