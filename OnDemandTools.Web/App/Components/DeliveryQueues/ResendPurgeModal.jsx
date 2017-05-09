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

var ResendPurgeModal = React.createClass({
    OnclickType(id,type){
        if(type=="resend")
            this.resendQueue(id);
        else if(type=="purge")
            this.purgeQueue(id);
        else
            this.clearQueue(id);

    },
    resendQueue(id){ 
        queueAction.resetQueues(id);
        this.props.handleClose();        
    },
    purgeQueue(id){ 
        queueAction.purgeQueues(id);
        this.props.handleClose();        
    },
    clearQueue(id){ 
        queueAction.clearQueues(id);
        this.props.handleClose();        
    },
    render: function() {
        var message, title;
        if(this.props.data.modalActionType=="resend")
        {
            title = "Resend";
            message = this.props.data.modalResendMessage.replace('<queue name>', this.props.data.queueDetails.friendlyName);
        }
        else if(this.props.data.modalActionType=="clear")
        {
            title = "Clear undelivered notification to " + this.props.data.queueDetails.friendlyName;
            message = this.props.data.modalClearMessage.replace('<queue name>', this.props.data.queueDetails.friendlyName);
        }
        else
        {
            title = "Purge";
            message = this.props.data.modalPurgeMessage.replace('<queue name>', this.props.data.queueDetails.friendlyName);
        }

        return (
           <Modal show={this.props.data.showModal} onHide={this.props.handleClose}> 
                <Modal.Header closeButton>
                    <Modal.Title>
                        {title}
                    </Modal.Title>
                </Modal.Header>        
                <Modal.Body>
                    {
                        <p>{message}</p>
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>  
                    <Button bsStyle="primary" onClick={this.OnclickType.bind(this, this.props.data.queueDetails.name, this.props.data.modalActionType)}>Continue</Button>
                </Modal.Footer>
           </Modal>
        )
    }
});

export default ResendPurgeModal;