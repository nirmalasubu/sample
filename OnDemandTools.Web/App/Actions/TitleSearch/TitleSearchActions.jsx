import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';

export const titleSearchSuccess = (titles) => {
    return {
        type: actionTypes.TITLE_SEARCH_SUCCESS,
        titles
    }
};

export const titleSearch = (param) => {
    return (dispatch) => {
        return Axios.get('/api/titles/search/' + param)
            .then(response => {
                dispatch(titleSearchSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};