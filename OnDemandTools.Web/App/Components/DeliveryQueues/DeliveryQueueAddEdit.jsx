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

@connect((store) => {
    return {
        notificationHistory: store.notificationHistory,
        config: store.config
    };
})
class DeliveryQueueAddEdit extends React.Component {

    constructor(props) {
        super(props);

        this.state = ({ queueModel: {} });
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

    render() {
        return (
            <Modal bsSize="large" onEntering={this.resetForm.bind(this, this.props.data.queueDetails.name)} show={this.props.data.showAddEditModel} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Edit Queue {this.props.data.queueDetails.friendlyName}</p>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>

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
                                    <Checkbox>Allow Airings/Assets with no version</Checkbox>
                                    <Checkbox> BIM Required </Checkbox>
                                    <Checkbox> Report Statuses</Checkbox>
                                    <Checkbox> Priority Queue</Checkbox>
                                    <Checkbox>Prohibit Resending MediaId to the Queue</Checkbox>
                                </FormGroup>
                            </Tab>
                            <Tab eventKey={3} title="Notifications">
                                <FormGroup>
                                    <Checkbox>Title Change</Checkbox>
                                    <Checkbox> Image Changes </Checkbox>
                                    <Checkbox> Video Changes</Checkbox>
                                    <Checkbox> Package Changes</Checkbox>
                                    <ControlLabel>Status Changes</ControlLabel>
                                </FormGroup>
                            </Tab>
                        </Tabs>
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