import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';

// Future  we get data through Axios or fetch
const emptyTitles = {
    titles:[],
    titlesBackup:[],
    titleTypeFilterParameters:[],
    seriesFilterParameters:[]
};

export const titleSearchSuccess = (titles) => {
    return {
        type: actionTypes.TITLE_SEARCH_SUCCESS,
        titles
    }
};

export const clearTitleSuccess = (titles) => {
    return {
        type: actionTypes.CLEAR_TITLE_SUCCESS,
        titles
    }
};

export const titleSearch = (param) => {
    return (dispatch) => {
        return Axios.get('/api/titles/search/' + param)
            .then(response => {
                response.data.titlesBackup= $.extend(true, [], response.data.titles);
                dispatch(titleSearchSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const clearTitles = () => {
    return (dispatch) => {        
       dispatch(clearTitleSuccess(emptyTitles));
    };
};