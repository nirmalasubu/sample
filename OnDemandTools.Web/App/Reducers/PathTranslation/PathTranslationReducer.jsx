import * as actionTypes from 'Actions/ActionTypes';


/// <summary>
/// Reducer definitions for path translation actions
/// </summary>
export const PathTranslationReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_PATHTRANSLATION_SUCCESS:             
            console.log(action.pathTranslationObj);                      
            return action.pathTranslationObj;
        case actionTypes.SAVE_PATHTRANSLATION_SUCCESS:
            return;
        case actionTypes.DELETE_PATHTRANSLATION_SUCCESS:
            return;      
        case actionTypes.DELETE_PATHTRANSLATION_SUCCESS:
            return;    
        default:
            return state;
    }
};