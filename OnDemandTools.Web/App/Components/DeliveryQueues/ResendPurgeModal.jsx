import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { Button } from 'react-bootstrap';
import * as queueAction from 'Actions/DeliveryQueue/DeliveryQueueActions';

var ResendPurgeModal = React.createClass({
    resendQueue(id){ 
        console.log("resend");
        queueAction.resetQueues(id);
        this.props.handleClose();        
    },
    purgeQueue(id){ 
        console.log("purge");
        queueAction.purgeQueues(id);
        this.props.handleClose();        
    },
    render: function() {
        var message;
        if(this.props.data.modalActionType=="resend")
            message = this.props.data.modalResendMessage.replace('<queue name>', this.props.data.queueDetails.friendlyName);
        else
            message = this.props.data.modalPurgeMessage.replace('<queue name>', this.props.data.queueDetails.friendlyName);

        return (
           <Modal show={this.props.data.showModal} onHide={this.props.handleClose}>          
                <Modal.Body>
                    {
                        <p>{message}</p>
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>  
                    <Button onClick={this.props.data.modalActionType=="resend"?(this.resendQueue.bind(this, this.props.data.queueDetails.name)):(this.purgeQueue.bind(this, this.props.data.queueDetails.name))}>Continue</Button>
                </Modal.Footer>
           </Modal>
        )
    }
});

export default ResendPurgeModal;