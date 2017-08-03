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
import * as idActions from 'Actions/AiringIdDistribution/AiringIdDistributionActions';
import CancelWarningModal from 'Components/Common/CancelWarningModal';

@connect((store) => {
    return {

    };
})

// Sub component of airing id distribution page to  add ,edit and delete current airing id details
class AddEditDistribution extends React.Component {

    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);
        this.state = ({
            airingIdUnModifiedData: {},
            isProcessing: false,
            currentAiringId: {},
            showWarningModel: false,
            showError: false,
            validationStateCode: null,
            validationStateSequence: null,
            validateStatusCurrent:null,
            validateStatusUpper:null,
            validateStatusLower:null,
            validateUniqueCode:null,
            postUrl:""
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
            currentAiringId: this.props.data.airingIdDetails,
            airingIdUnModifiedData: jQuery.extend(true, {}, this.props.data.airingIdDetails)
        });
    }

    /// <summary>
    /// Called to close the add edit pop up or open cancel warning pop up
    /// </summary>
    handleClose() {
            if (JSON.stringify(this.state.airingIdUnModifiedData) == JSON.stringify(this.state.currentAiringId)) {
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
        var model = this.state.airingIdUnModifiedData;
        jQuery.extend(this.state.currentAiringId, this.state.airingIdUnModifiedData);

        this.props.handleClose();
    }
    
    /// <summary>
    /// Updating the code/sequence/billing numbers in the state on change of text
    /// </summary>
    handleTextChange(value,event) {
        var url = this.state.postUrl;
        var model = this.state.currentAiringId;
        if(value=="code")
            model.prefix = event.target.value;
        if(value=="sequence")
            model.sequenceNumber = event.target.value;
        if(value=="current")
            model.billingNumberCurrent = event.target.value;
        if(value=="lower")
            model.billingNumberLower = event.target.value;
        if(value=="uppper")
            model.billingNumberUpper = event.target.value;
        if(value=="url")
            url = event.target.value.toUpperCase();

        this.setState({
            currentAiringId: model,
            postUrl:url
        });

        this.validateForm();
    }

    //<summary>
    /// capitalize the code or prefix text
    ///</summary>
    ConvertToUpperCase(event){
        var val=event.target.value;
        event.target.value=val.toUpperCase();
    }
    
    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveEnabled() {
        return (this.state.validationStateCode || this.state.validationStateSequence || this.state.validateStatusCurrent || this.state.validateStatusUpper ||
            this.state.validateStatusLower || this.state.validateUniqueCode);
    }

    /// <summary>
    /// This function is to set validations states value
    /// </summary>
    validateForm() {
        var isNameValid = this.state.currentAiringId.prefix.match("^[a-zA-Z]+$")==null;  //validation to accept only aplhabets
        var isSequenceEmpty = (this.state.currentAiringId.sequenceNumber == "");
        var isCurrentEmpty = (this.state.currentAiringId.billingNumberCurrent == "");
        var isUpperEmpty = (this.state.currentAiringId.billingNumberUpper == "");
        var isLowerEmpty = (this.state.currentAiringId.billingNumberLower == "");

        this.setState({
            validationStateCode: isNameValid  ? 'error' : null,
            validationStateSequence: isSequenceEmpty ? 'error' : null,
            validateStatusCurrent: isCurrentEmpty ? 'error' : null,
            validateStatusUpper: isUpperEmpty ? 'error' : null,
            validateStatusLower: isLowerEmpty ? 'error' : null,
            validateUniqueCode: this.isCodeUnique(this.state.currentAiringId) ? 'error' : null
        });
    }

    /// <summary>
    /// To validate the status name is unique
    /// </summary>
    isCodeUnique(airingId) {
        for (var x = 0; x < this.props.airingIdandPrefix.length; x++) {
            if (this.props.airingIdandPrefix[x].id != airingId.id) {
                if (this.props.airingIdandPrefix[x].name == airingId.name) {
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
    /// To Save the airing Id details
    /// </summary>
    handleSave() {
        var elem = this;

        this.setState({ isProcessing: true });

        this.props.dispatch(idActions.saveStatus(this.state.currentAiringId))
            .then(() => {
                if (this.state.status.id == null) {
                    NotificationManager.success(this.state.status.name + ' Current Airing Id successfully created.', '', 2000);
                }
                else {
                    NotificationManager.success(this.state.status.name + ' Current Airing Id updated successfully.', '', 2000);
                }
                this.props.dispatch(idActions.fetchCurrentAiringId());

                setTimeout(function () {
                    elem.props.handleClose();
                }, 3000);
            }).catch(error => {
                if (this.state.status.id == null) {
                    NotificationManager.error(this.state.status.name + ' Current Airing Id creation failed. ' + error, 'Failure');
                }
                else {
                    NotificationManager.error(this.state.status.name + ' Current Airing Id update failed. ' + error, 'Failure');
                }
                this.setState({ isProcessing: false });
            });        
    }
    
    componentDidMount() {
        var initialValue={ "id": null, "prefix": "", "sequenceNumber": 0, "billingNumberCurrent": 0, "billingNumberUpper": 0, "billingNumberLower": 0};

        //required to overcome form control warning of categoryName
        this.setState({
            currentAiringId: initialValue            
        });
      
    }

render() {
    var msg = "";
    if (this.state.showError)
        msg = (<label data-ng-show="showError" class="alert alert-danger"><strong>Error!</strong> This Code is already exists. Please use a unique code.</label>);

    return (
        <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this)} onEntered={this.onEnteredModel.bind(this)} show={this.props.data.showAddEditModel} onHide={this.handleClose.bind(this)}>
            <Modal.Header closeButton>
                <Modal.Title>
                    <div>{this.props.data.airingIdDetails.id != null ? "Edit ID -" + this.state.airingIdUnModifiedData.prefix : "Add ID"}</div>
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
                            <FormGroup controlId="prefixControl" validationState={this.state.validationStateCode||this.state.validateUniqueCode}>
                                <ControlLabel>Code</ControlLabel>
                                <FormControl type="text"  value={this.state.currentAiringId.prefix} maxLength="20" ref="inputCode" placeholder="Enter 4 letter upper case code" 
                                onChange={(event) =>this.handleTextChange("code", event)} onKeyUp={(event) =>this.ConvertToUpperCase(event)}/>
                            </FormGroup>
                        </Col>
                        <Col sm={4}>
                            <FormGroup controlId="sequence" validationState={this.state.validationStateSequence}>
                                <ControlLabel>Current Number</ControlLabel>
                                <FormControl type="number"  value={this.state.currentAiringId.sequenceNumber} ref="inputSequenceNumber" placeholder="1 - 99,999" maxLength="5" 
                                 onChange={(event) =>this.handleTextChange("sequence", event)}/>
                            </FormGroup>
                         </Col>
                     </Form >
                    </Row>
                    <Row>
                      <Form> 
                        <Col sm={3}>
                            <FormGroup controlId="upper" validationState={this.state.validateStatusUpper}>
                                <ControlLabel>Upper Billing Number</ControlLabel>
                                <FormControl type="number"  value={this.state.currentAiringId.billingNumberUpper} maxLength="5" ref="inputUpper" placeholder="0" 
                                onChange={(event) =>this.handleTextChange("upper", event)} />
                            </FormGroup>
                        </Col>
                        <Col sm={2}>
                            <FormGroup controlId="current" validationState={this.state.validateStatusCurrent}>
                                <ControlLabel>Current Billing Number</ControlLabel>
                                <FormControl type="number"  value={this.state.currentAiringId.billingNumberCurrent} maxLength="5" ref="inputCurrent" placeholder="0" 
                                onChange={(event) =>this.handleTextChange("current", event)} />
                            </FormGroup>
                        </Col>
                        <Col sm={3}>
                            <FormGroup controlId="lower" validationState={this.state.validateStatusLower}>
                                <ControlLabel>Lower Billing Number</ControlLabel>
                                <FormControl type="number"  value={this.state.currentAiringId.billingNumberLower} maxLength="5" ref="inputLower" placeholder="0" 
                                onChange={(event) =>this.handleTextChange("lower", event)} />
                            </FormGroup>
                        </Col>
                     </Form >
                    </Row>
                    <FormGroup controlId="urlpost">
                    <ControlLabel>URL</ControlLabel>
                    <FormControl type="text" disabled class="workflowStatus-description" value={this.state.currentAiringId.postUrl} ref="inputUrl"   placeholder="URL will be shown in edit page" 
                     onChange={(event) =>this.handleTextChange("url", event)}/>
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

export default AddEditDistribution