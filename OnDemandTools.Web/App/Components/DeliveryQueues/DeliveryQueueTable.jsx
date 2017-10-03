import React from 'react';
import Moment from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import ResendPurgeModal from 'Components/DeliveryQueues/ResendPurgeModal';
import ResetByDateRange from 'Components/DeliveryQueues/QueueResetByDateRange';
import NotificationHistory from 'Components/DeliveryQueues/QueueNotificationHistory';
import DeliveryQueueAddEdit from 'Components/DeliveryQueues/DeliveryQueueAddEdit';
import RemoveQueueModal from 'Components/DeliveryQueues/RemoveQueueModal';
import { getNewQueue } from 'Actions/DeliveryQueue/DeliveryQueueActions';
require('react-bootstrap-table/css/react-bootstrap-table.css');

@connect((store) => {
    return {
        user: store.user
    };
})
// Sub component used within Delivery queues page to display
// detailed queue information 
class DeliveryQueueTable extends React.Component {

    // Define default component state information. 
    constructor(props) {
        super(props);

        this.state = {
            showModal: false,
            newQueueModel: {},
            showDateRangeResetModel: false,
            showNotificationHistoryModel: false,
            showAddEditModel: false,
            showDeleteModal: false,
            queueDetails: "",
            modalActionType: "",
            modalPurgeMessage: "You are about to purge all notifications to the <queue name> queue. Do you wish to continue?",
            modalResendMessage: "If you continue, <queue name> Queue will be reset and any notifications matching your criteria will be delivered again. Do you wish to continue?",
            modalClearMessage: "You are about to clear all undelivered notifications to the <queue name> queue. Do you wish to continue?",
            options: {
                defaultSortName: 'friendlyName',
                defaultSortOrder: 'asc',
                sizePerPageList: [{
                    text: '10 ', value: 10
                }, {
                    text: '25 ', value: 25
                }, {
                    text: '50 ', value: 50
                },
                {
                    text: 'All ', value: 10000000
                }],
                onSortChange: this.onSortChange.bind(this)
            },

        }
    }

    // Invoked immediately after queue component is mounted.
    componentDidMount() {

        // Asychrnously retrieve an empty queue model
        let promise = getNewQueue();
        promise.then(message => {
            this.setState({
                newQueueModel: message
            });
        }).catch(error => {
            this.setState({
                newQueueModel: {}
            });
        });
    }


    ///<summary>
    /// on clicking sort arrow in any page of the table should take to the First page in the pagination.
    ///</summary>
    onSortChange() {

        const sizePerPage = this.refs.deliveryQueueTable.state.sizePerPage;
        this.refs.deliveryQueueTable.handlePaginationData(1, sizePerPage);
    }

