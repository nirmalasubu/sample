import React from 'react';
import { connect } from 'react-redux';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Button } from 'react-bootstrap';
import * as queueAction from 'Actions/DeliveryQueue/DeliveryQueueActions';
import { NotificationContainer, NotificationManager } from 'react-notifications';

@connect((store) => {
    return {}
})

class RemoveQueueModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            isProcessing: false
        });
    }

    onOpenModal(){        
        this.setState({
            message:"If you continue, '" + this.props.data.queueDetails.friendlyName + "' will be permanently deleted.",
            isProcessing: false
        });
    }

    onContinue(){
        this.setState({ isProcessing: true });           
        this.props.dispatch(queueAction.deleteQueues(this.props.data.queueDetails.id, this.props.data.queueDetails.name))
            .then(() => {
                this.props.handleClose();
            }).catch(error => {
                this.setState({ isProcessing: false });
            });
    }

    render(){

        return (
           <Modal show={this.props.data.showDeleteModal} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}> 
                <Modal.Header closeButton>
                    <Modal.Title>
           Remove {this.props.data.queueDetails.friendlyName}
                    </Modal.Title>
                </Modal.Header>        
                <Modal.Body>
           {
                        <p>{this.state.message}</p>
           }
                </Modal.Body>
                <Modal.Footer>
                    <Button  onClick={this.props.handleClose}>Cancel</Button>  
                    <Button disabled={this.state.isProcessing} bsStyle="primary" onClick={(event) => this.onContinue(event)}>{this.state.isProcessing ? "Processing" : "Continue"}</Button>
                </Modal.Footer>
           </Modal>
        )
    }
}

export default RemoveQueueModal;