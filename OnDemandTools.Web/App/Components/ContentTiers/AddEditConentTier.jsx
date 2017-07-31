import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Tabs, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, Well, Panel } from 'react-bootstrap';
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import AddEditContentTierProduct from 'Components/Categories/AddEditContentTierProduct';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import * as contentTierActions from 'Actions/ContentTier/ContentTierActions';
import CancelWarningModal from 'Components/Common/CancelWarningModal';

@connect((store) => {
    return {

    };
})

// Sub component of contentTier page to  add ,edit and delete contentTier details
class AddEditContentTier extends React.Component {
    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);

        this.state = ({
            contentTierUnModifiedData: "",
            isProcessing: false,
            validationStateName: null,
            validationStateDestinationName: null,
            contentTierDetails: {},
            showWarningModel: false,
            showError: false,
            validationStateUniqueName: null
        });
    }

    /// <summary>
    /// This function is called after entering the modal pop up
    /// </summary>
    onEnteredModel() {
        this.validateForm();
    }

    /// <summary>
    /// This function is called on entering the modal pop up
    /// </summary>
    onOpenModel() {
        this.setState({
            isProcessing: false,
            contentTierDetails: this.props.data.contentTierDetails,
            contentTierUnModifiedData: jQuery.extend(true, {}, this.props.data.contentTierDetails)
        });

    }

    /// <summary>
    /// To save and update the contentTier to destinations table
    /// </summary>
    handleSave() {
        var elem = this;
        if (this.state.validationStateName != "error") {

            if (!this.hasValidDestinations()) {
                NotificationManager.error('At-least one destination required for contentTier.', 'Destination required');
                return false;
            }
            var model = this.state.contentTierDetails;

            for (var i = 0; i < model.destinations.length; i++) {
                if (model.destinations[i].contentTiers[0].removed)
                    model.destinations[i].contentTiers[0].name = "";
                else
                    model.destinations[i].contentTiers[0].name = model.name;
            }

            this.setState({ contentTierDetails: model, isProcessing: true });

            this.props.dispatch(contentTierActions.saveContentTier(this.state.contentTierDetails))
                .then(() => {
                    if (this.state.contentTierDetails.id == null) {
                        NotificationManager.success(this.state.contentTierDetails.name + ' contentTier successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(this.state.contentTierDetails.name + ' contentTier updated successfully.', '', 2000);
                    }
                    this.props.dispatch(contentTierActions.fetchCategories()); // Called to refresh the page data
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    if (this.state.contentTierDetails.id == null) {
                        NotificationManager.error(this.state.contentTierDetails.name + ' contentTier creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(this.state.contentTierDetails.name + ' contentTier update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
    }

    hasValidDestinations() {
        var hasOneDestination = false;
        for (var i = 0; i < this.state.contentTierDetails.destinations.length; i++) {
            if (this.state.contentTierDetails.destinations[i].contentTiers[0].removed == undefined
                && this.state.contentTierDetails.destinations[i].name.length > 2) {
                hasOneDestination = true;
            }
        }

        return hasOneDestination;
    }

    //called from cancel warning component to close add edit pop up
    handleAddEditClose() {
        var model = this.state.destinationUnModifiedData;
        jQuery.extend(this.state.contentTierDetails, this.state.contentTierUnModifiedData);

        this.setState({ validationStateDestinationName: null });
        this.props.handleClose();
    }

    /// <summary>
    /// Called to open the cancel warning pop up
    /// </summary>
    openWarningModel() {
        this.setState({ showWarningModel: true });
    }

    /// <summary>
    /// Called to close the cancel warning pop up
    /// </summary>
    closeWarningModel() {
        this.setState({ showWarningModel: false });
    }

    /// <summary>
    /// Called to close the add edit pop up or open cancel warning pop up
    /// </summary>
    handleClose() {
        if (JSON.stringify(this.state.contentTierUnModifiedData) == JSON.stringify(this.state.contentTierDetails)) {
            this.props.handleClose();
        }
        else {
            this.openWarningModel();
        }
    }

    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveEnabled() {
        return (this.state.validationStateName != null || this.state.validationStateUniqueName != null);
    }

    /// <summary>
    /// This function is to set validations states value
    /// </summary>
    validateForm() {
        var name = this.state.contentTierDetails.name;
        var hasNameError = (name == "")


        this.setState({
            validationStateName: hasNameError ? 'error' : null,
            validationStateUniqueName: this.isNameUnique(this.state.contentTierDetails) ? 'error' : null
        });
    }

    /// <summary>
    /// Updating the contentTier name in the state on change of text
    /// </summary>
    handleTextChange(event) {

        var model = this.state.contentTierDetails;

        model.name = event.target.value;

        this.setState({
            contentTierDetails: model
        });

        this.validateForm();
    }

    //callback function to update the validation
    updateDestinationNameValidation(IsDestinationNameRequired) {
        this.setState({ validationStateDestinationName: (IsDestinationNameRequired == true) ? 'error' : null });
    }

    /// <summary>
    /// To validate the contentTier name is unique
    /// </summary>
    isNameUnique(contentTierDetails) {

        for (var x = 0; x < this.props.contentTierIdandNames.length; x++) {

            if (this.props.contentTierIdandNames[x].id != contentTierDetails.id) {
                if (this.props.contentTierIdandNames[x].name == contentTierDetails.name) {
                    this.setState({
                        showError: true
                    });

                    return true;
                }
                else {
                    this.setState({
                        showError: false
                    });
                }
            }
        }

        return false;
    }

    componentDidMount() {
        var initialContentTier={ "id": null, "name": "", "destinations":[]};

        //required to overcome form control warning of contentTierName
        this.setState({
            contentTierDetails: initialContentTier 
        });
      
    }

    render() {
        var msg = "";
        if (this.state.showError)
            msg = (<label data-ng-show="showError" class="alert alert-danger"><strong>Error!</strong> ContentTier Name already exists. Please use a unique contentTier name.</label>);

        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} onEntered={this.onEnteredModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{this.props.data.contentTierDetails.id != null ? "Edit ContentTier -" + this.props.data.contentTierDetails.name : "Add ContentTier"}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        {msg}
                        <FormGroup
                            controlId="contentTierName" validationState={this.state.validationStateName}>
                            <ControlLabel>ContentTier Name</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.contentTierDetails.name}
                                disabled={this.state.contentTierDetails.id != null}
                                ref="inputContentTierName"
                                placeholder="Enter a ContentTier Name"
                                onChange={this.handleTextChange.bind(this)}
                            />
                        </FormGroup>

                        <AddEditContentTierProduct data={this.props.data.contentTierDetails} validationStates={this.updateDestinationNameValidation.bind(this)} />
                        <NotificationContainer />
                        <CancelWarningModal data={this.state} handleClose={this.closeWarningModel.bind(this)} handleAddEditClose={this.handleAddEditClose.bind(this)} />
                    </div>
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

export default AddEditContentTier;