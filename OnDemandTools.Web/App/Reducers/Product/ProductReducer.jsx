export const ProductReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_PRODUCTS_SUCCESS':
            return action.products;
        default:
            return state;
    }
};