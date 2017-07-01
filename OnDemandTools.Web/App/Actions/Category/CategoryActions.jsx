import * as actionTypes from 'Actions/ActionTypes';
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

export const fetchCategories = () => {
    return (dispatch) => {
        return Axios.get('/api/category')
            .then(response => {
                dispatch(fetchCategorySuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const deleteCategorySuccess = (name) => {
    return {
        type: actionTypes.DELETE_CATEGORY_SUCCESS,
        name
    }
};

export const deleteCategory = (name) => {
    return (dispatch) => {        
        return Axios.delete('/api/category/'+ name)
            .then(response => {
                dispatch(deleteCategorySuccess(name))
            })
            .catch(error => {
                throw (error);
            });
    };
};

