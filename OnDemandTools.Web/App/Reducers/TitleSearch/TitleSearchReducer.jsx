import $ from 'jquery';

export const TitleSearchReducer = (state = {}, action) => {
    switch (action.type) {
        case 'TITLE_SEARCH_SUCCESS':
            return action.titles;
        case 'CLEAR_TITLE_SUCCESS':
            return action.titles;
        case 'FILTER_TITLE_SUCCESS':
            var existingData = $.extend(true, {}, state);
            if (action.filter.isTitleType) {
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