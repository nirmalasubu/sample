import React from 'react';
import ReactDOM from 'react-dom';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { saveQueue } from 'Actions/DeliveryQueue/DeliveryQueueActions';
import { getResultsForQuery } from 'Actions/DeliveryQueue/DeliveryQueueActions';
import { connect } from 'react-redux';
import Moment from 'moment';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import Select from 'react-select';
import 'react-select/dist/react-select.css';
import validator from 'validator';

@connect((store) => {
    return {
        statuses: store.statuses
    };
})
class DeliveryQueueAddEdit extends React.Component {

    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);

        this.state = ({
            validationStateName: "",
            validationStateEmail: "",
            validationQueryState: "",
            syntaxCheckerResults: "",
            isProcessing: false,
            queueModel: {},
            options: [],
            selectAllArray: []
        });
    }

    getStatusNames(statusesObjects) {
        var names = [];

        for (var x = 0; x < statusesObjects.length; x++) {
            names.push(statusesObjects[x].name);
        }

        return names;
    }

    getOptions(statusesObjects) {
        var options = [
            { value: 'Select All', label: 'Select All' },
        ];

        for (var x = 0; x < statusesObjects.length; x++) {
            var optionValue = { value: statusesObjects[x].name, label: statusesObjects[x].name };
            options.push(optionValue);
        }

        return options;
    }

    resetForm(queueName) {
        var model = $.extend(true, {}, this.props.data.queueDetails);
        var optionValues = this.getOptions(this.props.statuses);
        var statusNames = this.getStatusNames(this.props.statuses);

        this.setState({
            validationStartDateState: "",
            validationEndDateState: "",
            syntaxCheckerResults: "",
            queueModel: model,
            options: optionValues,
            selectAllArray: statusNames,
            isProcessing: false
        });

        //set the focus
        let node = ReactDOM.findDOMNode(this.refs.inputFriendlyName);
        if (node && node.focus instanceof Function) {
            node.focus();
        }
    }

    syntaxChecker(event) {
        this.setState({
            syntaxCheckerResults: "please wait..."
        });

        var iDate = new Moment();
        var deliverCriteria = {
            name: "",
            query: this.state.queueModel.query,
            windowOption: 0,
            startDateTime: iDate,
            endDateTime: iDate
        }

        let promise = getResultsForQuery(deliverCriteria);

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

    handleSave() {
        var elem = this;
        this.validateForm();
        if (this.state.validationStateName != "error" && this.state.validationStateEmail != "error" && this.state.validationQueryState != "error") {

            this.setState({ isProcessing: true });

            this.props.dispatch(saveQueue(this.state.queueModel))
                .then(() => {
                    if (this.props.data.queueDetails.id == null) {
                        NotificationManager.success(this.state.queueModel.friendlyName + ' queue successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(this.state.queueModel.friendlyName + ' queue updated successfully.', '', 2000);
                    }
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    if (this.props.data.queueDetails.id == null) {
                        NotificationManager.error(this.state.queueModel.friendlyName + ' queue creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(this.state.queueModel.friendlyName + ' queue update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
    }

    validateForm() {
        var name = this.state.queueModel.friendlyName;
        var email = this.state.queueModel.contactEmailAddress;
        var value = this.state.queueModel.query;
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

        this.setState({
            validationStateName: (name == "") ? 'error' : '',
            validationStateEmail: (email != "" && validator.isEmail(email)) ? '' : 'error',
            validationQueryState: hasError ? 'error' : ''
        });
    }

    handleFriendlyNameChange(event) {
        var model = this.state.queueModel;
        model.friendlyName = event.target.value;
        this.setState({ queueModel: model });
        this.validateForm();
    }

    handleRemoteNameChange(event) {
        var model = this.state.queueModel;
        model.name = event.target.value;
        this.setState({ queueModel: model });
    }

    handleRoutingKeyChange(event) {
        var model = this.state.queueModel;
        model.routingKey = event.target.value;
        this.setState({ queueModel: model });
    }

    handleEmailAddressChange(event) {
        var model = this.state.queueModel;
        model.contactEmailAddress = event.target.value;
        this.setState({ queueModel: model });
        this.validateForm();
    }

    handleHoursOutChange(event) {
        var model = this.state.queueModel;
        model.hoursOut = event.target.value;
        this.setState({ queueModel: model });
    }

    handleQueryChange(event) {
        var model = this.state.queueModel;
        model.query = event.target.value;
        this.setState({ queueModel: model });
        this.validateForm();
    }

    handleMultiChange(values) {
        var valArray = values.split(',');
        var found = jQuery.grep(valArray, function (value, i) {
            return value.indexOf("Select All") != -1
        }).length;

        var model = this.state.queueModel;
        if (found)
            model.statusNames = this.state.selectAllArray;
        else
            model.statusNames = valArray;

        this.setState({ queueModel: model });
    }

    handleCheckboxChange(event) {

        var model = this.state.queueModel;
        var checked = event.target.checked;

        switch (event.target.name) {
            case "active":
                model.active = checked;
                break;
            case "allowAiringsWithNoVersion":
                model.allowAiringsWithNoVersion = checked;
                break;
            case "bimRequired":
                model.bimRequired = checked
                break;
            case "report":
                model.report = checked
                break;
            case "isPriorityQueue":
                model.isPriorityQueue = checked
                break;
            case "isProhibitResendMediaId":
                model.isProhibitResendMediaId = checked
                break;
            case "detectTitleChanges":
                model.detectTitleChanges = checked
                break;
            case "detectImageChanges":
                model.detectImageChanges = checked
                break;
            case "detectVideoChanges":
                model.detectVideoChanges = checked
                break;
            case "detectPackageChanges":
                model.detectPackageChanges = checked
                break;
            default:
                throw "Checkbox binding not found for control" + event.target.name;
        }

        this.setState({ queueModel: model });


    }

    render() {
        return (
            <Modal bsSize="large" backdrop="static" className="addEditQueueModel" onEntering={this.resetForm.bind(this, this.props.data.queueDetails.name)} onEntered={this.validateForm.bind(this)} show={this.props.data.showAddEditModel} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                       <div> {this.props.data.queueDetails.id == null ? "New Queue" : "Edit Queue " + this.props.data.queueDetails.friendlyName}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <Grid>
                                <Form>
                                    <Row>
                                        <Col md={4} >
                                            <FormGroup
                                                controlId="friendlyName" validationState={this.state.validationStateName}>
                                                <ControlLabel>Queue Name</ControlLabel>
                                                <FormControl
                                                    type="text"
                                                    value={this.state.queueModel.friendlyName}
                                                    ref="inputFriendlyName"
                                                    placeholder="Enter Queue Friendly Name"
                                                    onChange={this.handleFriendlyNameChange.bind(this)}
                                                />
                                            </FormGroup>
                                        </Col>
                                        <Col md={4}>
                                            <FormGroup
                                                controlId="contactEmail" validationState={this.state.validationStateEmail}>
                                                <ControlLabel>Contact Email &nbsp;<span tooltip data-toggle="tooltip" data-placement="right" title="Queue owner email address." class="glyphicon glyphicon-info-sign"></span></ControlLabel>
                                                <FormControl
                                                    type="text"
                                                    value={this.state.queueModel.contactEmailAddress}
                                                    placeholder="Enter Contact Email"
                                                    onChange={this.handleEmailAddressChange.bind(this)}
                                                />
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col md={4} >
                                            <FormGroup
                                                controlId="hoursOut">
                                                <ControlLabel>Advanced Delivery (hours) &nbsp;<span tooltip data-toggle="tooltip" data-placement="right" title="Number of hours to be notified before flight start." class="glyphicon glyphicon-info-sign"></span></ControlLabel>
                                                <FormControl
                                                    type="text"
                                                    value={this.state.queueModel.hoursOut}
                                                    placeholder="Enter Hours Out"
                                                    onChange={this.handleHoursOutChange.bind(this)}
                                                />
                                            </FormGroup>
                                        </Col>
                                        <Col md={4}>
                                            <FormGroup
                                                controlId="remoteName">
                                                <ControlLabel>Queue ID</ControlLabel>
                                                <FormControl
                                                    type="text"
                                                    disabled={this.props.data.queueDetails.id == null ? false : true}
                                                    value={this.state.queueModel.name}
                                                    placeholder="If blank, guid will be generated."
                                                    onChange={this.handleRemoteNameChange.bind(this)}
                                                />
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col md={4} >
                                            <FormGroup
                                                controlId="isActive">
                                                <Checkbox name="active" onChange={this.handleCheckboxChange.bind(this)}
                                                    checked={this.state.queueModel.active}> Activate Queue to receive notifications. &nbsp;<span tooltip data-toggle="tooltip" data-placement="right" title="Queue will receive notifications only if this is checked." class="glyphicon glyphicon-info-sign"></span></Checkbox>
                                            </FormGroup>
                                        </Col>
                                        <Col md={4}>
                                            <FormGroup
                                                controlId="remoteName">
                                                <ControlLabel>Routing Key</ControlLabel>
                                                <FormControl
                                                    type="text"
                                                    disabled={this.props.data.queueDetails.id == null ? false : true}
                                                    value={this.state.queueModel.routingKey}
                                                    placeholder="If blank, guid will be generated."
                                                    onChange={this.handleRoutingKeyChange.bind(this)}
                                                />
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col md={8}>
                                            <Tabs defaultActiveKey={1} >
                                                <Tab eventKey={1} title="Criteria">
                                                    <Grid>
                                                        <Row>
                                                            <Col md={4}>
                                                                <FormGroup
                                                                    controlId="query" validationState={this.state.validationQueryState}>
                                                                    <ControlLabel>Query 
                                                                        &nbsp;
                                                                        <a target="_mongo" href="http://docs.mongodb.org/master/tutorial/query-documents/">
                                                                            <span tooltip data-toggle="tooltip" data-placement="right" title="Click to view the Mongo query syntax official documentation." class="glyphicon glyphicon-info-sign"></span>
                                                                        </a>
                                                                        &nbsp;&nbsp;
                                                                        <span>{this.state.validationQueryState != '' ? "Please provide valid query" : ""}</span>
                                                                    </ControlLabel>
                                                                    <FormControl
                                                                        bsClass="form-control form-control-modal-Edit"
                                                                        type="text"
                                                                        value={this.state.queueModel.query}
                                                                        placeholder='{"Network" : "TBS"}'
                                                                        onChange={this.handleQueryChange.bind(this)}
                                                                        componentClass="textarea"
                                                                    />
                                                                </FormGroup>
                                                            </Col>
                                                            <Col md={4}>
                                                                <ControlLabel>
                                                                    <Button disabled={this.state.validationQueryState != ''} class="btn-link noPadding" onClick={(event) => this.syntaxChecker(event)}>Validate Query</Button>
                                                                </ControlLabel>
                                                                <FormGroup controlId="queryResults">
                                                                    <FormControl bsClass="form-control form-control-modal-Edit" componentClass="textarea" value={this.state.syntaxCheckerResults} placeholder="Results" />
                                                                </FormGroup>
                                                            </Col>
                                                        </Row>
                                                    </Grid>
                                                </Tab>
                                                <Tab eventKey={2} title="Options">
                                                    <FormGroup>
                                                        <Grid>
                                                            <Row>
                                                                <Col md={4}>
                                                                    <Checkbox name="allowAiringsWithNoVersion" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.allowAiringsWithNoVersion}>Allow Airings/Assets with no version &nbsp; <span tooltip data-toggle="tooltip" data-placement="right" title="This will send notifications even if an airing doesn't have a version number" class="glyphicon glyphicon-info-sign"></span></Checkbox>
                                                                    <Checkbox name="bimRequired" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.bimRequired}> BIM Required &nbsp; <span tooltip data-toggle="tooltip" data-placement="right" title="This will send notifications only if the airing is in BIM" class="glyphicon glyphicon-info-sign"></span> </Checkbox>
                                                                    <Checkbox name="report" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.report}> Report Statuses &nbsp; <span tooltip data-toggle="tooltip" data-placement="right" title="If checked, a status will be sent to Digital Fulfillment before a notification is sent to the queue" class="glyphicon glyphicon-info-sign"></span></Checkbox>
                                                                </Col>
                                                                <Col md={5}>
                                                                    <Checkbox name="isPriorityQueue" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.isPriorityQueue}> Priority Queue &nbsp; <span tooltip data-toggle="tooltip" data-placement="right" title="If checked, notifications for airings with the closest flight window to current timestamp will be listed first" class="glyphicon glyphicon-info-sign"></span></Checkbox>
                                                                    <Checkbox name="isProhibitResendMediaId" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.isProhibitResendMediaId}>Prohibit Resending MediaId to the Queue &nbsp; <span tooltip data-toggle="tooltip" data-placement="right" title="This will prohibit sending notifications with the media ID if the media ID has already been posted in a previous notification" class="glyphicon glyphicon-info-sign"></span></Checkbox>
                                                                </Col>
                                                            </Row>
                                                        </Grid>
                                                    </FormGroup>
                                                </Tab>
                                                <Tab eventKey={3} title="Notifications">
                                                    <FormGroup>
                                                        <Grid>
                                                            <Row>
                                                                <Col md={8}>
                                                                    <p>When the following updates are made, if checked and the Queue is active, a notification will be sent to the Queue</p>
                                                                </Col>
                                                            </Row>
                                                            <Row>
                                                                <Col md={2}>
                                                                    <Checkbox name="detectTitleChanges" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.detectTitleChanges}>Title Changes</Checkbox>
                                                                    <Checkbox name="detectImageChanges" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.detectImageChanges}> Image Changes </Checkbox>
                                                                </Col>
                                                                <Col md={2}>
                                                                    <Checkbox name="detectVideoChanges" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.detectVideoChanges}> Video Changes</Checkbox>
                                                                    <Checkbox name="detectPackageChanges" onChange={this.handleCheckboxChange.bind(this)}
                                                                        checked={this.state.queueModel.detectPackageChanges}> Package Changes</Checkbox>
                                                                </Col>
                                                                <Col md={4}>
                                                                    <ControlLabel>Status Changes</ControlLabel>
                                                                    <Select multi simpleValue className="select-control-width" options={this.state.options} onChange={this.handleMultiChange.bind(this)} value={this.state.queueModel.statusNames} />
                                                                </Col>
                                                            </Row>
                                                        </Grid>
                                                    </FormGroup>
                                                </Tab>
                                            </Tabs>
                                        </Col>
                                    </Row>
                                </Form>
                            </Grid>
                        </div>
                    </div>
                    <NotificationContainer />
                </Modal.Body>
                <Modal.Footer>
                    <Button disabled={this.state.isProcessing} onClick={this.props.handleClose}>Cancel</Button>
                    <Button disabled={(this.state.validationStateName != '' || this.state.validationStateEmail != '' || this.state.validationQueryState != '' || this.state.isProcessing)} onClick={this.handleSave.bind(this)} className="btn btn-primary btn-large">
                        {this.state.isProcessing ? "Processing" : "Save"}
                    </Button>
                </Modal.Footer>
            </Modal>
        )
    }
}
export default DeliveryQueueAddEdit;