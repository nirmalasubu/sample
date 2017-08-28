import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';
import DeliveryQueueFilter from 'Components/DeliveryQueues/DeliveryQueueFilter';
import DeliveryQueueTable from 'Components/DeliveryQueues/DeliveryQueueTable';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

// Parent component for all things related to Queue. Individual features
// and layouts are defined further within sub components
class Queue extends React.Component {

    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);

        this.state = {
            stateQueue: [],

            filterValue: {
                queueName: "",
                contactName: "",
                queueId: "",
                isInActive: false
            },

            columns: [{ "label": "Queue Name", "dataField": "friendlyName", "sort": true },
            { "label": "Advanced Delivery", "dataField": "hoursOut", "sort": false },
            { "label": "Contact", "dataField": "contactEmailAddress", "sort": false },
            { "label": "Remote Queue", "dataField": "name", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "friendlyName",
            deliveryQueueHub: $.connection.deliveryQueueCountHub,
            deliveryQueueSignalRData: {
                "queues": [{ "friendlyName": "", "name": "", "messageCount": 0, "pendingDeliveryCount": 0, "processedDateTime": "0001-01-01T00:00:00" }],
                "jobcount": 0, "jobLastRun": ""
            }
        }
    }

    // Function to handle filtering of queue data. This handler
    // is used within the queue filter sub component. Filtering is currently
    // supported for queue name, contact person, queue remote id and
    // active vs inactive queues. 
    handleFilterUpdate(filtersValue, type) {
        var valuess = this.state.filterValue;

        // Handle queue name filtering
        if (type == "QN")
            valuess.queueName = filtersValue;

        // Handle queue contact name filtering
        if (type == "CN")
            valuess.contactName = filtersValue;

        // Handle queue id filtering
        if (type == "QI")
            valuess.queueId = filtersValue;

        // Handle active vs inactive queue filterin
        if (type == "CB")
            valuess.isInActive = filtersValue;

        // Clear queue filter selections
        if (type == "CL") {
            valuess.queueName = "";
            valuess.contactName = "";
            valuess.queueId = "";
            valuess.isInActive = false;
        }

        // Replace component state with the correct
        // filtering type/values
        this.setState({
            filterValue: valuess
        });
    }

    // Invoked immediately after queue component is mounted.
    componentDidMount() {

        // // Setup communication with SignalR hub/service
        this.state.deliveryQueueHub.client.GetQueueDeliveryCount = this.GetQueueDeliveryCount.bind(this);
        $.connection.hub.start();

        // Dispatch another action to asynchronously fetch full list of queue data
        // from server. Once it is fetched, the data will be stored
        // in redux store
        this.props.fetchQueue();

        // Set page title
        document.title = "ODT - Delivery Queues";
    }

    // Dispatch action to asynchronously communicate with SignalR service
    // to render real time queue stats
    GetQueueDeliveryCount(data) {
        this.props.signal(data);
    }

    // The goal of this function is to filter 'queues' (which is stored in Redux store)
    // based on user provided filter criteria and return the refined 'queues' list.
    // If no filter criteria is provided then return the full 'queues' list
    getFilterVal(queues, filterVal) {
        if (filterVal.queueName != undefined) {
            var friendlyName = filterVal.queueName.toLowerCase();
            var contactName = filterVal.contactName.toLowerCase();
            var queueId = filterVal.queueId.toLowerCase();
            var inActive = filterVal.isInActive;

            return queues.filter(obj => ((friendlyName != "" ? obj.friendlyName.toLowerCase().indexOf(friendlyName) != -1 : true)
                && (contactName != "" ? obj.contactEmailAddress.toLowerCase().indexOf(contactName) != -1 : true)
                && (queueId != "" ? obj.name.toLowerCase().indexOf(queueId) != -1 : true)
                && (!inActive ? obj.active : true)));
        }
        else
            queues;
    };

    render() {

        var filteredQueues = this.getFilterVal(this.props.queues, this.state.filterValue);
        return (
            <div>
                <PageHeader pageName="Delivery Queues" />
                <DeliveryQueueFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <DeliveryQueueTable RowData={filteredQueues} ColumnData={this.state.columns} KeyField={this.state.keyField} signalrData={this.props.signalRQueueData} />
            </div>
        )
    }
}

// Define props for component that are inferred
// directly or indirectly from state variables. Note that these states
// are inherited from Redux store. 
const mapStateToProps = (state, ownProps) => {
    return {
        signalRQueueData: state.queueCountData,
        queues: state.queues
    }
};

// Define props for component that are linked to the dispatch feature
// used via Redux. A call to any of these props will invoke the corresponding
// dispatcher
const mapDispatchToProps = (dispatch) => {
    return {
        fetchQueue: () => dispatch(queueActions.fetchQueues()),
        signal: (signalRdata) => dispatch(queueActions.signalRStart(signalRdata))
    };
};

// The idea here is that before any dispatch occurs, all of the logic within
// 'mapStateToProps' is executed. This is to faciliate mapping of correct
// props within this component to the correct states defined either within the component
// or globally within the Redux store
export default connect(mapStateToProps, mapDispatchToProps)(Queue);


