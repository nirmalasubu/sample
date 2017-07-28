﻿import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';


export const filterCategorySuccess= (filterCategory) => {
    return {
        type: actionTypes.FILTER_CATEGORIES_SUCCESS,
        filterCategory
    }
};

export const fetchCategorySuccess = (categories) => {
    return {
        type: actionTypes.FETCH_CATEGORIES_SUCCESS,
        categories
    }
};

export const saveCategorySuccess = (category) => {
    return {
        type: actionTypes.SAVE_CATEGORIES_SUCCESS,
        category
    }
};

export const fetchCategories = () => {
    return (dispatch) => {
        return Axios.get('/api/category')
            .then(response => {
                dispatch(fetchCategorySuccess(response.data)),
                dispatch(timerSuccess())
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const saveCategory = (model) => {
    return (dispatch) => {        
        return Axios.post('/api/category', model)
            .then(response => {
                dispatch(saveCategorySuccess(response.data)),
                dispatch(timerSuccess())
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const getNewCategory = () => {
    timerSuccess();
    return Axios.get('/api/category/newCategory')
        .then(response => {
            
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};

export const deleteCategorySuccess = (name) => {
    return {
        type: actionTypes.DELETE_CATEGORY_SUCCESS,
        name
    }
};

export const timerSuccess = () => {
    return {
        type: "TIMER_SUCCESS"
        
    }
};

export const deleteCategory = (name) => {
    return (dispatch) => {        
        return Axios.delete('/api/category/'+ name)
            .then(response => {
                dispatch(deleteCategorySuccess(name)),
                dispatch(timerSuccess())  // need to update the store when server action happens

            })
            .catch(error => {
                throw (error);
            });
    };
};

