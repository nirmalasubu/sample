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
import * as permissionAction from 'Actions/Permissions/PermissionActions';
import { NotificationContainer, NotificationManager } from 'react-notifications';

@connect((store) => {
    return {

    }
})
class SystemInactivateModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            isProcessing: false
        });
    }

    onOpenModal() {
        this.setState({
            message: "If you continue, '" + this.props.data.inActiveModal.userName + "' will no longer be able to access ODT API.",
            isProcessing: false
        });
    }

    onContinue() {
        this.setState({ isProcessing: true });

        var model = this.props.data.inActiveModal;
        model.api.isActive = false;
        this.props.dispatch(permissionAction.savePermission(model))
            .then(() => {
                this.props.handleClose();
            }).catch(error => {
                this.setState({ isProcessing: false });
            });
    }

    render() {

        return (
            <Modal show={this.props.data.showInactiveModal} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        Deactivate System: {this.props.data.inActiveModal.userName}
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {
                        <p>{this.state.message}</p>
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>
                    <Button disabled={this.state.isProcessing} bsStyle="primary" onClick={(event) => this.onContinue(event)}>{this.state.isProcessing ? "Processing" : "Continue"}</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default SystemInactivateModal;