export const StatusReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_STATUS_SUCCESS':
            return action.statuses;
        default:
            return state;
    }
};