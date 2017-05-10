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
                    <ControlLabel>This query does not permanently override the query in the Queue nor does it persists once this window is closed and the delivery is made. <br/><br/> </ControlLabel>
                    <Form >
                        <Grid>
                            <Row>
                                <Col md={2} componentClass={ControlLabel}>
                                    Delivery Airings where: 
                                </Col>                            
                                <Col md={4}>
                                    <FormGroup>
                                        <InputGroup>
                                            <Radio name="dateAvailibilityWindow">
                                                &nbsp; Start date falls in availability window
                                            </Radio>
                                            <Radio name="dateAvailibilityWindow">
                                                &nbsp; Any Part of flight falls in availability window
                                             </Radio>
                                        </InputGroup>
                                    </FormGroup>
                                </Col>
                            </Row>
                        </Grid>
                    </Form>
                    <br/>
                    <Form >
                        <Grid>
                            <Row>
                                <Col md={2} componentClass={ControlLabel}>
                                    Starting Between:  
                                </Col>
                                <Col md={2}>
                                    <FormGroup >
                                        <DatePicker style={datePickerWidth} showClearButton={false} />                                        
                                    </FormGroup>
                                </Col>
                                <Col md={1} componentClass={ControlLabel}>AND</Col>
                                <Col md={2}>
                                    <FormGroup >
                                        <DatePicker style={datePickerWidth} showClearButton={false} />
                                    </FormGroup>
                                </Col>
                            </Row>
                        </Grid>
                    </Form>
                    <br/>
                    <Form >
                        <Grid>
                            <Row>
                                <Col md={4}>
                                    <ControlLabel>Query Criteria</ControlLabel>
                                    <FormGroup controlId="queryInput" >
                                        <FormControl style={textAreaWidth} componentClass="textarea" placeholder="Query" />
                                    </FormGroup>
                                </Col>
                                <Col md={4}>
                                    <ControlLabel>Syntax Checker</ControlLabel>
                                    <FormGroup controlId="queryResults">
                                        <FormControl style={textAreaWidth} componentClass="textarea" placeholder="Results" />
                                    </FormGroup>
                                </Col>
                            </Row>
                        </Grid>
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

const datePickerWidth = { maxWidth: '200px' };

const textAreaWidth = { maxWidth: '400px', height: '100px' };

export default QueueResetByDateRange;