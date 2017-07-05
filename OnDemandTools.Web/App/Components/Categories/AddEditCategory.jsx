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
import AddEditCategoryDestination from 'Components/Categories/AddEditCategoryDestination';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import * as categoryActions from 'Actions/Category/CategoryActions';
import CancelWarningModal from 'Components/Common/CancelWarningModal';

@connect((store) => {
    return {

    };
})

// Sub component of category page to  add ,edit and delete category details
class AddEditCategory extends React.Component {
    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);

        this.state = ({
            categoryUnModifiedData:"",
            isProcessing: false,
            validationStateName: null,
            validationStateDestinationName: null,
            categoryDetails:{},
            showWarningModel: false
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
            categoryDetails: this.props.data.categoryDetails,
            categoryUnModifiedData: jQuery.extend(true, {}, this.props.data.categoryDetails)
        });       
        
    }

    /// <summary>
    /// To save and update the category to destinations table
    /// </summary>
    handleSave() {
        var elem = this;
        if (this.state.validationStateName != "error" && this.state.validationStateDestinationName != "error") {
            var model = this.state.categoryDetails;

            for(var i=0; i<model.destinations.length-1; i++)
                model.destinations[i].categories[0].name = model.name;

            this.setState({categoryDetails: model, isProcessing: true});

            this.props.dispatch(categoryActions.saveCategory(this.state.categoryDetails))
                .then(() => {
                    if (this.state.categoryDetails.id == null) {
                        NotificationManager.success(this.state.categoryDetails.name + ' category successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(this.state.categoryDetails.name + ' category updated successfully.', '', 2000);
                    }
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    if (this.state.categoryDetails.id == null) {
                        NotificationManager.error(this.state.categoryDetails.name + ' category creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(this.state.categoryDetails.name + ' category update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
    }

    //called from cancel warning component to close add edit pop up
    handleAddEditClose() {
        var model = this.state.destinationUnModifiedData;
        jQuery.extend(this.state.categoryDetails, this.state.categoryUnModifiedData);
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
        if (JSON.stringify(this.state.categoryUnModifiedData) == JSON.stringify(this.state.categoryDetails)) {
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
        return (this.state.validationStateName != null || this.state.validationStateDestinationName != null);
    }

    /// <summary>
    /// This function is to set validations states value
    /// </summary>
    validateForm() {
        var name = this.state.categoryDetails.name;
        var hasNameError = (name == "");

        this.setState({
            validationStateName: hasNameError  ? 'error' : null
        });
    }

    /// <summary>
    /// Updating the category name in the state on change of text
    /// </summary>
    handleTextChange(event) {       

        var model = this.state.categoryDetails;
                
        model.name = event.target.value;

        this.setState({
            categoryDetails:model
        });

        this.validateForm();
    }

    //callback function to update the validation
    updateDestinationNameValidation(IsDestinationNameRequired) {
        this.setState({ validationStateDestinationName: (IsDestinationNameRequired == true) ? 'error' : null });
    }

    render() {        
        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} onEntered={this.onEnteredModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{(this.props.data.categoryDetails.destinations!=undefined && this.props.data.categoryDetails.destinations.length>0 && this.props.data.categoryDetails.destinations[0].categories[0].id!=undefined) ? "Edit Category -" + this.props.data.categoryDetails.name : "Add Category"}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        <FormGroup
                            controlId="categoryName" validationState={this.state.validationStateName}>
                            <ControlLabel>Category Name</ControlLabel>
                            <FormControl
                            type="text"
                            disabled={(this.props.data.categoryDetails.destinations!=undefined && this.props.data.categoryDetails.destinations.length>0 && this.props.data.categoryDetails.destinations[0].categories[0].id!=undefined) ? true : false}
                            value={this.state.categoryDetails.name}
                            ref="inputCategoryName"
                            placeholder="Enter a Category Name"
                            onChange={this.handleTextChange.bind(this)}
                            />
                        </FormGroup>
                        <Panel bsStyle="category">
                            <AddEditCategoryDestination data={this.props.data.categoryDetails} validationStates={this.updateDestinationNameValidation.bind(this)} />
                        </Panel>
                        <NotificationContainer />
                        <CancelWarningModal data={this.state} handleClose={this.closeWarningModel.bind(this)} handleAddEditClose={this.handleAddEditClose.bind(this)} />
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.handleClose.bind(this)}>Cancel</Button>
                    <Button disabled={this.isSaveEnabled()} onClick={this.handleSave.bind(this)} className="btn btn-primary btn-large">
                        {this.state.isProcessing ? "Processing" : "Save"}
                    </Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default AddEditCategory;