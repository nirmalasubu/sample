import React from 'react';
import { connect } from 'react-redux';
import * as destinationActions from 'Actions/Destination/DestinationActions';
import DestinationFilter from 'Components/Destinations/DestinationFilter';
import DestinationTable from 'Components/Destinations/DestinationTable';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    var arr = getFilterVal(store.destinations, store.filterDestination);
    return {
        destinations: store.destinations,
        filteredDestinations: (arr!=undefined?arr:store.destinations)
    };
})

class DestinationPage extends React.Component {

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
            { "label": "Content", "dataField": "content", "sort": true },
            { "label": "Actions", "dataField": "name", "sort": false }
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

        this.props.dispatch(destinationActions.filterDestinations(this.state.filterValue));
        this.props.dispatch(destinationActions.fetchDestinations());

    }

    //called on the page load
    componentDidMount() {

        this.props.dispatch(destinationActions.filterDestinations(this.state.filterValue));

        this.props.dispatch(destinationActions.fetchDestinations());

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
    console.log(filterVal);
    if(filterVal.code!=undefined)
    {        
        var code = filterVal.code.toLowerCase();
        var description = filterVal.description.toLowerCase();
        var content = filterVal.content;
        
        return (destinations.filter(obj=> ((code != "" ? obj.name.toLowerCase().indexOf(code) != -1 : true)
                && (description != "" ? obj.description.toLowerCase().indexOf(description) != -1 : true)
                && (obj.content!=null?(content != "" ? ((content=="HD"?obj.content.highDefinition:false) || (content=="SD"?obj.content.standardDefinition:false)
                                       || (content=="C3"?obj.content.cx:false) || (content=="NonC3"?obj.content.nonCx:false)) : true):false)
            )));
    }
    else
        return destinations;
};


export default DestinationPage;


