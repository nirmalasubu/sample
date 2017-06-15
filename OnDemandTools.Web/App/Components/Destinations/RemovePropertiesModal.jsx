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


class RemovePropertiesModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: ""
        });
    }

    onOpenModal(){        
        this.setState({
            message:" Would you like to Remove the property?"
        });
    }

    onContinue(){
        this.props.handleClose();
        this.props.handleRemoveAndClose(this.props.data.propertyIndexToRemove);
    }

    render(){

        return (
           <Modal show={this.props.data.showPropertiesDeleteModal} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}> 
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

export default RemovePropertiesModal;