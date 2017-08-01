import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Modal, ModalDialogue, ModalBody, ModalFooter, ModalHeader, ModalTitle, Button, NotificationContainer, NotificationManager } from 'react-bootstrap';


/// <summary>
//  Model window for confirming delete operation
/// </summary>
class RemovePathTranslationModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            isProcessing: false,
            sourceBaseUrl: "",
            sourceBrand: "",
            targetbaseUrl: "",
            targetProtectionType: "",
            targetUrlType: ""
        });
    }

   

    ///<summary>
    // Open modal with message
    ///</summary>
    onOpenModal() {
        this.setState({
            sourceBaseUrl: this.props.data.pathTranslationDetails.source.baseUrl,
            sourceBrand: this.props.data.pathTranslationDetails.source.brand,
            targetbaseUrl: this.props.data.pathTranslationDetails.target.baseUrl,
            targetProtectionType: this.props.data.pathTranslationDetails.target.targetProtectionType,
            targetUrlType:this.props.data.pathTranslationDetails.target.urlType
        });
    }

    ///<summary>
    //  when user conforms to delete product
    ///</summary>
    onContinue() {
        this.setState({ isProcessing: true });
        console.log("whoo...deleted");
        // this.props.dispatch(productActions.deleteProduct(this.props.data.productDetails.id))
        //     .then(() => {
        //         this.setState({ isProcessing: false });
        //         this.props.handleClose();
        //     }).catch(error => {
        //         this.setState({ isProcessing: false });
        //     });
    }

    render() {

        return (
            <Modal show={this.props.data.showDeleteModal} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        Remove Path Translation
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        <p>If you continue, the following path translation entry will be deleted.</p>
                        <div>
                            <h4>Source</h4>
                            <div><span class='standout'>Base URL:</span> {this.state.sourceBaseUrl}</div>
                            <div><span class='standout'>Brand:</span> {this.state.sourceBrand}</div>
                        </div>
                        <div>
                            <h4>Target</h4>
                            <div><span class='standout'>Base URL:</span> {this.state.targetbaseUrl}</div>
                            <div><span class='standout'>Protection Type:</span> {this.state.targetProtectionType}</div>
                            <div><span class='standout'>URL Type:</span> {this.state.targetUrlType}</div>
                        </div>
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>
                    <Button disabled={this.state.isProcessing} bsStyle="primary" onClick={(event) => this.onContinue(event)}>{this.state.isProcessing ? "Processing" : "Continue"}</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default RemovePathTranslationModal;