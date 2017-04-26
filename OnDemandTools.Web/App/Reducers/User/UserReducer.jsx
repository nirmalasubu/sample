export const UserReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_USERDETAILS_SUCCESS':
            return action.user;
        default:
            return state;
    }
};