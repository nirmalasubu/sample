import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import Moment from 'moment';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

@connect((store) => {
    return {
        notificationHistory: store.notificationHistory,
        config: store.config
    };
})
class DeliveryQueueAddEdit extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({ queueModel: {},
            options : [
            { value: 'Edited', label: 'Edited' },
            { value: 'Encoding', label: 'Encoding' },
            { value: 'Medium', label: 'Medium' },
            ],
            selectedOptions : []
        });
    }
    resetForm(queueName) {
        var model = $.extend(true, {}, this.props.data.queueDetails);
        this.setState({ queueModel: model });
        console.log(model);
    }

    handleFriendlyNameChange(event) {
        var model = this.state.queueModel;
        model.friendlyName = event.target.value;
        this.setState({ queueModel: model });
    }

    handleRemoteNameChange(event) {
        var model = this.state.queueModel;
        model.name = event.target.value;
        this.setState({ queueModel: model });
    }

    handleEmailAddressChange(event) {
        var model = this.state.queueModel;
        model.contactEmailAddress = event.target.value;
        this.setState({ queueModel: model });
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
    }

    handleMultiChange(values) {
        this.setState({ selectedOptions: values })
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
                    <Grid>
                    <Form>
                        <Row>
                            <Col md={4} >
                                <FormGroup
                                    controlId="friendlyName">                            
                                    <ControlLabel>Queue Name</ControlLabel>
                                    <FormControl
                                        type="text"
                                        value={this.state.queueModel.friendlyName}
                                        placeholder="Enter Queue Friendly Name"
                                        onChange={this.handleFriendlyNameChange.bind(this)}
                                    />
                                </FormGroup>
                            </Col>
                            <Col md={4}>
                                <FormGroup
                                    controlId="contactEmail">
                                    <ControlLabel>Contact Email</ControlLabel>
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
                                    <ControlLabel>Hours Out</ControlLabel>
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
                                    <ControlLabel>Remote Name</ControlLabel>
                                    <FormControl
                                        type="text"
                                        value={this.state.queueModel.name}
                                        placeholder="Enter Remote Name"
                                        onChange={this.handleRemoteNameChange.bind(this)}
                                    />
                                </FormGroup>
                            </Col>
                        </Row>
                        <Row>
                            <Col md={9}>
                                <Tabs defaultActiveKey={1} >
                                    <Tab eventKey={1} title="Criteria">
                                        <FormGroup
                                            controlId="remoteName">
                                            <ControlLabel>Query</ControlLabel>
                                            <FormControl
                                                type="text"
                                                value={this.state.queueModel.query}
                                                placeholder="{'Network':'TBS'}"
                                                onChange={this.handleQueryChange.bind(this)}
                                                componentClass="textarea"
                                            />
                                        </FormGroup>
                                    </Tab>
                                    <Tab eventKey={2} title="Options">
                                        <FormGroup>
                                            <Checkbox checked={this.state.queueModel.allowAiringsWithNoVersion}>Allow Airings/Assets with no version</Checkbox>
                                            <Checkbox checked={this.state.queueModel.bimRequired}> BIM Required </Checkbox>
                                            <Checkbox checked={this.state.queueModel.report}> Report Statuses</Checkbox>
                                            <Checkbox checked={this.state.queueModel.isPriorityQueue}> Priority Queue</Checkbox>
                                            <Checkbox checked={this.state.queueModel.isProhibitResendMediaId}>Prohibit Resending MediaId to the Queue</Checkbox>
                                        </FormGroup>
                                    </Tab>
                                    <Tab eventKey={3} title="Notifications">
                                        <FormGroup>
                                            <Checkbox checked={this.state.queueModel.detectTitleChanges}>Title Change</Checkbox>
                                            <Checkbox checked={this.state.queueModel.detectImageChanges}> Image Changes </Checkbox>
                                            <Checkbox checked={this.state.queueModel.detectVideoChanges}> Video Changes</Checkbox>
                                            <Checkbox checked={this.state.queueModel.detectPackageChanges}> Package Changes</Checkbox>
                                            <ControlLabel>Status Changes</ControlLabel>
                                            <Select multi simpleValue options={this.state.options}  onChange={ this.handleMultiChange.bind(this)} value={this.state.selectedOptions} />                                            
                                        </FormGroup>
                                    </Tab>
                                </Tabs>
                            </Col>
                        </Row>
                    </Form>
                    </Grid>
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