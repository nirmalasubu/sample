export const CategoryReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_CATEGORIES_SUCCESS':
            return action.categories;
        case 'SAVE_CATEGORIES_SUCCESS':
            var categoryIndex = state.findIndex((obj => obj.id == action.category.id));
            if (categoryIndex < 0) {                
                //state.push(action.destination);
                return [
                    ...state.filter(obj => obj.id !== action.category.id),
                    Object.assign({}, action.category)
                ]
            }
            else {
                state[categoryIndex] = action.category;
                return state;
            } 
        case 'FILTER_CATEGORIES_SUCCESS':       // Required to obtain  category object state 
            const assignState = Object.assign([], state);
            return assignState;
        case 'DELETE_CATEGORY_SUCCESS':
            var dIndex = state.findIndex((obj => obj.name == action.name));
            if (dIndex >= 0)
                state.splice(dIndex,1);
            return state;
        default:
            return state;
    }
};

    export const FilterCategoryDataReducer = (state = [], action) => {
        switch (action.type) {
            case 'FILTER_CATEGORIES_SUCCESS':
                return action.filterCategory;   // search  criteria for category
            default:
                return state;
        }
};