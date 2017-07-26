
/// <summary>
/// Reducer definitions for path translation actions
/// </summary>
export const PathTranslationReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_PATHTRANSLATION_SUCCESS':
            return;
        case 'SAVE_PATHTRANSLATION_SUCCESS':
            return;
        case 'DELETE_PATHTRANSLATION_SUCCESS':
            return;        
        default:
            return state;
    }
};