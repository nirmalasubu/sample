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
import * as productActions from 'Actions/Product/ProductActions';
import { NotificationContainer, NotificationManager } from 'react-notifications';

// To connect to store. Required for 'this.props.dispatch' to work
@connect((store) => {
    
})

// Sub component used within Product page to delete product
class RemoveProductModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            isProcessing: false
        });
    }

    ///<summary>
    // React modal pop up control bubbles up when delete product modal up loads
    ///</summary>
    onOpenModal(){        
        this.setState({
            message:"If you continue, '" + this.props.data.productDetails.name + "' will be permantly deleted."
        });
    }

    ///<summary>
    //  when user conforms to delete product
    ///</summary>
    onContinue(){
        this.setState({ isProcessing: true });           
        this.props.dispatch(productActions.deleteProduct(this.props.data.productDetails.id))
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
               Remove: {this.props.data.productDetails.name}
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

export default RemoveProductModal;