﻿import React from 'react';
import Moment from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import ResendPurgeModal from 'Components/DeliveryQueues/ResendPurgeModal';
require('react-bootstrap-table/css/react-bootstrap-table.css');


var ReactTable = React.createClass({    
    
    getInitialState() {
        return { 
            showModal: false, 
            queueDetails : "", 
            modalActionType: "",
            modalPurgeMessage: "You are about to purge all notifications to the <queue name> queue. Do you wish to continue?",
            modalResendMessage: "If you continue, <queue name> Queue will be reset and any notifications matching your criteria will be delivered again. Do you wish to continue?",
        };
    },

    close() {
        this.setState({ showModal: false, queueDetails: "", modalActionType: "" });
    },

    open(val, type) {
        this.setState({ showModal: true, queueDetails: val, modalActionType: type });
    },
    
    actionFormat: function() {
        return '<i class="fa fa-search" aria-hidden="true"></i> <i class="fa fa-calendar" aria-hidden="true"></i>'
    },
    queueFormat: function(val) {
        var queueItem = $.grep(this.props.RowData, function (v) {
            if(v.name==val) return v;
        });
        var ItemToRefresh = $.grep(this.props.signalrData.queues, function (v) {
            if(v.name==val) return v;
        }); 
        var showdate=ItemToRefresh[0].processedDateTime.indexOf('0001')==-1? true:false;
        var datetoFormat=Moment(ItemToRefresh[0].processedDateTime).format('lll');
        return(
                <div>
                    <p>{val}</p>
                    <i>Delivery:{ItemToRefresh[0].pendingDeliveryCount}</i> 
                    <button class="btn-xs btn-link" title="clear pending deliveries to queue">Clear</button>
                    <button class="btn-xs btn-link" title="Queue will be reset and any notifications matching your criteria will be delivered again" onClick={(event) => this.open(queueItem[0], "resend", event)}>Resend</button><br />
                    <i>Consumption:{ItemToRefresh[0].messageCount}</i>
                    <button class="btn-xs btn-link" onClick={(event) => this.open(queueItem[0], "purge", event)}>Purge</button><br />
                    {showdate? (<span class="small">(updated: {datetoFormat})</span>):(null)}
                </div>
            );        
                    },
    contactFormat:function(val)
    {
        return '<p data-toggle="tooltip" title="'+val +'">'+val +'</p>'
    },
    render: function() {
        var row;
        row = this.props.ColumnData.map(function(item, index) {

            if(item.label == "Remote Queue"){
                return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.queueFormat}>{item.label}</TableHeaderColumn>
            }
            else if(item.label == "Actions"){
                return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat}>{item.label}</TableHeaderColumn>
            }
            else if(item.label=="Contact")
            {
                return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.contactFormat}>{item.label}</TableHeaderColumn>
            }
            else{
               return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort}>{item.label}</TableHeaderColumn>
            }
            
            }.bind(this));

        return (
            <div>
                <BootstrapTable data={this.props.RowData} striped={true} hover={true} keyField={this.props.KeyField} pagination={true}>
                    {row}
                </BootstrapTable>           
                <ResendPurgeModal data={this.state} handleClose={this.close}/>
            </div>
                  )
        }
});

export default ReactTable;