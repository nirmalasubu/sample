import React from 'react';
import { Modal, ModalDialogue, ModalBody, ModalFooter, ModalHeader, ModalTitle, Grid, Row, Col, InputGroup, Form, ControlLabel, FormGroup, FormControl, Button, Panel } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';
import $ from 'jquery';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import * as PathTranslationHelper from 'Actions/PathTranslation/PathTranslationActions';
import PathTranslationModel from './PathTranslationModel'
import CancelWarningModal from 'Components/Common/CancelWarningModal';
import _ from 'lodash';


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
            modalTitle: "",
            showWarningModel: false,
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

        // Clone original prop to prepare data for this component. This is to avoid
        // indirect manipulation of original parent component data
        const pathTranModel = $.extend(true, {}, this.props.data.pathTranslationDetails);

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
                modalTitle: this.props.permissions.disableControl ? "Path Translation" : "Edit Path Translation"
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

        var urlValid = this.state.pathTranslationDetails.source.baseUrl
            .match(/^(http|ftp|https|HTTP|HTTPS):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?$/) ? true : false;

        return urlValid ? null : 'error';
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
        var urlValid = this.state.pathTranslationDetails.target.baseUrl
            .match(/^(http|ftp|https|HTTP|HTTPS):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?$/) ? true : false;

        return urlValid ? null : 'error';
    }


    /// <summary>
    /// Handle targetprocType changes
    /// </summary>
    handletargetprocTypeChange = (event) => {
        var model = this.state.pathTranslationDetails;
        model.target.protectionType = $.trim(event.target.value);
        this.setState({
            pathTranslationDetails: model
        });
    }

    /// <summary>
    /// Return validate state for targetprocType
    /// </summary>
    gettargetprocTypeValidationState = () => {
        return $.trim(this.state.pathTranslationDetails.target.protectionType) ? null : 'error';
    }

    /// <summary>
    /// Handle targetURLType changes
    /// </summary>
    handletargetURLTypeChange = (event) => {
        var model = this.state.pathTranslationDetails;
        model.target.urlType = $.trim(event.target.value);
        this.setState({
            pathTranslationDetails: model
        });
    }

    /// <summary>
    /// Return validate state for targetURLType
    /// </summary>
    gettargetURLTypeValidationState = () => {
        return $.trim(this.state.pathTranslationDetails.target.urlType) ? null : 'error';
    }

    /// <summary>
    /// Determine whether cancel button needs to be enabled or not
    /// </summary>
    isCloseEnabled = () => {
        return (this.state.isProcessing);
    }

    /// <summary>
    /// Determine whether save button needs to be enabled or not
    /// </summary>
    isSaveEnabled = () => {

        if (this.state.isProcessing) {
            return true;
        }

        return !(this.getsourceBaseURLValidationState() == null && this.gettargetBaseURLValidationState() == null &&
            this.gettargetprocTypeValidationState() == null && this.gettargetURLTypeValidationState() == null);
    }

    /// <summary>
    /// Handle close event for this component
    /// </summary>
    handleAddEditClose = () => {
        this.props.handleClose();
    }

    /// <summary>
    /// Open warning window
    /// </summary>
    openWarningModel = () => {
        this.setState({ showWarningModel: true });
    }

    /// <summary>
    /// Close warning window
    /// </summary>
    closeWarningModel = () => {
        this.setState({ showWarningModel: false });
    }

    /// <summary>
    /// Called to close the add edit pop up or open cancel warning pop up
    /// </summary>
    handleClose = () => {
        if (_.isEqual(this.state.pathTranslationDetails, this.props.data.pathTranslationDetails)) {
            this.props.handleClose();
        }
        else {
            this.openWarningModel();
        }
    }

    /// <summary>
    /// Save path translation
    /// </summary>
    handleSave = () => {
        this.setState({ isProcessing: true });

        this.props.dispatch(PathTranslationHelper.savePathTranslation(this.state.pathTranslationDetails))
            .then(() => {
                // Saved successfully. Now show appropriate message               
                if (this.state.pathTranslationDetails.id == "") {
                    NotificationManager.success('Path translation created successfully.', '', 500);
                }
                else {
                    NotificationManager.success('Path translation updated successfully.', '', 500);
                }

                // Reset save button state and close window              
                setTimeout(() => {
                    this.props.handleClose();
                }, 1000);

            }).catch(error => {
                // Show appropriate message in case of failure
                if (this.state.pathTranslationDetails.id == "") {
                    NotificationManager.error('Failed to create path translation. ' + error.message, 'Failure', 800);
                }
                else {
                    NotificationManager.error('Failed to update path translation. ' + error.message, 'Failure', 800);
                }

                // Reset save button
                this.setState({ isProcessing: false });
            });

    }



    render() {

        var saveButton = null;
        if (this.props.permissions.canAddOrEdit) {
            saveButton = (<Button disabled={this.isSaveEnabled()} onClick={this.handleSave} bsStyle="primary">
                {this.state.isProcessing ? "Processing" : "Save"}
            </Button>);
        }



        return (
            <Modal show={this.props.data.showAddEditModal} onEntering={this.onOpenModal} onHide={this.props.handleClose} onExiting={this.purgeModalHistory}
                onHide={this.handleClose}>
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
                                disabled={this.props.permissions.disableControl}
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
                                disabled={this.props.permissions.disableControl}
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
                                disabled={this.props.permissions.disableControl}
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
                                disabled={this.props.permissions.disableControl}
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
                                disabled={this.props.permissions.disableControl}
                                placeholder="Enter URL type"
                                value={this.state.pathTranslationDetails.target.urlType}
                                onChange={this.handletargetURLTypeChange}
                            />

                        </FormGroup>
                    </Panel>
                    <NotificationContainer />
                    <CancelWarningModal data={this.state} handleClose={this.closeWarningModel} handleAddEditClose={this.handleAddEditClose} />
                </Modal.Body>
                <Modal.Footer>
                    <Button disabled={this.isCloseEnabled()} onClick={this.handleClose}>Close</Button>
                    {saveButton}
                </Modal.Footer>
            </Modal>
        )
    }

}


export default AddEditPathTranslationModal;