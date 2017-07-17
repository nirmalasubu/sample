export const ProductReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_PRODUCTS_SUCCESS':
            return action.products;
        case 'FILTER_PRODUCTS_SUCCESS':       // Required to obtain product object state 
            const assignState = Object.assign([], state);
            return assignState;
        default:
            return state;
    }
};

export const FilterProductDataReducer = (state = [], action) => {
    switch (action.type) {
        case 'FILTER_PRODUCTS_SUCCESS':
            return action.filterProduct;   // search  criteria for product
        default:
            return state;
    }
};