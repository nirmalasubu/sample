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


class RemoveDestinationModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: ""
        });
    }

    onOpenModal(){        
        this.setState({
            message:" If this destination is removed, it will be deleted when category changes are saved?"
        });
    }

    onContinue(){
        this.props.handleClose();
        this.props.handleRemoveAndClose(this.props.data.destinationIndexToRemove);
    }

    render(){

        return (
           <Modal show={this.props.data.showDestinationsDeleteModal} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}> 
                <Modal.Header closeButton>
                    <Modal.Title>
           Warning !
                    </Modal.Title>
                </Modal.Header>        
                <Modal.Body>
           {
                        <p>{this.state.message}</p>
           }
                </Modal.Body>
                <Modal.Footer>
                    <Button  onClick={this.props.handleClose}>NO</Button>  
                    <Button bsStyle="primary" onClick={(event) => this.onContinue(event)}>YES</Button>
                </Modal.Footer>
           </Modal>
        )
           }
}

export default RemoveDestinationModal;