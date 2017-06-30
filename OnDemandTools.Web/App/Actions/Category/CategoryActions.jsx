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


