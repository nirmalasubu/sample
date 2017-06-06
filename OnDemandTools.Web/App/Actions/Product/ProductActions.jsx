import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';


// Future  we get data through Axios or fetch

export const fetchProductSuccess = (products) => {
    return {
        type: actionTypes.FETCH_PRODUCTS_SUCCESS,
        products
    }
};

export const fetchProducts = () => {
    return (dispatch) => {
        return Axios.get('/api/product')
            .then(response => {
                dispatch(fetchProductSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};


