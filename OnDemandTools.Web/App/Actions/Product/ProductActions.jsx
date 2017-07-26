﻿import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';


// Future  we get data through Axios or fetch

export const fetchProductSuccess = (products) => {
    return {
        type: actionTypes.FETCH_PRODUCTS_SUCCESS,
        products
    }
};

export const filterProductSuccess= (filterProduct) => {
    return {
        type: actionTypes.FILTER_PRODUCTS_SUCCESS,
        filterProduct
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

export const getNewProduct = () => {
    return Axios.get('/api/product/newProduct')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};

export const saveProductSuccess = (product) => {
    return {
        type: actionTypes.SAVE_PRODUCT_SUCCESS,
        product
    }
};

export const saveProduct = (model) => {
    return (dispatch) => {        
        return Axios.post('/api/product', model)
            .then(response => {
                dispatch(saveProductSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const deleteProductSuccess = (objectId) => {
    return {
        type: actionTypes.DELETE_PRODUCT_SUCCESS,
        objectId
    }
};

export const deleteProduct = (id) => {
    return (dispatch) => {        
        return Axios.delete('/api/product/'+ id)
            .then(response => {
                dispatch(deleteProductSuccess(id))
            })
            .catch(error => {
                throw (error);
            });
    };
};


