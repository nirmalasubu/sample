export const ProductReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_PRODUCTS_SUCCESS':
            return action.products;
        case 'SAVE_PRODUCT_SUCCESS':
            var productIndex = state.findIndex((obj => obj.id == action.product.id));
            if (productIndex < 0) { 
                return [
                    ...state.filter(obj => obj.id !== action.product.id),
                    Object.assign({}, action.product)
                ]
            }
            else {
                state[productIndex] = action.product;
                return state;
            }
        case 'DELETE_PRODUCT_SUCCESS':
            const newState = Object.assign([], state);
            const indexOfProduct = state.findIndex(obj => {
                return obj.id == action.objectId
            })
            newState.splice(indexOfProduct, 1);
            return newState;
        default:
            return state;
    }
};