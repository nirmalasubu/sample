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
import * as permissionActions from 'Actions/Permissions/PermissionActions';
import validator from 'validator';
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
            validationStateUserName: null,
            validationStateFirstName:null,
            validationStateLastName:null,
            validationStatePhoneNumber: null,
            validationStateExtension: null
        
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
    /// This function is called on entering the modal pop up
    /// </summary>
    onOpenModel() {        
        this.setState({
            isProcessing: false,
            permission: this.props.data.permission,
            permissionsUnModifiedData: jQuery.extend(true, {}, this.props.data.permission)
        });

    }


    /// <summary>
    /// Called to close the add edit pop up or open cancel warning pop up
    /// </summary>
    handleClose() {
        if (JSON.stringify(this.state.permissionsUnModifiedData) == JSON.stringify(this.state.permission)) {
            this.props.handleClose();
        }
        else {
            this.openWarningModel();
        }
        
    }

    /// <summary>
    //called from cancel warning component to close add edit pop up
    /// </summary>
    handleAddEditClose() {
        this.props.handleClose();
    }


    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveDisabled() {
        return (this.state.isProcessing || 
            this.state.validationStateUserName != null||
            this.state.validationStateFirstName != null||this.state.validationStateLastName != null||
             this.state.validationStatePhoneNumber != null||this.state.validationStateExtension != null)
    }

    /// <summary>
    /// property method to called in the sub component to update the model
    /// </summary>
    updatePermission(permission)
    {
        this.setState({ permission: permission });
    }

    /// <summary>
    /// property method to called in the sub component to validate user basic info
    /// </summary>
    updateBasicValidateStates(isValidUserName)
    {
        this.setState({
            validationStateUserName: isValidUserName ? null : 'error'
            
        });

    }

    /// <summary>
    /// property method to called in the sub component to validate user personal info
    /// </summary>
    updatePersonalInfoValidateStates(isValidFirstName,isValidLastName,isvalidPhoneNumber,isvalidextension)
    {
        this.setState({
            validationStateFirstName: isValidFirstName ? null : 'error',
            validationStateLastName: isValidLastName ? null : 'error',
            validationStatePhoneNumber: isvalidPhoneNumber ? null : 'error',
            validationStateExtension: isvalidextension ? null : 'error',
        });
    }

    /// <summary>
    /// To Save the user details
    /// </summary>
    handleSave() {
      
        var elem = this;
        if (!this.isSaveDisabled()) {

            this.setState({ isProcessing: true });

            this.props.dispatch(permissionActions.savePermission(this.state.permission))
                .then(() => {
                    if (this.state.permission.id == null) {
                        NotificationManager.success(this.state.permission.userName + ' userId successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(this.state.permission.userName + ' userId updated successfully.', '', 2000);
                    }
                    
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    if (this.state.permission.id == null) {
                        NotificationManager.error(this.state.permission.userName + ' userId creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(this.state.permission.userName + ' userId update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
        
    }

    componentDidMount() {

    }

    render() {

        return (
            <Modal bsSize="large" backdrop="static"   onEntering={this.onOpenModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>{this.props.data.permission.id != null ? "Edit User" : "Add User"}</div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <AddEditUserBasicInformation data={this.props.data.permission} 
                            updatePermission={this.updatePermission.bind(this)}  validationStates={this.updateBasicValidateStates.bind(this)}/>
                            <Panel header="Personal information" >
                                <AddEditUserPersonalInformation data={this.props.data.permission}
                            updatePermission={this.updatePermission.bind(this)} validationStates={this.updatePersonalInfoValidateStates.bind(this)}/>
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