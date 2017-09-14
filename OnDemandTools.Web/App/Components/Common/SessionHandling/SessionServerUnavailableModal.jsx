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


class SessionServerUnavailableModal extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <Modal backdrop="static" show={this.props.data.showServerUnavailableModal} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        Warning !
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {
                        <p>Server session updated. Click Ok to refresh</p>
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Ok</Button>
                </Modal.Footer>
            </Modal>
        )

    }
}

export default SessionServerUnavailableModal;