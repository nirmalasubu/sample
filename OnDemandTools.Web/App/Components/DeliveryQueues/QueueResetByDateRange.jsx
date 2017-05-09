import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import * as queueAction from 'Actions/DeliveryQueue/DeliveryQueueActions';
import { Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import DatePicker from "react-bootstrap-date-picker";

var QueueResetByDateRange = React.createClass({
    resetQueue(id){ 
        queueAction.resetQueues(id);
        this.props.handleClose();        
    },
    render: function() {
        return (
           <Modal bsSize="large" show={this.props.data.showDateRangeResetModel} onHide={this.props.handleClose}>
                 <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Delivery by Date Range to {this.props.data.queueDetails.friendlyName}</p>
                    </Modal.Title>
                </Modal.Header>         
                <Modal.Body>
                    <ControlLabel>This query does not permanently override the query in the Queue nor does it persists once this window is closed and the delivery is made. </ControlLabel>
                    <Form inline>
                        <ControlLabel>Delivery Airings where: </ControlLabel>
                        <FormGroup>
                            <InputGroup>
                                <ControlLabel> &nbsp;&nbsp;</ControlLabel>
                                <Radio name="dateAvailibilityWindow">
                                    &nbsp; Start date falls in availability window
                                </Radio>
                                <ControlLabel> &nbsp;&nbsp;</ControlLabel>
                                <Radio name="dateAvailibilityWindow">
                                    &nbsp; Any Part of flight falls in availability window
                                 </Radio>
                            </InputGroup>
                        </FormGroup>
                    </Form>
                    <Form inline>
                        <ControlLabel>Starting Between: </ControlLabel>{'  '}
                        <FormGroup >
                            <DatePicker style={datePickerWidth} showClearButton={false} />
                        </FormGroup>
                        {'  AND  '}
                        <FormGroup>
                            <DatePicker style={datePickerWidth} showClearButton={false} />
                        </FormGroup>
                    </Form>

                    <Form inline>
                        <FormGroup controlId="queryInput">                            
                            <FormControl componentClass="textarea" placeholder="Query" />
                        </FormGroup>
                        <FormGroup controlId="queryResults">                           
                            <FormControl componentClass="textarea" placeholder="Results" />
                        </FormGroup>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>  
                    <Button onClick={this.resetQueue.bind(this, this.props.data.queueDetails.name)}>Run Query</Button>
                </Modal.Footer>
           </Modal>
        )
    }
});

const datePickerWidth = { width: '400px' };

export default QueueResetByDateRange;