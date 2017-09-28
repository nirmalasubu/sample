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
import AddEditContentTierProduct from 'Components/ContentTiers/AddEditContentTierProduct';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import * as contentTierActions from 'Actions/ContentTier/ContentTierActions';
import CancelWarningModal from 'Components/Common/CancelWarningModal';

@connect((store) => {
    return {
        allContentTiers: store.contentTiers,
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
            validationStateProductName: null,
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
    /// To save and update the contentTier to products table
    /// </summary>
    handleSave() {
        var elem = this;
        if (this.state.validationStateName != "error") {

            if (!this.hasValidProducts()) {
                NotificationManager.error('At-least one product required for content tier.', 'Product required');
                return false;
            }
            var model = this.state.contentTierDetails;
            model.name = model.name.trim();
            for (var i = 0; i < model.products.length; i++) {
                if (model.products[i].contentTiers[0].removed)
                    model.products[i].contentTiers[0].name = "";
                else
                    model.products[i].contentTiers[0].name = model.name;
            }

            this.setState({ contentTierDetails: model, isProcessing: true });

            this.props.dispatch(contentTierActions.saveContentTier(this.state.contentTierDetails))
                .then(() => {
                    if (this.state.contentTierDetails.id == null) {
                        NotificationManager.success(this.state.contentTierDetails.name + ' content tier successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(this.state.contentTierDetails.name + ' content tier updated successfully.', '', 2000);
                    }
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    if (this.state.contentTierDetails.id == null) {
                        NotificationManager.error(this.state.contentTierDetails.name + ' content tier creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(this.state.contentTierDetails.name + ' content tier update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
    }

    hasValidProducts() {
        var hasOneProduct = false;
        for (var i = 0; i < this.state.contentTierDetails.products.length; i++) {
            if (this.state.contentTierDetails.products[i].contentTiers[0].removed == undefined
                && this.state.contentTierDetails.products[i].name.length > 2) {
                hasOneProduct = true;
            }
        }

        return hasOneProduct;
    }

    //called from cancel warning component to close add edit pop up
    handleAddEditClose() {
        var model = this.state.productUnModifiedData;
        jQuery.extend(this.state.contentTierDetails, this.state.contentTierUnModifiedData);

        this.setState({ validationStateProductName: null });
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
            var model = this.state.contentTierDetails;
            if (model.name == "" && model.products.length == 1 && model.products[0].name == "") {
                this.props.handleClose();
            }
            else
                this.openWarningModel();
        }
    }

    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveDisabled() {
        return (this.state.validationStateName != null || this.state.validationStateUniqueName != null || this.state.isProcessing);
    }

    /// <summary>
    /// This function is to set validations states value
    /// </summary>
    validateForm() {
        var name = this.state.contentTierDetails.name.trim();
        var hasNameError = (name == "");

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
        model.name = model.name.trimLeft();

        this.setState({
            contentTierDetails: model
        });

        this.validateForm();
    }

    //callback function to update the validation
    updateProductNameValidation(IsProductNameRequired) {
        this.setState({ validationStateProductName: (IsProductNameRequired == true) ? 'error' : null });
    }

    /// <summary>
    /// To validate the contentTier name is unique
    /// </summary>
    isNameUnique(contentTierDetails) {
        for (var x = 0; x < this.props.allContentTiers.length; x++) {
            if (this.props.allContentTiers[x].id != contentTierDetails.id) {
                if (this.props.allContentTiers[x].name == contentTierDetails.name.trim()) {
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
        var initialContentTier = { "id": null, "name": "", "products": [] };

        //required to overcome form control warning of contentTierName
        this.setState({
            contentTierDetails: initialContentTier
        });

    }

    render() {
        var saveButton = null;
        if (this.props.permissions.canAddOrEdit) {
            saveButton = <Button disabled={this.isSaveDisabled()} onClick={this.handleSave.bind(this)} className="btn btn-primary btn-large">
                {this.state.isProcessing ? "Processing" : "Save"}
            </Button>
        }
        var msg = "";
        if (this.state.showError)
            msg = (<label data-ng-show="showError" class="alert alert-danger"><strong>Error!</strong> Content Tier Name already exists. Please use a unique Content Tier name.</label>);

        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} onEntered={this.onEnteredModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{this.props.data.contentTierDetails.id != null ? (this.props.permissions.canAddOrEdit ? "Edit Content Tier - " : "") + this.props.data.contentTierDetails.name : "Add Content Tier"}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        {msg}
                        <FormGroup
                            controlId="contentTierName" validationState={this.state.validationStateName}>
                            <ControlLabel>Content Tier Name</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.contentTierDetails.name}
                                disabled={this.state.contentTierDetails.id != null}
                                ref="inputContentTierName"
                                placeholder="Enter a Content Tier Name"
                                onChange={this.handleTextChange.bind(this)}
                                />
                        </FormGroup>

                        <AddEditContentTierProduct permissions={this.props.permissions} data={this.props.data.contentTierDetails} validationStates={this.updateProductNameValidation.bind(this)} />
                        <NotificationContainer />
                        <CancelWarningModal data={this.state} handleClose={this.closeWarningModel.bind(this)} handleAddEditClose={this.handleAddEditClose.bind(this)} />
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button disabled={this.state.isProcessing} onClick={this.handleClose.bind(this)}>{this.props.permissions.canAddOrEdit ? "Cancel" : "Close"}</Button>
                    {saveButton}
                </Modal.Footer>
            </Modal>
        )
    }
}

export default AddEditContentTier;