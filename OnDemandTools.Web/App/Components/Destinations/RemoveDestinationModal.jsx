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
import * as destinationAction from 'Actions/Destination/DestinationActions';
import { NotificationContainer, NotificationManager } from 'react-notifications';

@connect((store) => {
    return {
        products: store.products,
        destinations: store.destinations
    };
})

class RemoveDestinationModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            isProcessing: false,
            inProduct:false
        });
    }

    onOpenModal(){
        if(this.isDestinationBeingUsed(this.props.data.destinationDetails.name))
        {
            this.setState({
                inProduct:true,
                message:"This destination is being used by product(s). Remove the destination from all products before deleting.",
                isProcessing: false
            });
        }
        else{
            this.setState({
                inProduct:false,
                message:"If you continue, the destination '" + this.props.data.destinationDetails.name + "' will be completely deleted which could cause a problem if this destination has been used.",
                isProcessing: false
            });
        }
    }

    isDestinationBeingUsed(destination) {
        var prod = this.props.products;
        for (var p = 0; p < prod.length; p++) {
            if (prod[p].destinations.indexOf(destination) > -1) {
                return true;
            }
        }

        return false;
    }

    onContinue(){
        if(!this.state.inProduct)
        {
            this.setState({ isProcessing: true });            
            this.props.dispatch(destinationAction.deleteDestination(this.props.data.destinationDetails.id))
                .then(() => {
                    this.props.handleClose();
                }).catch(error => {
                    this.setState({ isProcessing: false });
                });
        }
        else
            this.props.handleClose();
    }

    render(){

        return (
           <Modal show={this.props.data.showModal} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}> 
                <Modal.Header closeButton>
                    <Modal.Title>
           Remove {this.props.data.destinationDetails.name}
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

export default RemoveDestinationModal;