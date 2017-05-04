import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import { resetQueues } from 'Actions/DeliveryQueue/DeliveryQueueActions';
require('react-bootstrap-table/css/react-bootstrap-table.css');


var ReactTable = React.createClass({    
    
    getInitialState() {
        return { showModal: false, resendQueueName : "" };
    },

    close() {
        this.setState({ showModal: false, resendQueueName : "", resendQueueId: "" });
    },

    open(name, queueId) {
        console.log("open " + queueId);
        this.setState({ showModal: true, resendQueueName : name, resendQueueId: queueId });
    },
    resendQueue(id){
        console.log("resend " + id);
        resetQueues(id);
        close();
        window.location.reload(); 
    },
    actionFormat: function() {
        return '<i class="fa fa-search" aria-hidden="true"></i> <i class="fa fa-calendar" aria-hidden="true"></i>'
    },
    queueFormat: function(val) {
        var queueArray = $.grep(this.props.RowData, function (v) {
            return (v.name==val);
        });
        return(
                <div>
                    <p>{val}</p>
                    <i>Delivery</i>: 0
                    <button class="btn-xs btn-link" title="clear pending deliveries to queue">Clear</button>
                    <button class="btn-xs btn-link" title="clear pending deliveries to queue" onClick={(event) => this.open(queueArray[0].friendlyName, val, event)}>Resend</button><br />
                    <i>Consumption</i>: 21
                    <button class="btn-xs btn-link">Purge</button><br />
                    <span class="small">-Apr 25, 2017 2:24 PM</span>
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
            
                <Modal show={this.state.showModal} onHide={this.close}>          
                    <Modal.Body>
                    <p>If you continue, {this.state.resendQueueName} Queue will be reset and any notifications matching your criteria will be delivered again. 
                    Do you wish to continue?</p>
                    </Modal.Body>
                    <Modal.Footer>
                    <Button onClick={this.close}>Cancel</Button>  
                    <Button onClick={(event) => this.resendQueue(this.state.resendQueueId, event)}>Continue</Button>
                    </Modal.Footer>
                </Modal>
            </div>
                  )
        }
});

export default ReactTable;