import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import $ from 'jquery';
import * as configActions from 'Actions/Config/ConfigActions'

// Future  we get data through Axios or fetch
const emptyTitles = {
    titles: [],
    titlesBackup: [],
    titleTypeFilterParameters: [],
    seriesFilterParameters: []
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

export const filterTitleSuccess = (filter) => {
    return {
        type: actionTypes.FILTER_TITLE_SUCCESS,
        filter
    }
};

export const titleSearch = (param) => {
    return (dispatch) => {
        return Axios.get('/api/titles/search/' + param)
            .then(response => {
                response.data.titlesBackup = $.extend(true, [], response.data.titles);
                dispatch(titleSearchSuccess(response.data))
            })
            .catch(error => {
                dispatch(configActions.handleApplicationAPIError(error));
                throw (error);
            });
    };
};

export const clearTitles = () => {
    return (dispatch) => {
        dispatch(clearTitleSuccess(emptyTitles));
    };
};

export const filterTitles = (filter) => {
    return (dispatch) => {
        dispatch(filterTitleSuccess(filter));
    };
};

export const searchByTitleIds = (titleIds) => {
    var queryString = "?";

    for (var i = 0; i < titleIds.length; i++) {
        queryString += "ids=" + titleIds[i] + "&";
    }

    return Axios.get('/api/titles/searchByTitleIds/' + queryString)
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            dispatch(configActions.handleApplicationAPIError(error));
            throw (error);
        });
};