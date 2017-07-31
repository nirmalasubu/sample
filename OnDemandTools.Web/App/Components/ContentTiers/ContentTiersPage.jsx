import React from 'react';
import { connect } from 'react-redux';
import * as contentTierActions from 'Actions/ContentTier/ContentTierActions';
import ContentTierTable from 'Components/ContentTiers/ContentTierTable';
import ContentTierFilter from 'Components/ContentTiers/ContentTierFilter';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    // Retrieve filtered list of ContentTiers based on filterValue like contentTier name and destination
    // which are defined in Redux store
    var filteredContentTierValues = getFilterVal(store.contentTiers, store.filterContentTier);
    return {
        contentTiers: store.contentTiers,
        filteredContentTiers: (filteredContentTierValues != undefined ? filteredContentTierValues : store.destinations)
    };
})
class ContentTiersPage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            stateQueue: [],

            filterValue: {
                contentTierName: "",
                destination: "",

            },

            columns: [{ "label": "Name", "dataField": "name", "sort": true },
            { "label": "Destinations", "dataField": "destinations", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "name"
        }
    }

    ///<summary>
    // Function to handle filtering of ContentTier data. This handler
    // is used within the ContentTier filter sub component. Filtering is currently
    // supported for ContentTier name and destination
    /// </summary>
    handleFilterUpdate(filtersValue, type) {
        var stateFilterValue = this.state.filterValue;
        if (type == "CN")
            stateFilterValue.contentTierName = filtersValue;

        if (type == "DS")
            stateFilterValue.destination = filtersValue;

        if (type == "CL") {
            stateFilterValue.contentTierName = "";
            stateFilterValue.destination = "";
        }
        this.setState({
            filterValue: stateFilterValue
        });
        this.props.dispatch(contentTierActions.filterContentTierSuccess(this.state.filterValue));
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(contentTierActions.filterContentTierSuccess(this.state.filterValue));
        this.props.dispatch(contentTierActions.fetchContentTiers());

        document.title = "ODT - ContentTiers";
    }

    render() {
        return (
            <div>
                <PageHeader pageName="ContentTiers" />
                <ContentTierFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <ContentTierTable RowData={this.props.filteredContentTiers} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

///<summary>
// The goal of this function is to filter 'ContentTiers' (which is stored in Redux store)
// based on user provided filter criteria and return the refined 'contentTiers' list.
// If no filter criteria is provided then return the full 'contentTiers' list
///</summary>
const getFilterVal = (contentTiers, filterVal) => {

    if(filterVal==undefined) return;

    if (filterVal.contentTierName != undefined) {
        var contentTierName = filterVal.contentTierName.toLowerCase();
        var destination = filterVal.destination.toLowerCase();
        return (contentTiers.filter(obj => (contentTierName != "" ? obj.name.toLowerCase().indexOf(contentTierName) != -1 : true)
            && (destination != "" ? matchDestinations(obj.destinations, destination) : true)
        ));
    }
    else
        return contentTiers;
};

///<summary>
// returns  true  if any of the search value  destination matches destination list 
///</summary>
const matchDestinations = (objDestinations, destination) => {
    var destinationNames = [];
    objDestinations.map(function (item) {
        destinationNames.push(item.name);
    });
    var flag = false;
    destinationNames.map(function (name) {
        if (name.toLowerCase().indexOf(destination) != -1) {
            flag = true;
        }
    });
    return flag;
};
export default ContentTiersPage;