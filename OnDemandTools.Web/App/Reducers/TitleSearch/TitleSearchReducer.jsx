export const TitleSearchReducer = (state = [], action) => {
    switch (action.type) {
        case 'TITLE_SEARCH_SUCCESS':
            return action;
        default:
            return state;
    }
};