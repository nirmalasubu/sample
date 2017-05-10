import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import {fetchNotificationHistory} from 'Actions/DeliveryQueue/DeliveryQueueActions';
import { Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import DatePicker from "react-bootstrap-date-picker";
import { connect } from 'react-redux';

@connect((store) => {
    return {
        notificationHistory: store.notificationHistory,
    };
})
class QueueNotificationHistory extends React.Component {

    constructor(props) {
        super(props);
    }
    //called on the model load
    resetGrid(id) {
       console.log(id);
       this.props.dispatch(fetchNotificationHistory(id));
    }
    render () {

let myTest = null;
    if (this.props.notificationHistory.length>0) {
      myTest = <p> {this.props.notificationHistory[0].name} </p>
    } else {
      myTest = <p> Not found </p>
    }

        return (
            <Modal onEntering={this.resetGrid.bind(this, this.props.data.queueDetails.name)} show={this.props.data.showNotificationHistoryModel} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Notification History for {this.props.data.queueDetails.friendlyName}</p>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form inline>
                        <FormGroup controlId="queueName">
                            <FormControl type="text" ref="filterInput" placeholder="Search by one or more Airing IDs" />
                        </FormGroup>
                    </Form>

                    <p>{myTest}</p>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Close</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}
export default QueueNotificationHistory;