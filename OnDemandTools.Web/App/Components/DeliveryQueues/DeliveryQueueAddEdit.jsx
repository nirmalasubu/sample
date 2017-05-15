import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import Moment from 'moment';
import { NotificationContainer, NotificationManager } from 'react-notifications';

@connect((store) => {
    return {
        notificationHistory: store.notificationHistory,
        config: store.config
    };
})
class DeliveryQueueAddEdit extends React.Component {

    constructor(props) {
        super(props);
    }
    resetForm(queueName) {

    }
    render() {
        return (
            <Modal bsSize="large" onEntering={this.resetForm.bind(this, this.props.data.queueDetails.name)} show={this.props.data.showAddEditModel} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Edit Queue {this.props.data.queueDetails.friendlyName}</p>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form inline>
                        <FormGroup controlId="queueName">

                        </FormGroup>
                    </Form>
                    <NotificationContainer />
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>
                    <Button>Save</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}
export default DeliveryQueueAddEdit;