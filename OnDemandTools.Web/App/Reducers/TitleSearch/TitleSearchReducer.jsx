﻿import $ from 'jquery';
import * as actionTypes from 'Actions/ActionTypes';

export const TitleSearchReducer = (state = {}, action) => {
    switch (action.type) {
        case actionTypes.TITLE_SEARCH_SUCCESS:
            return action.titles;
        case actionTypes.CLEAR_TITLE_SUCCESS:
            return action.titles;
        case actionTypes.FILTER_TITLE_SUCCESS:
            var existingData = $.extend(true, {}, state);
            if (action.filter.value == "") {
                existingData.titles = $.extend(true, [], existingData.titlesBackup);
            }
            else if (action.filter.isTitleType) {
                var filteredData = $.grep(existingData.titlesBackup, function (e) { return e.titleType.name == action.filter.value; });
                existingData.titles = filteredData;
            }
            else {
                var filteredData = $.grep(existingData.titlesBackup, function (e) { return e.seriesTitleNameSortable == action.filter.value; });
                existingData.titles = filteredData;
            }
            return existingData;
        default:
            return state;
    }
};