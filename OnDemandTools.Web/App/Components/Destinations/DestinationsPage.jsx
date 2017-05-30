import React from 'react';
import { connect } from 'react-redux';
import * as destinationActions from 'Actions/Destination/DestinationActions';
import DestinationFilter from 'Components/Destinations/DestinationFilter';
import DestinationTable from 'Components/Destinations/DestinationTable';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';


class Destinations extends React.Component {

    constructor(props) {
        super(props);
       
        this.state = {
            stateQueue: [],

            filterValue: {
                code: "",
                description: "",
                content: ""
            },

            columns: [{ "label": "Code", "dataField": "name", "sort": true },
            { "label": "Description", "dataField": "description", "sort": true },
            { "label": "Content", "dataField": "content", "sort": true }
            ],
            keyField: "name"
        }
    }

    handleFilterUpdate(filtersValue, type) {
        var valuess = this.state.filterValue;
        if (type == "CD")
            valuess.code = filtersValue;

        if (type == "DS")
            valuess.description = filtersValue;

        if (type == "CN")
            valuess.content = filtersValue;

        if (type == "CL") {
            valuess.code = "";
            valuess.description = "";
            valuess.content = "";
        }

        this.setState({
            filterValue: valuess
        });

        this.props.filterDestination(this.state.filterValue);

    }

    //called on the page load
    componentDidMount() {
        this.props.filterDestination(this.state.filterValue);

        this.props.fetchDestination();

        document.title = "ODT - Destinations";
    }

    render() {
        return (
            <div>
               
                <PageHeader pageName="Destinations" />
                <DestinationFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <DestinationTable RowData={this.props.filteredDestinations} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

const getFilterVal = (destinations, filterVal) => {
    console.log("getfilter");
    if(filterVal.code!=undefined)
    {        
        var code = filterVal.code.toLowerCase();
        var description = filterVal.description.toLowerCase();
        var content = filterVal.content;
        
        return (destinations.filter(obj=> ((code != "" ? obj.name.toLowerCase().indexOf(code) != -1 : true)
                && (description != "" ? obj.description.toLowerCase().indexOf(description) != -1 : true)
                && (content != "" ? ((content=="HD"?obj.content.highDefinition:false) || (content=="SD"?obj.content.standardDefinition:false)
                                       || (content=="C3"?obj.content.cx:false) || (content=="NonC3"?obj.content.nonCx:false)) : true)
            )));
    }
    else
        destinations;
};

// Maps state from store to props
const mapStateToProps = (state, ownProps) => {
    
    var arr = getFilterVal(state.destinations, state.filterDestination);
    return {
        destinations: state.destinations,
        filteredDestinations: (arr!=undefined?arr:state.destinations)
    }
};

// Maps actions to props
const mapDispatchToProps = (dispatch) => {
    return {
        fetchDestination: () => dispatch(destinationActions.fetchDestinations()),
        filterDestination: (filterVal) =>dispatch(destinationActions.filterDestinationSuccess(filterVal))
    };
};

// Use connect to put them together
export default connect(mapStateToProps, mapDispatchToProps)(Destinations);


