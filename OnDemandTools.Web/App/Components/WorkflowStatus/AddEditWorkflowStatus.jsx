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

@connect((store) => {
    return {

    };
})

// Sub component of category page to  add ,edit and delete category details
class AddEditWorkflowStatus extends React.Component {

    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);
        this.state = ({
            statusUnModifiedData: "",
            isProcessing: false,
            status: {},
            showWarningModel: false,
            showError: false,
            validationStateUniqueName: null,
            validateStatusName:null,
            validateUser:null,
            validateUniqueStatusName:null
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
                status: this.props.data.status,
                statusUnModifiedData: jQuery.extend(true, {}, this.props.data.status)
            });

    }

    /// <summary>
    /// Called to close the add edit pop up or open cancel warning pop up
    /// </summary>
    handleClose() {
            if (JSON.stringify(this.state.statusUnModifiedData) == JSON.stringify(this.state.status)) {
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
        var model = this.state.destinationUnModifiedData;
        jQuery.extend(this.state.status, this.state.statusUnModifiedData);

      //  this.setState({ validationStateDestinationName: null });
        this.props.handleClose();
    }
    
    /// <summary>
    /// Updating the user/description/name in the state on change of text
    /// </summary>
    handleTextChange(value,event) {

        var model = this.state.status;
        if(value=="user")
            model.user = event.target.value;
        if(value=="description")
            model.description = event.target.value;
        if(value=="name")
            model.name = event.target.value.toUpperCase();

        this.setState({
            status: model
        });

        this.validateForm();
    }

    //<summary>
    /// capitalize the status name text
    ///</summary>
    ConvertToUpperCase(event){
        var val=event.target.value;
        event.target.value=val.toUpperCase();
    }
    
    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveEnabled() {
        return ( this.state.validateStatusName != null||this.state.validateUser != null||this.state.validateUniqueStatusName != null);
    }

    /// <summary>
    /// This function is to set validations states value
    /// </summary>
    validateForm() {
        var isNameValid = this.state.status.name.match("^[a-zA-Z]+$")==null;  //validation to accept only aplhabets
        var isUserEmpty = (this.state.status.user == "");

        this.setState({
            validateStatusName: isNameValid  ? 'error' : null,
            validateUser: isUserEmpty ? 'error' : null,
            validateUniqueStatusName: this.isStatusNameUnique(this.state.status) ? 'error' : null
        });
    }

    /// <summary>
    /// To validate the status name is unique
    /// </summary>
    isStatusNameUnique(status) {
        for (var x = 0; x < this.props.statusIdandNames.length; x++) {
            if (this.props.statusIdandNames[x].id != status.id) {
                if (this.props.statusIdandNames[x].name == status.name) {
                    this.setState({showError: true });
                    return true;
                }
                else {
                    this.setState({showError: false});
                }
            }
        }
        return false;
    }

    /// <summary>
    /// To Save the status details
    /// </summary>
    handleSave() {
        var elem = this;
        if (this.state.validateStatusName != "error" && this.state.validateUser != "error"&& this.state.validateUniqueStatusName != "error") {

            this.setState({ isProcessing: true });

            this.props.dispatch(statusActions.saveStatus(this.state.status))
                .then(() => {
                    if (this.state.status.id == null) {
                        NotificationManager.success(this.state.status.name + ' status successfully created.', '', 2000);
                    }
                    else {
                        NotificationManager.success(this.state.status.name + ' status updated successfully.', '', 2000);
                    }
                    this.props.dispatch(statusActions.fetchStatus());
                    setTimeout(function () {
                        elem.props.handleClose();
                    }, 3000);
                }).catch(error => {
                    if (this.state.status.id == null) {
                        NotificationManager.error(this.state.status.name + ' status creation failed. ' + error, 'Failure');
                    }
                    else {
                        NotificationManager.error(this.state.status.name + ' status update failed. ' + error, 'Failure');
                    }
                    this.setState({ isProcessing: false });
                });
        }
        else
            return false;
    }
    
    componentDidMount() {
        var initialStatus={ "id": null, "name": "", "description":"","user":""};

        //required to overcome form control warning of categoryName
        this.setState({
            status: initialStatus
            
        });
      
    }

render() {
    var msg = "";
    if (this.state.showError)
        msg = (<label data-ng-show="showError" class="alert alert-danger"><strong>Error!</strong> Status Name already exists. Please use a unique status name.</label>);

    return (
        <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} onEntered={this.onEnteredModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
            <Modal.Header closeButton>
                <Modal.Title>
                    <div>{this.props.data.status.id != null ? "Edit Status -" + this.state.statusUnModifiedData.name : "Add Status"}</div>
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div class="panel panel-default">
                   <div class="panel-body">
                    {msg}
                     <Grid >
                     <Row>
                      <Form> 
                    <Col sm={4}>
                    <FormGroup controlId="statusName" validationState={this.state.validateStatusName||this.state.validateUniqueStatusName}>
                    <ControlLabel>Status Name</ControlLabel>
                    <FormControl type="text"  value={this.state.status.name} maxLength="20" ref="inputStatusName" placeholder="Status Name" 
                    onChange={(event) =>this.handleTextChange("name", event)} onKeyUp={(event) =>this.ConvertToUpperCase(event)}/>
                    </FormGroup>
                   </Col>
                   <Col sm={4}>
                   <FormGroup controlId="user" validationState={this.state.validateUser}>
                    <ControlLabel>User Group</ControlLabel>
                    <FormControl type="text"  value={this.state.status.user} ref="inputStatusUser" placeholder="User Group" maxLength="20" 
                     onChange={(event) =>this.handleTextChange("user", event)}/>
                    </FormGroup>
                     </Col>
                     </Form >
                    </Row>
                    <FormGroup controlId="description">
                    <ControlLabel>Description</ControlLabel>
                    <FormControl componentClass="textarea" class="workflowStatus-description"  rows="3" cols="40" maxLength="150" value={this.state.status.description} ref="inputStatusDescription"   placeholder="Description" 
                     onChange={(event) =>this.handleTextChange("description", event)}/>
                   </FormGroup>
                   </Grid>
                  </div>
                  </div>
                    <NotificationContainer />
                    <CancelWarningModal data={this.state} handleClose={this.closeWarningModel.bind(this)} handleAddEditClose={this.handleAddEditClose.bind(this)} />
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

export default AddEditWorkflowStatus