    openCreateNewQueueModel() {
        this.setState({ showAddEditModel: true, queueDetails: this.state.newQueueModel });
    }

    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, queueDetails: val });
    }

    closeAddEditModel() {
        this.setState({ showAddEditModel: false });
    }

    openDateRangeResetModel(val) {
        this.setState({ showDateRangeResetModel: true, queueDetails: val });
    }

    closeDateRangeResetModel() {
        this.setState({ showDateRangeResetModel: false });
    }

    openNotificationHistoryModel(val) {
        this.setState({ showNotificationHistoryModel: true, queueDetails: val });
    }

    closeNotificationHistoryModel() {
        this.setState({ showNotificationHistoryModel: false });
    }

    close() {
        this.setState({ showModal: false, queueDetails: "" });
    }

    open(val, type) {
        this.setState({ showModal: true, queueDetails: val, modalActionType: type });
    }

    openDeleteModel(val) {
        this.setState({ showDeleteModal: true, queueDetails: val });
    }

    closeDeleteModel() {
        this.setState({ showDeleteModal: false });
    }

    // Format for displaying the action column details within grid
    actionFormat(val, rowData) {
        var deleteQueueButton = null;

        if (this.props.permissions.canDelete) {
            deleteQueueButton = <button class="btn-link" title="Delete Delivery Queue" onClick={(event) => this.openDeleteModel(rowData, event)}>
                <i class="fa fa-trash"></i>
            </button>
        }

        if (this.props.permissions.canAddOrEdit) {
            return (<div>

                <button class="btn-link" title="Edit Delivery Queue" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Notification History" onClick={(event) => this.openNotificationHistoryModel(rowData, event)}>
                    <i class="fa fa-history" aria-hidden="true"></i>
                </button>

                <button class="btn-link" disabled={!rowData.active} title="Query by Date Range" onClick={(event) => this.openDateRangeResetModel(rowData, event)}>
                    <i class="fa fa-calendar" aria-hidden="true"></i>
                </button>
                {deleteQueueButton}
            </div>)
        }

        return (<div>

            <button class="btn-link" title="View Delivery Queue" onClick={(event) => this.openAddEditModel(rowData, event)} >
                <i class="fa fa-book"></i>
            </button>

            <button class="btn-link" title="Notification History" onClick={(event) => this.openNotificationHistoryModel(rowData, event)}>
                <i class="fa fa-history" aria-hidden="true"></i>
            </button>

        </div>)

    }

    // Format for displaying remote queue column details
    queueFormat(val) {
        var queueItem = $.grep(this.props.RowData, function (v) {
            if (v.name == val) return v;
        });
        var clearButton, purgeButton, resendButton = null

        if (this.props.permissions.canAddOrEdit) {
            clearButton = <button class="btn-xs btn-link" disabled={!queueItem[0].active} title="clear pending deliveries to queue">Clear</button>
            resendButton = <button class="btn-xs btn-link" disabled={!queueItem[0].active} title="Queue will be reset and any notifications matching your criteria will be delivered again" onClick={(event) => this.open(queueItem[0], "resend", event)}>Resend</button>
            purgeButton= <button class="btn-xs btn-link" disabled={!queueItem[0].active} onClick={(event) => this.open(queueItem[0], "purge", event)}>Purge</button>
        }
        if (Object.keys(this.props.signalrData).length === 0 && this.props.signalrData.constructor === Object) {
            return (<div>
                <p>Queue ID: {val}</p>
                <i>Delivery:</i>
                <i class="fa fa-spinner fa-pulse fa-fw margin-bottom"></i>
                {clearButton}
                {resendButton}
                <br />
                <i>Consumption:</i>
                <i class="fa fa-spinner fa-pulse fa-fw margin-bottom"></i>
                {purgeButton}
                <br />
            </div>)
        } else {

            var ItemToRefresh = $.grep(this.props.signalrData.queues, function (v) {
                if (v.name == val) return v;
            });

            var showdate = ItemToRefresh[0].processedDateTime.indexOf('0001') == -1 ? true : false;
            var datetoFormat = Moment(ItemToRefresh[0].processedDateTime).format('lll');
            return (
                <div>
                    <p>Queue ID: {val}</p>
                    <i>Delivery:</i>
                    <span class="badge">{ItemToRefresh[0].pendingDeliveryCount}</span>
                    {clearButton}
                    {resendButton}
                    <br />
                    <i>Consumption:</i>
                    <span class="badge">{ItemToRefresh[0].messageCount}</span>
                    {purgeButton}<br />
                    {showdate ? (<span class="small">(updated: {datetoFormat})</span>) : (null)}
                </div>
            )
        }
    }

    contactFormat(val) {
        return '<p data-toggle="tooltip" title="' + val + '">' + val + '</p>';
    }

    queueNameFormat(val, rowData) {
        return (
            <div>
                {val} {!rowData.active ? (<span class="label label-danger">Inactive</span>) : (null)}
            </div>
        )
    }

    advanceDeliveryFormat(val, rowData) {
        return (
            <div> {val} {val > 1 ? "hours" : "hour"} </div>
        );
    }


    render() {

        var addButton = null;

        if (this.props.permissions.canAdd) {
            addButton = <div>
                <button class="btn-link pull-right addMarginRight" title="New Queue" onClick={(event) => this.openCreateNewQueueModel(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New Queue</span>
                </button>
            </div>;
        }
        var row;
        row = this.props.ColumnData.map(function (item, index) {
            if (item.label == "Queue Name") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.queueNameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Remote Queue") {
                return <TableHeaderColumn width="410px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.queueFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="150px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Contact") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.contactFormat}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn width="100px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.advanceDeliveryFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
        }.bind(this));


        return (
            <div>
                {addButton}
                <BootstrapTable ref="deliveryQueueTable" data={this.props.RowData} striped={true} hover={true} keyField={this.props.KeyField} pagination={true} options={this.state.options}>
                    {row}
                </BootstrapTable>
                <ResendPurgeModal data={this.state} handleClose={this.close.bind(this)} />
                <ResetByDateRange data={this.state} handleClose={this.closeDateRangeResetModel.bind(this)} />
                <NotificationHistory data={this.state} handleClose={this.closeNotificationHistoryModel.bind(this)} />
                <DeliveryQueueAddEdit data={this.state} permissions={this.props.permissions} handleClose={this.closeAddEditModel.bind(this)} />
                <RemoveQueueModal data={this.state} handleClose={this.closeDeleteModel.bind(this)} />
            </div>)
    }

}

export default DeliveryQueueTable;

