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
import Moment from 'moment';

@connect((store) => {
    return {
        notificationHistory: store.notificationHistory
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
    airingIdFormat(val, row) {
        return <a href="" title={row.message} target="_blank"> {val} </a>
    }

    dateFormat(val, row) {
        var formattedDate = Moment(val).format('lll');
        return <div>{formattedDate}</div>
    }
    
    alertData(val) {
        console.log(val);
        console.log("fsdfsd");
    }
    actionsFormat(airingId) {
        return <div>
            <Button class="btn-xs btn-link"
                title="Airing will be reset and notifications will be delivered again"  onClick={this.alertData.bind(this,airingId)}
            >Resend</Button>
        </div >
    }
    render() {
        return (
            <Modal bsSize="large" onEntering={this.resetGrid.bind(this, this.props.data.queueDetails.name)} show={this.props.data.showNotificationHistoryModel} onHide={this.props.handleClose}>
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
                    <BootstrapTable data={this.props.notificationHistory} striped hover pagination={true}>
                        <TableHeaderColumn isKey dataSort={true} dataField="airingId" dataFormat={this.airingIdFormat} >Airing Id</TableHeaderColumn>
                        <TableHeaderColumn dataField="dateTime" dataSort={false} dataFormat={this.dateFormat}>Processed Time</TableHeaderColumn>
                        <TableHeaderColumn dataField="airingId" dataSort={false} dataFormat={this.actionsFormat.bind(this)}>Actions</TableHeaderColumn>
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