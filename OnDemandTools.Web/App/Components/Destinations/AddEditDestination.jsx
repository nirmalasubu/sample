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
import { saveDestination } from 'Actions/Destination/DestinationActions';
import * as destinationAction from 'Actions/Destination/DestinationActions';

@connect((store) => {
    return {
        config: store.config
    };
})

class AddEditDestinationModel extends React.Component {

    constructor(props) {
        super(props);

        this.state = ({
            isProcessing: false,
            destinationUnModifiedData:"",
            validationStateName: "",
            validationStateDescription: "",
            validationStateExternalId: ""
        });
    }
    //called on the model load
    onOpenModel(destination) {
        this.setState({ 
            isProcessing: false,
            destinationUnModifiedData: jQuery.extend(true, {}, this.props.data.destinationDetails)
        });
    }

    handleSave() {
        var elem = this;
        if (this.state.validationStateName != "error" && this.state.validationStateDescription != "error") {

            this.setState({ isProcessing: true });

            this.props.dispatch(saveDestination(this.props.data.destinationDetails))
                    .then(() => {
                        if (this.props.data.destinationDetails.id == null) {
                            NotificationManager.success(this.props.data.destinationDetails.name + ' destination successfully created.', '', 2000);
                        }
                        else {
                            NotificationManager.success(this.props.data.destinationDetails.name + ' destination updated successfully.', '', 2000);
                        }
                        this.props.dispatch(destinationAction.fetchDestinations());
                        setTimeout(function () {
                            elem.props.handleClose();
                        }, 3000);
                    }).catch(error => {
                        if (this.props.data.destinationDetails.id == null) {
                            NotificationManager.error(this.props.data.destinationDetails.name + ' destination creation failed. ' + error, 'Failure');
                        }
                        else {
                            NotificationManager.error(this.props.data.destinationDetails.name + ' destination update failed. ' + error, 'Failure');
                        }
                        this.setState({ isProcessing: false });
                    });
        }
        else
            return false;
    }

    handleClose(){
        if (JSON.stringify(this.state.destinationUnModifiedData)==JSON.stringify(this.props.data.destinationDetails)) {
            this.props.handleClose();
        }
        else
            this.props.handleCancelWarning();
    }

    updateValidations(name, description)
    {
        this.setState({
            validationStateName : (name!=undefined && (name == "" || name.length < 3 || name.length > 5)) ? 'error' : '',
            validationStateDescription : (description == "") ? 'error' : ''
        });
    }

    render() {
        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this, this.props.data.destinationDetails)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>Edit Destination {this.props.data.destinationDetails.name}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <AddEditDestinationBasic data={this.props.data.destinationDetails} validationStates={this.updateValidations.bind(this)} />
                    <Tabs defaultActiveKey={1} >
                        <Tab eventKey={1} title="Properties">
                            <AddEditDestinationProperties data={this.props.data.destinationDetails} />
                        </Tab>
                        <Tab eventKey={2} title="Deliverables">
                            <AddEditDestinationDeliverables data={this.props.data.destinationDetails} />
                        </Tab>
                    </Tabs>
                    <NotificationContainer />
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.handleClose.bind(this)}>Cancel</Button> 
                    <Button disabled={(this.state.validationStateName != '' || this.state.validationStateDescription != '' || this.state.isProcessing)} onClick={this.handleSave.bind(this)} className="btn btn-primary btn-large">
                        {this.state.isProcessing ? "Processing" : "Save"}
                    </Button>
                </Modal.Footer>
            </Modal>
        )
            }
            }

export default AddEditDestinationModel;