import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Modal, ModalDialogue, ModalBody, ModalFooter, ModalHeader, ModalTitle, Button } from 'react-bootstrap';
import * as PathTranslationHelper from 'Actions/PathTranslation/PathTranslationActions';
import { NotificationContainer, NotificationManager } from 'react-notifications';

/// <summary>
/// Connect this component to global Redux data store
/// </summary>
@connect((store) => {
    return {
    };
})


/// <summary>
/// Model window for confirming delete operation
/// </summary>
class RemovePathTranslationModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            pathTranslationId: "",
            isProcessing: false,
            sourceBaseUrl: "",
            sourceBrand: "",
            targetbaseUrl: "",
            targetProtectionType: "",
            targetUrlType: ""
        });
    }



    /// <summary>
    /// Open modal with message
    /// </summary>
    onOpenModal = () => {
        this.setState({
            pathTranslationId: this.props.data.pathTranslationDetails.id,
            sourceBaseUrl: this.props.data.pathTranslationDetails.source.baseUrl,
            sourceBrand: this.props.data.pathTranslationDetails.source.brand,
            targetbaseUrl: this.props.data.pathTranslationDetails.target.baseUrl,
            targetProtectionType: this.props.data.pathTranslationDetails.target.protectionType,
            targetUrlType: this.props.data.pathTranslationDetails.target.urlType
        });
    }

    /// <summary>
    /// Purge this component's state
    /// </summary>
    purgeModalHistory = () => {
        console.log('called');
        this.setState({
            message: "",
            pathTranslationId: "",
            isProcessing: false,
            sourceBaseUrl: "",
            sourceBrand: "",
            targetbaseUrl: "",
            targetProtectionType: "",
            targetUrlType: ""
        });
    }


    /// <summary>
    /// Handle 'continue' button click event. Once delete is successful, propogate
    /// the event back to the parent component for further action 
    /// </summary>
    onContinue = () => {
        this.setState({ isProcessing: true });
        this.props.dispatch(PathTranslationHelper.deletePathTranslation(this.state.pathTranslationId))
            .then(() => {
                this.setState({ isProcessing: false });

                // bubble up the event further
                this.props.handleClose();
            }).catch(error => {
                NotificationManager.error(error.message, 'Failure');
                this.setState({ isProcessing: false });
            });
    }

    render() {
        return (
            <Modal show={this.props.data.showDeleteModal} onEntering={this.onOpenModal} onHide={this.props.handleClose} onExiting={this.purgeModalHistory}>
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
                            <span class='standout'>Base URL:</span> {this.state.sourceBaseUrl}
                            {
                                this.state.sourceBrand ? <div><span class='standout'>Brand:</span> {this.state.sourceBrand}</div> : ''
                            }

                        </div>
                        <div>
                            <h4>Target</h4>
                            <div><span class='standout'>Base URL:</span> {this.state.targetbaseUrl}</div>
                            <div><span class='standout'>Protection Type:</span> {this.state.targetProtectionType}</div>
                            <div><span class='standout'>URL Type:</span> {this.state.targetUrlType}</div>
                        </div>
                    </div>
                    <NotificationContainer />
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>
                    <Button disabled={this.state.isProcessing} bsStyle="primary" onClick={this.onContinue}>{this.state.isProcessing ? "Processing" : "Continue"}</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default RemovePathTranslationModal;