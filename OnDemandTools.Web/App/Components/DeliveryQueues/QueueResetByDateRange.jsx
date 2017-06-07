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
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import Moment from 'moment';

class QueueResetByDateRange extends React.Component {
    constructor(props) {
        super(props);
        var iDate = new Moment();
        this.state = {
            validationStartDateState: "",
            validationEndDateState: "",
            validationQueryState: "",
            queryValue: "",
            syntaxCheckerResults: "",
            deliverCriteria: {
                name: "",
                query: "initialstage",
                windowOption: 0,
                startDateTime: iDate,
                endDateTime: iDate
            }
        }
    }

    modelOpen() {
        var iDate = new Moment();
        var criteria = this.state.deliverCriteria;
        criteria.startDateTime = iDate;
        criteria.endDateTime = iDate;
        criteria.query = this.props.data.queueDetails.query;
        criteria.windowOption = 0;

        this.setState({
            deliverCriteria: criteria,
            validationStartDateState: "",
            validationEndDateState: "",
            syntaxCheckerResults: ""
        });

        this.validateForm();
    }

    resetQueue(criteria) {
        queueAction.resetQueueByCriteria(criteria);
        this.props.handleClose();
    }

    handleRawStartDateChange(value) {
        this.setState({ validationStartDateState: 'error' });
    }

    handleChangeStartDate(value) {
        if (value == null) {
            this.setState({ validationStartDateState: 'error' });
        }
        else {
            var criteria = this.state.deliverCriteria;
            this.setState({ validationStartDateState: '' });
            criteria.startDateTime = value;

            var mins = value.diff(this.state.endDateTime, 'hours');
            if (mins > 0) {
                criteria.endDateTime = value;
            }

            this.setState({
                deliverCriteria: criteria
            });
        }
    }

    handleRawEndDateChange(value) {
        this.setState({ validationEndDateState: 'error' });
    }

    handleChangeEndDate(value) {
        if (value == null) {
            this.setState({ validationEndDateState: 'error' });
        }
        else {
            this.setState({ validationEndDateState: '' });

            var criteria = this.state.deliverCriteria;
            var mins = value.diff(criteria.startDateTime, 'hours');
            if (mins > 0) {
                criteria.endDateTime = value;
                this.setState({
                    deliverCriteria: criteria
                });
            }
        }
    }

    _onOptionChange(val) {
        var valuess = this.state.deliverCriteria;
        valuess.windowOption = val;
        this.setState({
            deliverCriteria: valuess
        });
    }

    onQueryChange(event) {
        var value = event.target.value;
        var criteria = this.state.deliverCriteria;
        criteria.query = value;
        this.setState({
            deliverCriteria: criteria
        });

        this.validateForm();
    }

    syntaxChecker(event) {

        this.setState({
            syntaxCheckerResults: "please wait..."
        });

        let promise = queueAction.getResultsForQuery(this.state.deliverCriteria);

        promise.then(message => {
            this.setState({
                syntaxCheckerResults: JSON.stringify(message, null, 2)
            });
        })
            .catch(error => {
                this.setState({
                    syntaxCheckerResults: error
                });
            });
    }

    validateForm() {
        var value = this.state.deliverCriteria.query;
        var hasError = false;
        if (value == "") {
            hasError = true;
        }
        else {
            try {
                JSON.parse(value);
            } catch (e) {
                hasError = true;
            }
        }
        this.setState({ validationQueryState: hasError ? 'error' : '' });
    }

    _onSubmitForm() {
        if (this.state.validationStartDateState == "error" || this.state.validationEndDateState == "error" || this.state.validationQueryState == "error") {
            return false;
        }
        else {
            var valuess = this.state.deliverCriteria;
            valuess.name = this.props.data.queueDetails.name;
            valuess.query = valuess.query === "initialstage" ? this.state.queryValue : valuess.query;
            this.setState({
                deliverCriteria: valuess
            });
            this.resetQueue(this.state.deliverCriteria);
        }
    }

