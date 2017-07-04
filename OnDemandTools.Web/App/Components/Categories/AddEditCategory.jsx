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
import AddEditCategoryDestination from 'Components/Categories/AddEditCategoryDestination';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import * as categoryActions from 'Actions/Category/CategoryActions';

@connect((store) => {
    return {

    };
})

class AddEditCategory extends React.Component {

    constructor(props) {
        super(props);

        this.state = ({
            isProcessing: false,
            validationStateName: null,
            validationStateDestinationName: null,
            categoryDetails:{}
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
            categoryDetails: this.props.data.categoryDetails
        });       
        
    }

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

    handleAddEditClose() {
        //var model = this.state.destinationUnModifiedData;
        //jQuery.extend(this.props.data.destinationDetails, this.state.destinationUnModifiedData);
        this.props.handleClose();
    }

    /// <summary>
    /// Called to close the pop up
    /// </summary>
    handleClose() {
        //if (JSON.stringify(this.state.destinationUnModifiedData) == JSON.stringify(this.props.data.destinationDetails)) {
              this.props.handleClose();
        //}
        //else {
        //    this.openWarningModel();
        //}
    }

    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveEnabled() {
        return (this.state.validationStateName != null || this.state.validationStateDestinationName != null);
    }


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

    updateDestinationDetail(destinations)
    {

    }

    render() {        
        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} onEntered={this.onEnteredModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{($.inArray(this.props.data.categoryDetails.name, this.props.data.categoryDetails.destinations) > -1) ? "Edit Category -" + this.props.data.categoryDetails.name : "Add Category"}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        <FormGroup
                            controlId="categoryName" validationState={this.state.validationStateName}>
                            <ControlLabel>Category Name</ControlLabel>
                            <FormControl
                            type="text"
                            disabled={($.inArray(this.props.data.categoryDetails.name, this.props.data.categoryDetails.destinations) > -1) ? true : false}
                            value={this.state.categoryDetails.name}
                            ref="inputCategoryName"
                            placeholder="Enter a Category Name"
                            onChange={this.handleTextChange.bind(this)}
                            />
                        </FormGroup>
                        <Tabs defaultActiveKey={1} >
                            <Tab eventKey={1} >
                                <AddEditCategoryDestination data={this.props.data.categoryDetails} validationStates={this.updateDestinationNameValidation.bind(this)} updateDetails={this.updateDestinationDetail.bind(this)} />
                            </Tab>
                        </Tabs>
                        <NotificationContainer />
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