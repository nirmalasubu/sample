import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Tabs, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, Panel } from 'react-bootstrap';
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import AddEditProductBasic from 'Components/Products/AddEditProductBasic';
import AddEditProductDestination from 'Components/Products/AddEditProductDestination';
import { saveProduct } from 'Actions/Product/ProductActions';
import * as productAction from 'Actions/Product/ProductActions';
import CancelWarningModal from 'Components/Common/CancelWarningModal';

@connect((store) => {
    return {

    };
})

///<summary>
/// Sub component of product page to  add ,edit and delete product details
///</summary>
class AddEditProduct extends React.Component {

    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);

        this.state = ({
            isProcessing: false,
            productDetails: {
                externalId: "",
                name: "",
                description: "",
                mappingId: "",
                dynamicAdTrigger: false,
                tags: []
            },
            validationState: null,
            showWarningModel: false
        });
    }
    /// <summary>
    /// This function is called on entering the modal pop up
    /// </summary>
    onOpenModel(product) {
        //var model = $.extend(true, {}, product);

        this.setState({
            isProcessing: false,
            productDetails: product
        });
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
    /// To save and update the product details
    /// </summary>
    handleSave() {
        var elem = this;
        if (this.state.validationStateName != "error" && this.state.validationStateDescription != "error") {

            this.setState({ isProcessing: true });

            var productToSave = this.state.productDetails;

            this.props.dispatch(saveProduct(productToSave))
                .then(() => {
                    if (this.state.productDetails.id == null) {
                        NotificationManager.success(productToSave.name + ' product successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(productToSave.name + ' product updated successfully.', '', 2000);
                    }

                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);

                }).catch(error => {
                    if (productToSave.id == null) {
                        NotificationManager.error(productToSave.name + ' product creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(productToSave.name + ' product update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
    }

    /// <summary>
    /// called from cancel warning component to close add edit pop up
    /// </summary>
    handleAddEditClose() {
        this.props.handleClose();
    }

    /// <summary>
    /// Called to close the add edit pop up or open cancel warning pop up
    /// </summary>
    handleClose() {
        if (JSON.stringify(this.state.destinationDetails) == JSON.stringify(this.props.data.destinationDetails)) {
            this.props.handleClose();
        }
        else {
            this.openWarningModel();
        }
    }

    /// <summary>
    /// Verify if the state for Basic product info component changed. If so, set the local
    /// property of this component to reflect that change. This will enable/disable the
    /// save button
    /// </summary>
    updateBasicValidateStates(error) {
        this.setState({
            validationState: error ? 'error' : null
        });
    }

    /// <summary>
    /// Determine whether save button needs to be enabled or not based on the validation states value
    /// </summary>
    isSaveEnabled() {
        return (this.state.validationState != null);
    }

    render() {
        return (
            <Modal bsSize="large" backdrop="static"
                onEntering={this.onOpenModel.bind(this, this.props.data.productDetails)}
                show={this.props.data.showAddEditModel}
                onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{this.props.data.productDetails.id == null ? "Add Product" : "Edit Product " + this.props.data.productDetails.name}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <AddEditProductBasic
                        data={this.props.data.productDetails}
                        validationStates={this.updateBasicValidateStates.bind(this)} />
                    <Panel header="Destination" >
                        <AddEditProductDestination
                            data={this.props.data.productDetails} />
                    </Panel>
                    <NotificationContainer />
                    <CancelWarningModal
                        data={this.state}
                        handleClose={this.closeWarningModel.bind(this)}
                        handleAddEditClose={this.handleAddEditClose.bind(this)} />
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

export default AddEditProduct;