    render() {
        this.state.queryValue = this.props.data.queueDetails.query;
        var dateLabel = (this.state.deliverCriteria.windowOption === 0) ? "Starting Between :" : "Availability Between :";

        return (
            <Modal bsSize="large" className="queueResetByDateRangeModel" onEntered={this.modelOpen.bind(this)} show={this.props.data.showDateRangeResetModel} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>Delivery by Date Range to {this.props.data.queueDetails.friendlyName}</div>
                    </Modal.Title>
                </Modal.Header>
                <Form>
                    <Modal.Body>
                        <ControlLabel>This query does not permanently override the query in the Queue nor does it persist once this window is closed and the delivery is made. <br /><br /> </ControlLabel>

                        <Grid>
                            <Row>
                                <Col md={2} componentClass={ControlLabel}>
                                    Delivery Airings where:
                                </Col>
                                <Col md={4}>
                                    <FormGroup>
                                        <InputGroup>
                                            <Radio onClick={this._onOptionChange.bind(this, 0)} checked={this.state.deliverCriteria.windowOption === 0} name="dateAvailibilityWindow">
                                                &nbsp; Start date falls in availability window
                                            </Radio>
                                            <Radio onClick={this._onOptionChange.bind(this, 1)} checked={this.state.deliverCriteria.windowOption === 1} name="dateAvailibilityWindow">
                                                &nbsp; Any Part of flight falls in availability window
                                            </Radio>
                                        </InputGroup>
                                    </FormGroup>
                                </Col>
                            </Row>
                        </Grid>
                        <br />
                        <Grid>
                            <Row>
                                <Col md={2} componentClass={ControlLabel}>
                                    {dateLabel}
                                </Col>
                                <Col md={8}>
                                    <Form inline>
                                        <FormGroup validationState={this.state.validationStartDateState} >
                                            <DatePicker id="startDateCtrl" selected={this.state.deliverCriteria.startDateTime} dateFormat="MM/DD/YYYY" onChange={(event) => this.handleChangeStartDate(event)}
                                                onChangeRaw={(event) => this.handleRawStartDateChange(event.target.value)}
                                                className={"form-control"}
                                            />
                                        </FormGroup>
                                        <ControlLabel className="marginLeftRight">AND</ControlLabel>
                                        <FormGroup validationState={this.state.validationEndDateState}>
                                            <DatePicker id="endDateCtrl" selected={this.state.deliverCriteria.endDateTime} minDate={this.state.deliverCriteria.startDateTime} dateFormat="MM/DD/YYYY" onChange={(event) => this.handleChangeEndDate(event)}
                                                onChangeRaw={(event) => this.handleRawEndDateChange(event.target.value)}
                                                className={"form-control"} />
                                        </FormGroup>
                                    </Form>
                                </Col>
                            </Row>
                        </Grid>
                        <br />
                        <Grid>
                            <Row>
                                <Col md={4}>
                                    <FormGroup controlId="queryInput" validationState={this.state.validationQueryState}>
                                        <ControlLabel>Query Criteria &nbsp;
                                        <a target="_mongo" href="http://docs.mongodb.org/master/tutorial/query-documents/">
                                                <span tooltip data-toggle="tooltip" data-placement="right" title="Click to view the Mongo query syntax official documentation." class="glyphicon glyphicon-info-sign"></span>
                                            </a>
                                            &nbsp;&nbsp;
                                        <span>{this.state.validationQueryState != '' ? "Please provide valid query criteria" : ""}</span>
                                        </ControlLabel>
                                        <FormControl bsClass="form-control form-control-modal" onChange={this.onQueryChange.bind(this)} value={(this.state.deliverCriteria.query === "initialstage" ? this.state.queryValue : this.state.deliverCriteria.query)} componentClass="textarea" />
                                    </FormGroup>
                                </Col>
                                <Col md={4}>
                                    <ControlLabel>
                                        <Button disabled={this.state.validationQueryState != ''} class="btn-link noPadding" onClick={(event) => this.syntaxChecker(event)}>Validate Query Criteria</Button>
                                    </ControlLabel>
                                    <FormGroup controlId="queryResults">
                                        <FormControl bsClass="form-control form-control-modal" componentClass="textarea" value={this.state.syntaxCheckerResults} placeholder="Results" />
                                    </FormGroup>
                                </Col>
                            </Row>
                        </Grid>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={this.props.handleClose}>Cancel</Button>
                        <Button disabled={(this.state.validationEndDateState != '' || this.state.validationStartDateState != '' || this.state.validationQueryState != '')}
                            onClick={this._onSubmitForm.bind(this)} className="btn btn-primary btn-large">Run Query</Button>
                    </Modal.Footer>
                </Form>
            </Modal>
        )
    }
}

export default QueueResetByDateRange;