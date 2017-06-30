export const CategoryReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_CATEGORIES_SUCCESS':
            return action.categories;
        case 'FILTER_CATEGORIES_SUCCESS':       // Required to obtain  category object state 
            const assignState = Object.assign([], state);
            return assignState;
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