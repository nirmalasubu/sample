import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';

export const titleSearchSuccess = (user) => {
    return {
        type: actionTypes.TITLE_SEARCH_SUCCESS,
        user
    }
};

export const titleSearch = (param) => {
    return (dispatch) => {
        return Axios.get('/api/title/' + param)
            .then(response => {
                dispatch(titleSearchSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};