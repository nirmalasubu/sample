import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';
import DeliveryQueueFilter from 'Components/DeliveryQueues/DeliveryQueueFilter';
import DeliveryQueueTable from 'Components/DeliveryQueues/DeliveryQueueTable';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';


class Queue extends React.Component {

    constructor(props) {
        super(props);
       
        this.state = {
            stateQueue: [],

            filterValue: {
                queueName: "",
                contactName: "",
                queueId: "",
                isInActive: true
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

        this.setState({
            stateQueue: this.props.filteredQueues,
        });

    }

    //called on the page load
    componentDidMount() {
      
        this.state.deliveryQueueHub.client.GetQueueDeliveryCount = this.GetQueueDeliveryCount.bind(this);
        $.connection.hub.start();

        this.props.filterQueue(this.state.filterValue);

        let promise = this.props.fetchQueue();
        promise.then(newqueue => {
            this.setState({
                stateQueue: this.props.filteredQueues
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
                <DeliveryQueueTable RowData={this.props.filteredQueues} ColumnData={this.state.columns} KeyField={this.state.keyField} signalrData={this.props.signalRQueueData} />
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
    var arr = getFilterVal(state.queues, state.filterValue);
    return {
        queues: state.queues,
        signalRQueueData:state.queueCountData,
        filteredQueues: (arr!=undefined?arr:state.queues)
    }
};

// Maps actions to props
const mapDispatchToProps = (dispatch) => {
    return {
        fetchQueue: () => dispatch(queueActions.fetchQueues()),
        signal: (signalRdata) =>dispatch(queueActions.signalRStart(signalRdata)),
        filterQueue: (filterVal) =>dispatch(queueActions.filterQueues(filterVal))
    };
};

// Use connect to put them together
export default connect(mapStateToProps, mapDispatchToProps)(Queue);


