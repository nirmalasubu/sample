import React from 'react';
import { connect } from 'react-redux';
import * as destinationActions from 'Actions/Destination/DestinationActions';
//import DeliveryQueueFilter from 'Components/DeliveryQueues/DeliveryQueueFilter';
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
                content: {
                    hd: true,
                    sd: true,
                    cx: true,
                    nonCx: true
                }
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
        if (type == "QN")
            valuess.queueName = filtersValue;

        if (type == "CN")
            valuess.contactName = filtersValue;

        if (type == "QI")
            valuess.queueId = filtersValue;

        if (type == "CB")
            valuess.isInActive = filtersValue;

        if (type == "CL") {
            valuess.queueName = "";
            valuess.contactName = "";
            valuess.queueId = "";
            valuess.isInActive = true;            
        }

        this.setState({
            filterValue: valuess
        });

        this.props.filterQueue(this.state.filterValue);

    }

    //called on the page load
    componentDidMount() {

        //this.props.filterQueue(this.state.filterValue);

        this.props.fetchDestination();

        document.title = "ODT - Destinations";
    }

    render() {
        return (
            <div>
               
                <PageHeader pageName="Destinations" />
                <DestinationTable RowData={this.props.destinations} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>

        )
    }
}

const getFilterVal = (queues, filterVal) => {
    if(filterVal.queueName!=undefined)
    {
        var friendlyName = filterVal.queueName.toLowerCase();
        var contactName = filterVal.contactName.toLowerCase();
        var queueId = filterVal.queueId.toLowerCase();
        var inActive = filterVal.isInActive;
        
        return queues.filter(obj=> ((friendlyName != "" ? obj.friendlyName.toLowerCase().indexOf(friendlyName) != -1 : true)
                && (contactName != "" ? obj.contactEmailAddress.toLowerCase().indexOf(contactName) != -1 : true)
                && (queueId != "" ? obj.name.toLowerCase().indexOf(queueId) != -1 : true) 
                && (!inActive?obj.active:true)));
    }
    else
        queues;
};

// Maps state from store to props
const mapStateToProps = (state, ownProps) => {
    //var arr = getFilterVal(state.queues, state.filterValue);
    return {
        destinations: state.destinations,
        //filteredQueues: (arr!=undefined?arr:state.queues)
    }
};

// Maps actions to props
const mapDispatchToProps = (dispatch) => {
    return {
        fetchDestination: () => dispatch(destinationActions.fetchDestinations()),
        //filterQueue: (filterVal) =>dispatch(queueActions.filterQueues(filterVal))
    };
};

// Use connect to put them together
export default connect(mapStateToProps, mapDispatchToProps)(Destinations);


