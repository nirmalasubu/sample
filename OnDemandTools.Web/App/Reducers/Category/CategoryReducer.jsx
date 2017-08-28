import * as actionTypes from 'Actions/ActionTypes';

export const CategoryReducer = (state = [], action) => {
    switch (action.type) {
        case actionTypes.FETCH_CATEGORIES_SUCCESS:
            return action.categories;
        case actionTypes.SAVE_CATEGORIES_SUCCESS:
            var updatedCategory = action.category; 
            updatedCategory.clicked = true;  // make the property to true .So it updates the title details from flow.
            var categoryIndex = state.findIndex((obj => obj.id == updatedCategory.id));
            if (categoryIndex < 0) {   // New category is created 
                return [
                    ...state.filter(obj => obj.id !== updatedCategory.id),
                    Object.assign({}, updatedCategory)
                ]
            }
            else {
                var newState = Object.assign([], state);   // always assign to new state otherwise returns old state by default.
                newState[categoryIndex] = updatedCategory;
                return newState;
            } 
        case actionTypes.FILTER_CATEGORIES_SUCCESS:       // Required to obtain  category object state 
            const assignState = Object.assign([], state);
            return assignState;
        case actionTypes.CATEGORY_EXPAND_ROW_CLICK_SUCCESS:  // while the row is expanded and clicked is true then to make title details  to be  fetched from flow .
        const categorys = Object.assign([], state);
            for (var i = 0; i < categorys.length; i++) {
                if (categorys[i].id == action.id) {
                    categorys[i].clicked = true;
                }
            }
            return categorys;
        case actionTypes.DELETE_CATEGORY_SUCCESS:
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