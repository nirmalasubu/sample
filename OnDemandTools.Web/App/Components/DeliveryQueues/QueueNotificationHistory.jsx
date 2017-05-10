import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { fetchNotificationHistory, clearNotificationHistory } from 'Actions/DeliveryQueue/DeliveryQueueActions';
import { Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import DatePicker from "react-bootstrap-date-picker";
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

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
        this.props.dispatch(clearNotificationHistory(id));
        this.props.dispatch(fetchNotificationHistory(id));
    }
    render() {
        return (
            <Modal onEntering={this.resetGrid.bind(this, this.props.data.queueDetails.name)} show={this.props.data.showNotificationHistoryModel} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Notification History for {this.props.data.queueDetails.friendlyName}  <i title="The list contains the 50 most recent processed notifications over the last 7 days" class="fa fa-info-circle" aria-hidden="true"></i></p>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form inline>
                        <FormGroup controlId="queueName">
                            <FormControl type="text" style={textSearchStyle} ref="filterInput" placeholder="Search by one or more Airing IDs" />
                        </FormGroup>
                    </Form>
                    <BootstrapTable data={this.props.notificationHistory} striped hover>
                        <TableHeaderColumn isKey dataField='airingId'>Airing Id</TableHeaderColumn>
                        <TableHeaderColumn dataField='dateTime'>Processed Time</TableHeaderColumn>
                    </BootstrapTable>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Close</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

const textSearchStyle = { width: '400px' };
export default QueueNotificationHistory;