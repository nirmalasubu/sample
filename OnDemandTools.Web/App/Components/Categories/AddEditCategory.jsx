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
            categoryDetails:""
        });
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
        return (this.state.validationStateName != null );
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

        //this.validateForm();
    }

    render() {
        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{($.inArray(this.state.categoryDetails.name, this.state.categoryDetails.destinations) > -1) ? "Edit Category -" + this.state.categoryDetails.name : "Add Category"}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        <FormGroup
                            controlId="categoryName" validationState={this.state.validationStateName}>
                            <ControlLabel>Category Name</ControlLabel>
                            <FormControl
                            type="text"
                            disabled={($.inArray(this.state.categoryDetails.name, this.state.categoryDetails.destinations) > -1) ? true : false}
                            value={this.state.categoryDetails.name}
                            ref="inputCategoryName"
                            placeholder="Enter a Category Name"
                            onChange={this.handleTextChange.bind(this)}
                            />
                        </FormGroup>
                        <Tabs defaultActiveKey={1} >
                            <Tab eventKey={1} >
                                <AddEditCategoryDestination data={this.state.categoryDetails} />
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