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
import Moment from 'moment';

var QueueResetByDateRange = React.createClass({
    getInitialState() {  
        var iDate = new Date().toISOString();
        var queueName = this.props.data.queueDetails.name;
        var queryDetail = this.props.data.queueDetails.query;
        console.log(this.props.data.queueDetails.query);
        return{
            validationStartDateState: "",
            validationEndDateState: "",
            validationQueryState: "",
            deliverCriteria: {
                name: queueName,
                query: queryDetail,
                windowOption: 1,
                startDateTime: iDate,
                endDateTime: iDate
            }
        };
    },
    resetQueue(criteria){ 
        queueAction.resetQueueByCriteria(criteria);
        this.props.handleClose();        
    },
    handleChangeStartDate(value) { 
        var valuess = this.state.deliverCriteria;
        valuess.startDateTime = value;
        this.setState({
            deliverCriteria: valuess
        });
    },
    handleChangeEndDate(value) {
        var valuess = this.state.deliverCriteria;
        valuess.endDateTime = value;
        this.setState({
            deliverCriteria: valuess
        });
    },
    _onOptionChange(val)
    {
        var valuess = this.state.deliverCriteria;
        valuess.windowOption = val;
        this.setState({
            deliverCriteria: valuess
        });
    },
    onQueryChange(event)
    {
        console.log(event.target.value);
        var valuess = this.state.deliverCriteria;
        valuess.query = event.target.value;
        this.setState({
            deliverCriteria: valuess
        });
    },
    _onSubmitForm()
    {
        console.log("submit");
        if(this.state.deliverCriteria.startDateTime=="")
            this.state.validationStartDateState="error";
        else
            this.state.validationStartDateState="";

        if(this.state.deliverCriteria.endDateTime=="")
            this.state.validationEndDateState="error";
        else
            this.state.validationEndDateState="";

        if(this.state.deliverCriteria.query=="")
            this.state.validationQueryState="error";
        else
            this.state.validationQueryState="";

        if(this.state.validationStartDateState=="error"||this.state.validationEndDateState=="error"||this.state.validationQueryState=="error")
        {
            return false;
        }
        else
        {
            this.resetQueue(this.state.deliverCriteria);
        }            
    },
    render: function() {        
        return (
           <Modal bsSize="large" show={this.props.data.showDateRangeResetModel} onHide={this.props.handleClose}>
                 <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Delivery by Date Range to {this.props.data.queueDetails.friendlyName}</p>
                    </Modal.Title>
                </Modal.Header> 
                <Form onSubmit={this._onSubmitForm.bind(this)}>        
                <Modal.Body>
                    <ControlLabel>This query does not permanently override the query in the Queue nor does it persists once this window is closed and the delivery is made. <br/><br/> </ControlLabel>
                    
                        <Grid>
                            <Row>
                                <Col md={2} componentClass={ControlLabel}>
                                    Delivery Airings where: 
                                </Col>                            
                                <Col md={4}>
                                    <FormGroup>
                                        <InputGroup>
                                            <Radio onClick={this._onOptionChange.bind(this, 1)} checked={this.state.deliverCriteria.windowOption === 1} name="dateAvailibilityWindow">
                                                &nbsp; Start date falls in availability window
                                            </Radio>
                                            <Radio onClick={this._onOptionChange.bind(this, 2)} checked={this.state.deliverCriteria.windowOption === 2} name="dateAvailibilityWindow">
                                                &nbsp; Any Part of flight falls in availability window
                                            </Radio>
                                        </InputGroup>
                                    </FormGroup>
                                </Col>
                            </Row>
                        </Grid>
                    <br/>
                        <Grid>
                            <Row>
                                <Col md={2} componentClass={ControlLabel}>
                                    Starting Between:  
                                </Col>
                                <Col md={2}>
                                    <FormGroup validationState={this.state.validationStartDateState} >
                                        <DatePicker style={datePickerWidth} onChange={this.handleChangeStartDate} value={this.state.deliverCriteria.startDateTime} id="startDateCtrl" showClearButton={false} />                                        
                                    </FormGroup>
                                </Col>
                                <Col md={1} componentClass={ControlLabel}>AND</Col>
                                <Col md={2}>
                                    <FormGroup validationState={this.state.validationEndDateState}>
                                        <DatePicker style={datePickerWidth} onChange={this.handleChangeEndDate} value={this.state.deliverCriteria.endDateTime} id="endDateCtrl" showClearButton={false} />
                                    </FormGroup>
                                </Col>
                            </Row>
                        </Grid>
                    <br/>
                        <Grid>
                            <Row>
                                <Col md={4}>
                                    <ControlLabel>Query Criteria</ControlLabel>
                                    <FormGroup controlId="queryInput" validationState={this.state.validationQueryState}>
                                        <FormControl style={textAreaWidth} onChange={this.onQueryChange.bind(this)} value={this.state.deliverCriteria.query} componentClass="textarea" />
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
                    
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>  
                    <Button className="btn btn-primary btn-large" type="submit">Run Query</Button>
                </Modal.Footer>
                </Form>
           </Modal>
        )
    }
});

const datePickerWidth = { maxWidth: '200px' };

const textAreaWidth = { maxWidth: '400px', height: '100px' };

export default QueueResetByDateRange;