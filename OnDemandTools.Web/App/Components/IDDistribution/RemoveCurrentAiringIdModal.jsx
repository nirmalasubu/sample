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
import * as airingIdActions from 'Actions/AiringIdDistribution/AiringIdDistributionActions';
import { NotificationContainer, NotificationManager } from 'react-notifications';

// To connect to store. Required for 'this.props.dispatch' to work
@connect((store) => {
    return {
        result:null
    };
})

// Sub component used within Distribution page to delete airing id
class RemoveCurrentAiringIdModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            isProcessing: false
        });
    }

    ///<summary>
    // React modal pop up control bubbles up when delete airing id modal up loads
    ///</summary>
    onOpenModal(){     
        this.setState({
            message:"If you continue, '" + this.props.data.airingIdDetails.prefix + "' will be permanantly deleted."
        });
    }

    ///<summary>
    //  when user conforms to delete airing id
    ///</summary>
    onContinue(){
        this.setState({ isProcessing: true });           
        this.props.dispatch(airingIdActions.deleteCurrentAiringId(this.props.data.airingIdDetails.id))
            .then(() => {
                this.setState({ isProcessing: false }); 
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
               Remove: {this.props.data.airingIdDetails.name}
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

export default RemoveCurrentAiringIdModal;