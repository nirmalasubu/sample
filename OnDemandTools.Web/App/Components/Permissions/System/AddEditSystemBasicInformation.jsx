import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';
import Moment from 'moment';
import * as destinationActions from 'Actions/Destination/DestinationActions';


@connect((store) => {
    return {
        permissions: store.permissions,
        destinations: store.destinations
    };
})
/// <summary>
/// Sub component of product page to  add ,edit user basic details
/// </summary>
class AddEditSystemBasicInformation extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            userBasicInfoModel: "",
            userBasicInfounmodifiedModel: "",
            componentJustMounted: true,
            validationStateEmail: null,
            validateUniqueUserName: null,
            showError: false
        });
    }

    componentWillMount() {
        // Dispatch another action to asynchronously fetch full list of destination data
        // from server. Once it is fetched, the data will be stored
        // in redux store
        this.props.dispatch(destinationActions.fetchDestinations());

        this.setState({
            userBasicInfoModel: this.props.data
        });
    }

    componentDidMount() {
        var model = this.state.userBasicInfoModel;
        if (this.state.userBasicInfoModel.id == null) {
            this.setState({ userBasicInfoModel: model, componentJustMounted: true });
        }
        this.setState({
            userBasicInfounmodifiedModel: jQuery.extend(true, {}, this.props.data)
        });
        this.validateForm();
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        this.setState({
            userBasicInfoModel: nextProps.data
        }, function () {
            if (this.state.componentJustMounted) {
                this.setState({ componentJustMounted: false }, function () {
                    this.validateForm();
                });
            }
        });

    }

    formatDate(val) {
        if (val == null) {
            return "never"
        }
        else {

            var d = new Date(val);
            var year = d.getFullYear();
            if (year < 2000) {
                return "never";
            }
            else {

                var dateFormat = Moment(val).format('lll');
                return Moment(val).format('lll');
            }
        }
    }

    /// <summary>
    /// To show/hide lastlogin property
    /// </summary>
    lastloginDisplay() {
        if (this.state.userBasicInfoModel.id != null) {
            return (<FormGroup controlId="lastlogin " >
                <ControlLabel>Last login</ControlLabel>
                <FormControl type="text" defaultValue={this.formatDate(this.state.userBasicInfoModel.portal.lastLoginTime)} disabled="true" />
            </FormGroup>);
        }
    }

    /// <summary>
    /// To show/hide isactive property
    /// </summary>
    activeDateDisplay() {


        if (this.state.userBasicInfoModel.id != null) {
            return (<FormGroup controlId="activeDate" >
                <ControlLabel>{this.state.userBasicInfoModel.portal.isActive ? "Active Date" : "Last Active Date"}</ControlLabel>
                <FormControl type="text" value={this.formatDate(this.state.userBasicInfoModel.activeDateTime)} readOnly />
            </FormGroup>);
        }
    }

    /// <summary>
    /// To update isactive property
    /// </summary>
    activeStatusChange() {
        var model = this.state.userBasicInfoModel;
        model.api.isActive = !this.state.userBasicInfoModel.api.isActive;

        if (model.api.isActive != this.state.userBasicInfounmodifiedModel.api.isActive) {
            var date = new Date();
            model.activeDateTime = model.id != null ? date : this.state.userBasicInfounmodifiedModel.activeDateTime;
        } else {
            model.activeDateTime = this.state.userBasicInfounmodifiedModel.activeDateTime;

        }

        this.setState({
            userBasicInfoModel: model
        });
        this.props.updatePermission(model);
    }


    /// <summary>
    /// To user name on the text box change
    /// </summary>
    handleTextChange(event) {

        var model = this.state.userBasicInfoModel;
        model.userName = event.target.value.trim();

        this.setState({
            userBasicInfoModel: model
        });
        this.validateForm();
        this.props.updatePermission(model);
    }

    /// <summary>
    /// To user name on the text box change
    /// </summary>
    handleNotesChange(event) {
        var model = this.state.userBasicInfoModel;
        model.notes = event.target.value.trimLeft();

        this.setState({
            userBasicInfoModel: model
        });
        this.validateForm();
        this.props.updatePermission(model);
    }


    /// <summary>
    /// To validate the form
    /// </summary>
    validateForm() {

        var isvalidUserId = true;
        var hasNameError = isvalidUserId && !(this.isUserNameUnique(this.state.userBasicInfoModel));
        this.setState({
            validationStateEmail: hasNameError ? null : 'error'
        });

        if (!isvalidUserId) {
            this.setState({
                showError: false
            });
        }

        this.props.validationStates(hasNameError);
    }


    /// <summary>
    /// To validate the user name is unique
    /// </summary>
    isUserNameUnique(user) {

        for (var x = 0; x < this.props.permissions.length; x++) {
            if (this.props.permissions[x].id != user.id) {
                if (this.props.permissions[x].userName.toLowerCase().trim() == user.userName.toLowerCase().trim()) {
                    this.setState({
                        showError: true
                    });

                    return true;
                }
                else {
                    this.setState({
                        showError: false
                    });
                }
            }
        }

        return false;
    }

    render() {
        var msg = ""
        if (this.state.showError)
            msg = (<label data-ng-show="showError" class="alert alert-danger"><i class="fa fa-exclamation-circle"></i> User ID already exists. Please use a unique User ID.</label>);
        return (
            <div>
                {msg}
                <Grid >
                    <Row>
                        <Form>
                            <Col sm={4}>
                                <FormGroup controlId="userId" validationState={this.state.validationStateEmail}>
                                    <ControlLabel>System ID</ControlLabel>
                                    <FormControl type="text" ref="inputUserName" placeholder="Enter System Id"
                                        onChange={(event) => this.handleTextChange(event)} value={this.state.userBasicInfoModel.userName} />
                                </FormGroup>
                            </Col>
                            <Col sm={4}>
                                <ControlLabel>Active Status</ControlLabel>
                                <div>
                                    <label class="switch">
                                        <input type="checkbox" checked={this.state.userBasicInfoModel.api.isActive} onChange={(event) => this.activeStatusChange()} />
                                        <span class="slider round"></span>
                                    </label>
                                </div>
                            </Col>
                        </Form >
                    </Row>
                    <Row>
                        <Form>
                            <Col sm={4}>
                                <FormGroup controlId="notes" validationState={this.state.validationStateNotes}>
                                    <ControlLabel>Notes</ControlLabel>
                                    <FormControl type="text" ref="inputNotes" placeholder="Enter System Notes"
                                        onChange={(event) => this.handleNotesChange(event)} value={this.state.userBasicInfoModel.notes} />
                                </FormGroup>
                            </Col>
                            <Col sm={4}>
                                {this.activeDateDisplay()}
                            </Col>
                        </Form >
                    </Row>                    
                </Grid>
            </div>
        )
    }
}

export default AddEditSystemBasicInformation