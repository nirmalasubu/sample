import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';
import DeliveryQueueFilter from 'Components/DeliveryQueues/DeliveryQueueFilter';
import DeliveryQueueTable from 'Components/DeliveryQueues/DeliveryQueueTable';
import PageHeader from 'Components/Common/PageHeader';
import $ from 'jquery';


class Queue extends React.Component {

    constructor(props) {
        super(props);
        // this.sortColumn = this.sortColumn.bind(this);
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
            { "label": "Actions", "dataField": "", "sort": false }
            ],
            keyField: "friendlyName",
            chatHubs: $.connection.myChatHub,
            helloMesasage: []

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

        let promise = this.props.fetchQueue();
        promise.then(newqueue => {
            this.setState({
                stateQueue: this.props.queues,

            })
        });

        this.state.chatHubs.client.sayHello = this.sayHello.bind(this);
        let chat = this.state.chatHubs;
        $.connection.hub.start().done(function () {          
            chat.server.hello()
        });

        document.title = "ODT - Delivery Queues";
    }

    sayHello(msg){
        var msgs = this.state.helloMesasage;
        msgs.push(msg);
        this.setState({
            helloMessage: msgs
        });
    }

    render() {
        return (
            <div>
                 <div>
          {this.state.helloMesasage.map((item, index) => {
              return (
                <div key={index}>
                  <p> { item } </p>
                </div>
              );
    })}
          </div>
                <PageHeader pageName="Delivery Queue" />
                <DeliveryQueueFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <DeliveryQueueTable RowData={this.state.stateQueue} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>

        )
    }
}


// Maps state from store to props
const mapStateToProps = (state, ownProps) => {
    return {
        // You can now say this.props.queues

        queues: state.queues
    }
};

// Maps actions to props
const mapDispatchToProps = (dispatch) => {

    return {
        fetchQueue: () => dispatch(queueActions.fetchQueues())
    };
};

// Use connect to put them together
export default connect(mapStateToProps, mapDispatchToProps)(Queue);


