export const ConfigReducer = (state = {}, action) => {
    switch (action.type) {
        case 'FETCH_CONFIG_SUCCESS':
            return action.config;
        default:
            return state;
    }
};