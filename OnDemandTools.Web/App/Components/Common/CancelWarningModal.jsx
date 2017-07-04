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


class CancelWarningModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: ""
        });
    }

    onOpenModal(){        
        this.setState({
            message:"Any changes made will be discarded. Would you like to continue?"
        });
    }

    onContinue(){
        this.props.handleClose();
        this.props.handleAddEditClose();
    }

    render(){

        return (
           <Modal show={this.props.data.showWarningModel} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}> 
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

export default CancelWarningModal;