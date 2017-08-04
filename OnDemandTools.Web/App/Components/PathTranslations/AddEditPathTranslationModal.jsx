import React from 'react';
import { Modal, ModalDialogue, ModalBody, ModalFooter, ModalHeader, ModalTitle, Grid, Row, Col, InputGroup, Form, ControlLabel, FormGroup, FormControl, Button, Panel } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';
import $ from 'jquery';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import * as PathTranslationHelper from 'Actions/PathTranslation/PathTranslationActions';
import PathTranslationModel from './PathTranslationModel'

/// <summary>
/// Connect this component to global Redux data store
/// </summary>
@connect((store) => {
    return {
        applicationError: store.applicationError
    };
})



/// <summary>
/// Sub component for adding/edit path translation
/// </summary>
class AddEditPathTranslationModal extends React.Component {

    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = {
            pathTranslationDetails: PathTranslationModel,
            isProcessing: false,
            modalTitle: ""
        }

    }

    /// <summary>
    /// Before the component is rendered, retrieve new path translation 
    /// model stub
    /// action/reducer.
    /// </summary>  
    componentWillMount() {

    }

    /// <summary>
    /// Weird, but got to do this!
    /// </summary>
    handleNullCase = (element) => {
        if (element == null) {
            return "";
        }
        else {
            return element;
        }
    }

    /// <summary>
    /// Purge this component's state
    /// </summary>
    purgeModalHistory = () => {
        console.log('called');
        this.setState({
            pathTranslationDetails: PathTranslationModel,
            isProcessing: false,
            modalTitle: ""
        });
    }




    /// <summary>
    /// Initialize modal data upon load
    /// </summary>
    onOpenModal = () => {

        // First do some sanity check before assigning the state with new data
        const pathTranModel = this.props.data.pathTranslationDetails;
        pathTranModel.id = pathTranModel.id ? pathTranModel.id : "";
        pathTranModel.source.baseUrl = pathTranModel.source.baseUrl ? pathTranModel.source.baseUrl : "";
        pathTranModel.source.brand = pathTranModel.source.brand ? pathTranModel.source.brand : "";
        pathTranModel.target.baseUrl = pathTranModel.target.baseUrl ? pathTranModel.target.baseUrl : "";
        pathTranModel.target.protectionType = pathTranModel.target.protectionType ? pathTranModel.target.protectionType : "";
        pathTranModel.target.urlType = pathTranModel.target.urlType ? pathTranModel.target.urlType : "";

        this.setState({
            pathTranslationDetails: pathTranModel
        });


        // Set modal title based on path translation object id. The assumption is that
        // if id is "" then it's adding new path translation, otherwise it's editing
        if (pathTranModel.id == "") {
            this.setState({
                modalTitle: "Add Path Translation"
            })
        }
        else {
            this.setState({
                modalTitle: "Edit Path Translation"
            })
        }

    }

    /// <summary>
    /// Handle sourceBaseURL changes
    /// </summary>
    handlesourceBaseURLChange = (event) => {
        var model = this.state.pathTranslationDetails;
        model.source.baseUrl = event.target.value;
        this.setState({
            pathTranslationDetails: model
        });
    }

    /// <summary>
    /// Return validate state for sourceBaseURL
    /// </summary>
    getsourceBaseURLValidationState = () => {
        return this.state.pathTranslationDetails.source.baseUrl ? null : 'error';
    }


    /// <summary>
    /// Handle sourceBrand changes
    /// </summary>
    handlesourceBrandChange = (event) => {
        var model = this.state.pathTranslationDetails;
        model.source.brand = event.target.value;
        this.setState({
            pathTranslationDetails: model
        });
    }


    /// <summary>
    /// Handle targetBaseURL changes
    /// </summary>
    handletargetBaseURLChange = (event) => {
        var model = this.state.pathTranslationDetails;
        model.target.baseUrl = event.target.value;
        this.setState({
            pathTranslationDetails: model
        });
    }

    /// <summary>
    /// Return validate state for targetBaseURL
    /// </summary>
    gettargetBaseURLValidationState = () => {
        return this.state.pathTranslationDetails.target.baseUrl ? null : 'error';
    }


    /// <summary>
    /// Handle targetprocType changes
    /// </summary>
    handletargetprocTypeChange = (event) => {
        var model = this.state.pathTranslationDetails;
        model.target.protectionType = event.target.value;
        this.setState({
            pathTranslationDetails: model
        });
    }

    /// <summary>
    /// Return validate state for targetprocType
    /// </summary>
    gettargetprocTypeValidationState = () => {
        return this.state.pathTranslationDetails.target.protectionType ? null : 'error';
    }

    /// <summary>
    /// Handle targetURLType changes
    /// </summary>
    handletargetURLTypeChange = (event) => {
        var model = this.state.pathTranslationDetails;
        model.target.urlType = event.target.value;
        this.setState({
            pathTranslationDetails: model
        });
    }

    /// <summary>
    /// Return validate state for targetURLType
    /// </summary>
    gettargetURLTypeValidationState = () => {
        return this.state.pathTranslationDetails.target.urlType ? null : 'error';
    }

    render() {



        return (
            <Modal show={this.props.data.showAddEditModal} onEntering={this.onOpenModal} onHide={this.props.handleClose} onExiting={this.purgeModalHistory}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        {this.state.modalTitle}
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Panel header="Path Translation Details">

                        <FormGroup validationState={this.getsourceBaseURLValidationState()}>
                            <div class="softHead">Source Path</div> 
                            <br />                          
                            <ControlLabel bsClass="standout" htmlFor="sourceBaseURL">Source Base URL</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Source base URL"
                                value={this.state.pathTranslationDetails.source.baseUrl}
                                id="sourceBaseURL"
                                onChange={this.handlesourceBaseURLChange}
                            />
                        </FormGroup>
                        <FormGroup>                           
                            <ControlLabel bsClass="standout" htmlFor="sourceBrand">Brand</ControlLabel>
                            <FormControl
                                type="text"
                                id="sourceBrand"
                                placeholder="Enter brand, if applicable"
                                value={this.state.pathTranslationDetails.source.brand}
                                onChange={this.handlesourceBrandChange}
                            />
                        </FormGroup>
                        <hr />
                        <FormGroup validationState={this.gettargetBaseURLValidationState()}>
                            <div class="softHead">Target Path</div>
                            <br />
                            <ControlLabel bsClass="standout" htmlFor="targetBaseURL">Target Base URL</ControlLabel>
                            <FormControl
                                type="text"
                                id="targetBaseURL"
                                placeholder="Target base URL"
                                value={this.state.pathTranslationDetails.target.baseUrl}
                                onChange={this.handletargetBaseURLChange}
                            />
                        </FormGroup>
                        <FormGroup validationState={this.gettargetprocTypeValidationState()}>                           
                            <ControlLabel bsClass="standout" htmlFor="targetprocType">Protection Type</ControlLabel>
                            <FormControl
                                type="text"
                                id="targetprocType"
                                placeholder="Enter protection type"
                                value={this.state.pathTranslationDetails.target.protectionType}
                                onChange={this.handletargetprocTypeChange}
                            />
                          </FormGroup>
                          <FormGroup validationState={this.gettargetURLTypeValidationState()}>                          
                            <ControlLabel bsClass="standout" htmlFor="targetURLType">URL Type</ControlLabel>
                            <FormControl
                                type="text"
                                id="targetURLType"
                                placeholder="Enter URL type"
                                value={this.state.pathTranslationDetails.target.urlType}
                                onChange={this.handletargetURLTypeChange}
                            />

                        </FormGroup>
                    </Panel>
                    <NotificationContainer />
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Cancel</Button>
                    <Button bsStyle="primary">{this.state.isProcessing ? "Processing" : "Continue"}</Button>
                </Modal.Footer>
            </Modal>
        )
    }

}


export default AddEditPathTranslationModal;