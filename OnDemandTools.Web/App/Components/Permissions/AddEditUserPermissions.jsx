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
import { NotificationContainer, NotificationManager } from 'react-notifications';
import * as statusActions from 'Actions/Status/StatusActions';
import CancelWarningModal from 'Components/Common/CancelWarningModal';
import AddEditUserPersonalInformation from 'Components/Permissions/AddEditUserPersonalInformation';
import AddEditUserBasicInformation from 'Components/Permissions/AddEditUserBasicInformation';
@connect((store) => {
    return {

    };
})

// Sub component of category page to  add ,edit and delete category details
class AddEditUserPermissions extends React.Component {

    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);
        this.state = ({
            permissionsUnModifiedData: "",
            isProcessing: false,
            permission: {},
            showWarningModel: false,
            showError: false,
            validationStateUniqueName: null,
            validateStatusName: null,
            validateUser: null,
            validateUniqueStatusName: null
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
            permission: this.props.data.permission,
            permissionsUnModifiedData: jQuery.extend(true, {}, this.props.data.permissions)

        });

    }

    /// <summary>
    /// Called to close the add edit pop up or open cancel warning pop up
    /// </summary>
    handleClose() {
        this.props.handleClose();
    }

    /// <summary>
    //called from cancel warning component to close add edit pop up
    /// </summary>
    handleAddEditClose() {
        this.props.handleClose();
    }

    /// <summary>
    /// Updating the user/description/name in the state on change of text
    /// </summary>
    handleTextChange(value, event) {


    }



    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveDisabled() {

    }

    /// <summary>
    /// This function is to set validations states value
    /// </summary>
    validateForm() {

    }

    /// <summary>
    /// To validate the status name is unique
    /// </summary>
    isStatusNameUnique(status) {

    }

    /// <summary>
    /// To Save the status details
    /// </summary>
    handleSave() {

    }

    componentDidMount() {
        // console.log(JSON.stringify(this.props.data));

    }

    render() {
       // console.log("this.state.permission :"+JSON.stringify(this.state.permission));  
        var msg = "";
        if (this.state.showError)
            msg = (<label data-ng-show="showError" class="alert alert-danger"><strong>Error!</strong> Status Name already exists. Please use a unique status name.</label>);

        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} onEntered={this.onEnteredModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{this.props.data.permission.id != null ? "Edit User" : "Add User"}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div class="panel panel-default">
                        <div class="panel-body">
                            {msg}
                            <AddEditUserBasicInformation data={this.props.data.permission} />
                            <Panel header="Personal information" >
                                <AddEditUserPersonalInformation info={this.props.data.permission} />
                            </Panel>
                            <Panel header="Permissions" >
                                <Tabs id="addeditpermission" defaultActiveKey={1} >
                                    <Tab eventKey={1} title="ODT portal">

                                    </Tab>
                                    <Tab eventKey={2} title="Delivery Queues">

                                    </Tab>
                                    <Tab eventKey={3} title="ODT API">

                                    </Tab>
                                    <Tab eventKey={4} title="Destinations">

                                    </Tab>
                                    <Tab eventKey={5} title="Brands">

                                    </Tab>
                                </Tabs>
                            </Panel>
                        </div>
                    </div>
                    <NotificationContainer />
                    <CancelWarningModal data={this.state} handleClose={this.closeWarningModel.bind(this)} handleAddEditClose={this.handleAddEditClose.bind(this)} />
                </Modal.Body>
                <Modal.Footer>
                    <Button disabled={this.state.isProcessing} onClick={this.handleClose.bind(this)}>Cancel</Button>
                    <Button disabled={this.isSaveDisabled()} onClick={this.handleSave.bind(this)} className="btn btn-primary btn-large">
                        {this.state.isProcessing ? "Processing" : "Save"}
                    </Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default AddEditUserPermissions