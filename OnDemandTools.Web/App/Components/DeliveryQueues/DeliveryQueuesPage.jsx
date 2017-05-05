import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';
import DeliveryQueueFilter from 'Components/DeliveryQueues/DeliveryQueueFilter';
import DeliveryQueueTable from 'Components/DeliveryQueues/DeliveryQueueTable';
import PageHeader from 'Components/Common/PageHeader';


class Queue extends React.Component {

    constructor(props) {
        super(props);
       
        this.state = {
            stateQueue: [],

            filterValue: {
                queueName: "",
                contactName: "",
                queueId: ""
            },

            columns: [{ "label": "Queue Name", "dataField": "friendlyName", "sort": true },
            { "label": "Advanced Delivery", "dataField": "hoursOut", "sort": true },
            { "label": "Contact", "dataField": "contactEmailAddress", "sort": true },
            { "label": "Remote Queue", "dataField": "name", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "friendlyName",
            deliveryQueueHub: $.connection.deliveryQueueCountHub,
            deliveryQueueSignalRData: {"queues":[{"friendlyName":"","name":"","messageCount":0,"pendingDeliveryCount":0,"processedDateTime":"0001-01-01T00:00:00"}],
                "jobcount":0,"jobLastRun":""}
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

        if (type == "CL") {
            this.state.filterValue.queueName = "";
            this.state.filterValue.contactName = "";
            this.state.filterValue.queueId = "";
        }

        this.setState({
            filterValue: valuess
        });

        if (this.state.filterValue.queueName != "" || this.state.filterValue.contactName != "" || this.state.filterValue.queueId != "") {
            var friendlyName = this.state.filterValue.queueName.toLowerCase();
            var contactName = this.state.filterValue.contactName.toLowerCase();
            var queueId = this.state.filterValue.queueId.toLowerCase();

            var queueArray = $.grep(this.props.queues, function (v) {
                return ((friendlyName != "" ? v.friendlyName.toLowerCase().indexOf(friendlyName) != -1 : true)
                    && (contactName != "" ? v.contactEmailAddress.toLowerCase().indexOf(contactName) != -1 : true)
                    && (queueId != "" ? v.name.toLowerCase().indexOf(queueId) != -1 : true));
            });
            this.setState({
                stateQueue: queueArray
            });
        }
        else
            this.setState({
                stateQueue: this.props.queues
            });

    }

    //called on the page load
    componentDidMount() {
      
        this.state.deliveryQueueHub.client.GetQueueDeliveryCount = this.GetQueueDeliveryCount.bind(this);
        $.connection.hub.start();

        let promise = this.props.fetchQueue();
        promise.then(newqueue => {
            this.setState({
                stateQueue: this.props.queues,

            })
        });
        document.title = "ODT - Delivery Queues";
    }


    GetQueueDeliveryCount(data){
        this.props.signal(data);
    }

    render() {
        return (
            <div>
               
                <PageHeader pageName="Delivery Queue" />
                <DeliveryQueueFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <DeliveryQueueTable RowData={this.state.stateQueue} ColumnData={this.state.columns} KeyField={this.state.keyField} signalrData={this.props.signalRQueueData} />
            </div>

        )
    }
}

// Maps state from store to props
const mapStateToProps = (state, ownProps) => {
    return {
        queues: state.queues,
        signalRQueueData:state.queueCountData
    }
};

// Maps actions to props
const mapDispatchToProps = (dispatch) => {
    return {
        fetchQueue: () => dispatch(queueActions.fetchQueues()),
        signal: (signalRdata) =>dispatch(queueActions.signalRStart(signalRdata))
    };
};

// Use connect to put them together
export default connect(mapStateToProps, mapDispatchToProps)(Queue);


