import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Tabs, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import AddEditDestinationBasic from 'Components/Destinations/AddEditDestinationBasic';
import AddEditDestinationDeliverables from 'Components/Destinations/AddEditDestinationDeliverables';
import AddEditDestinationProperties from 'Components/Destinations/AddEditDestinationProperties';
import AddEditDestinationCategories from 'Components/Destinations/AddEditDestinationCategories';
import { saveDestination } from 'Actions/Destination/DestinationActions';
import * as destinationAction from 'Actions/Destination/DestinationActions';
import CancelWarningModal from 'Components/Common/CancelWarningModal';

@connect((store) => {
    return {

    };
})

class AddEditDestinationModel extends React.Component {

    constructor(props) {
        super(props);

        this.state = ({
            isProcessing: false,
            destinationDetails: {
                name: "",
                description: "",
                deliverables: [],
                properties: [],
                categories: [],
                content: { cx: false, highDefinition: false, nonCx: false, standardDefinition: false }
            },
            validationStateName: null,
            validationStateDescription: null,
            validationStateExternalId: null,
            validationStatePropertyName: null,
            validationStateDeliverables: null,
            showWarningModel: false
        });
    }
    //called on the model load
    onOpenModel(destination) {
        var model = $.extend(true, {}, destination);        
        this.setState({
            isProcessing: false,
            destinationDetails: model
        });
    }

    openWarningModel() {
        this.setState({ showWarningModel: true });
    }

    closeWarningModel() {
        this.setState({ showWarningModel: false });
    }

    handleSave() {
        var elem = this;
        if (this.state.validationStateName != "error" && this.state.validationStateDescription != "error") {

            this.setState({ isProcessing: true });

            this.props.dispatch(saveDestination(this.state.destinationDetails))
                .then(() => {
                    if (this.state.destinationDetails.id == null) {
                        NotificationManager.success(this.state.destinationDetails.name + ' destination successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(this.state.destinationDetails.name + ' destination updated successfully.', '', 2000);
                    }
                    this.props.dispatch(destinationAction.fetchDestinations());
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    if (this.state.destinationDetails.id == null) {
                        NotificationManager.error(this.state.destinationDetails.name + ' destination creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(this.state.destinationDetails.name + ' destination update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
    }

    handleAddEditClose() {
        this.props.handleClose();
    }

    handleClose() {
        if (JSON.stringify(this.state.destinationDetails) == JSON.stringify(this.props.data.destinationDetails)) {
            this.props.handleClose();
        }
        else {
            this.openWarningModel();
        }
    }

    updateBasicValidateStates(name, description) {
        this.setState({
            validationStateName: name ? 'error' : null,
            validationStateDescription: description ? 'error' : null
        });
    }

    updatePropertyNameValidation(IspropertyNameRequired) {
        this.setState({ validationStatePropertyName: (IspropertyNameRequired == true) ? 'error' : null });
    }


    /// <summary>
    /// Verify if the state for deliverables component changed. If so, set the local
    /// property of this component to reflect that change. This will enable this component
    /// to adjust the rendering of its controls
    /// </summary>
    updateDeliverablesValidation(HasStateChanged) {
        this.setState({ validationStateDeliverables: (HasStateChanged == true) ? 'changed' : null });
    }

    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveEnabled() {
        return (this.state.validationStateName != null || this.state.validationStateDescription != null
            || this.state.validationStatePropertyName != null || this.state.isProcessing || this.state.validationStateDeliverables != null);
    }

    updateDestination(destination) {
        this.setState({ destinationDetails: destination });
    }

    render() {
        return (
            <Modal bsSize="large" backdrop="static"
                onEntering={this.onOpenModel.bind(this, this.props.data.destinationDetails)}
                show={this.props.data.showAddEditModel}
                onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{this.props.data.destinationDetails.id == null ? "Add Destination" : "Edit Destination " + this.props.data.destinationDetails.name}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <AddEditDestinationBasic
                        updateDestination={this.updateDestination.bind(this)}
                        data={this.state.destinationDetails}
                        validationStates={this.updateBasicValidateStates.bind(this)} />
                    <Tabs id="addeditdestination" defaultActiveKey={1} >
                        <Tab eventKey={1} title="Properties">
                            <AddEditDestinationProperties
                                updateDestination={this.updateDestination.bind(this)}
                                data={this.state.destinationDetails}
                                validationStates={this.updatePropertyNameValidation.bind(this)} />
                        </Tab>
                        <Tab eventKey={2} title="Deliverables">
                            <AddEditDestinationDeliverables
                                updateDestination={this.updateDestination.bind(this)}
                                data={this.state.destinationDetails}
                                validationStates={this.updateDeliverablesValidation.bind(this)} />
                        </Tab>
                        <Tab eventKey={3} title="Categories">
                            <AddEditDestinationCategories
                                updateDestination={this.updateDestination.bind(this)}
                                data={this.state.destinationDetails} />
                        </Tab>
                    </Tabs>
                    <NotificationContainer />
                    <CancelWarningModal
                        data={this.state}
                        handleClose={this.closeWarningModel.bind(this)}
                        handleAddEditDestinationClose={this.handleAddEditClose.bind(this)} />
                </Modal.Body>
                <Modal.Footer>
                    <Button disabled={this.state.isProcessing} onClick={this.handleClose.bind(this)}>Cancel</Button>
                    <Button disabled={this.isSaveEnabled()} onClick={this.handleSave.bind(this)} className="btn btn-primary btn-large">
                        {this.state.isProcessing ? "Processing" : "Save"}
                    </Button>
                </Modal.Footer>
            </Modal>

        )
    }
}

export default AddEditDestinationModel;