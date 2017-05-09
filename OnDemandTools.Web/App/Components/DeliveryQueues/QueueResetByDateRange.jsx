import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Button } from 'react-bootstrap';
import * as queueAction from 'Actions/DeliveryQueue/DeliveryQueueActions';

var QueueResetByDateRange = React.createClass({
    resetQueue(id){ 
        queueAction.resetQueues(id);
        this.props.handleClose();        
    },
    render: function() {
        return (
           <Modal show={this.props.data.showDateRangeResetModel} onHide={this.props.handleClose}>
                 <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Delivery by Date Range to {this.props.data.queueDetails.friendlyName}</p>
                    </Modal.Title>
                </Modal.Header>         
                <Modal.Body>
                    {
                        <p>Reset queue by date range for Queue {this.props.data.queueDetails.friendlyName}</p>
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>  
                    <Button onClick={this.resetQueue.bind(this, this.props.data.queueDetails.name)}>Continue</Button>
                </Modal.Footer>
           </Modal>
        )
    }
});

export default QueueResetByDateRange;