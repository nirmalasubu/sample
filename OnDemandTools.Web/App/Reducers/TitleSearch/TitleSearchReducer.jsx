export const TitleSearchReducer = (state = {}, action) => {
    switch (action.type) {
        case 'TITLE_SEARCH_SUCCESS':
            return action.titles;
        case 'CLEAR_TITLE_SUCCESS':
            return action.titles;
        default:
            return state;
    }
};