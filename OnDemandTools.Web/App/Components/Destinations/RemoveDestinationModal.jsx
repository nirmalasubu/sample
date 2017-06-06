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

@connect((store) => {
    return {
        products: store.products
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
                message:"This destination is being used by product(s). Remove the destination from all products before deleting."
            });
        }
        else{
            this.setState({
                inProduct:false,
                message:"If you continue, the destination '" + this.props.data.destinationDetails.name + "' will be completely deleted which could cause a problem if this destination has been used."
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
        var elem = this;
        if(!this.state.inProduct)
        {
            this.setState({ isProcessing: true });
            this.props.dispatch(destinationAction.deleteDestination())
                .then(() => {
                    NotificationManager.success(this.props.data.destinationDetails.name + ' Destination deleted successfully.', '', 2000);
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    NotificationManager.error(this.props.data.destinationDetails.name + ' delete failed. ' + error, 'Failure');
                    this.setState({ isProcessing: false });
                });
        }
        else
            elem.props.handleClose();
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
                    <Button onClick={this.props.handleClose}>Cancel</Button>  
                    <Button bsStyle="primary" onClick={(event) => this.onContinue(event)}>Continue</Button>
                </Modal.Footer>
           </Modal>
        )
    }
}

export default RemoveDestinationModal;