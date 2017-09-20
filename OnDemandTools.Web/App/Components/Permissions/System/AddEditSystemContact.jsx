import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import Select from 'react-select';
import { connect } from 'react-redux';
import validator from 'validator';
import Moment from 'moment';

/// <summary>
/// Sub component of product page to  add ,edit user basic details
/// </summary>
class AddEditSystemContact extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            technicalContactOptions: [],
            functionalContactOptions: [],
            validationStateTech: null,
            validationStateFunc: null
        });


    }

    componentWillMount() {

    }

    componentDidMount() {
        this.populateContactOptions(this.props.portalUsers);
        this.validateForm();
    }

    populateContactOptions(portalUsers) {

        var technicalContactId = this.props.data.api.technicalContactId;
        var functionalContactId = this.props.data.api.functionalContactId;

        var functionalContacts = portalUsers.filter(function (user) {
            return user.portal.isActive && user.id != technicalContactId
        });

        var technicalContacts = portalUsers.filter(function (user) {
            return user.portal.isActive && user.id != functionalContactId
        });

        var functionalOptions = functionalContacts.map(person => ({ value: person.id, label: person.firstName + " " + person.lastName }));
        this.setState({ functionalContactOptions: functionalOptions });


        var technicalOptions = technicalContacts.map(person => ({ value: person.id, label: person.firstName + " " + person.lastName }));
        this.setState({ technicalContactOptions: technicalOptions });

    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        //if (this.state.functionalContactOptions.length == 0) {
        //  this.populateContactOptions(nextProps.portalUsers);
        // }
    }

    /// <summary>
    /// To validate the form
    /// </summary>
    validateForm() {
        var technicalContactId = this.props.data.api.technicalContactId;
        var functionalContactId = this.props.data.api.functionalContactId;
        var hasContactError = (technicalContactId != "" && technicalContactId != null && functionalContactId != "" && functionalContactId != null);
        var hasTechError = (technicalContactId == "" || technicalContactId == null);
        var hasFuncError = (functionalContactId == "" || functionalContactId == null);

        this.setState({
            validationStateTech: hasTechError ? 'error' : null,
            validationStateFunc: hasFuncError ? 'error' : null
        });

        this.props.validationStates(hasContactError);
    }

    handleTechnicalContactChange(value) {
        var model = this.props.data;
        model.api.technicalContactId = value;
        this.props.updatePermission(model);
        this.populateContactOptions(this.props.portalUsers);
        this.validateForm();
    }

    handleFunctionalContactChange(value) {
        var model = this.props.data;
        model.api.functionalContactId = value;
        this.props.updatePermission(model);
        this.populateContactOptions(this.props.portalUsers);
        this.validateForm();
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
    /// To show/hide isactive property
    /// </summary>
    apiLastAccessedDisplay() {
        if (this.props.data.id != null) {
            return (<FormGroup>
                <ControlLabel>{"API Last Accessed"}</ControlLabel>
                <FormControl type="text" value={this.formatDate(this.props.data.api.lastAccessTime)} readOnly />
            </FormGroup>);
        }
    }

    render() {
        return (<div>
            <Grid >
                <Row>
                    <Form>
                        <Col sm={4}>
                            <FormGroup>
                                <ControlLabel>System API Key</ControlLabel>
                                <FormControl type="text" disabled={true} placeholder="API Key will be generated automatically"
                                    value={this.props.data.api.apiKey} />
                            </FormGroup>
                        </Col>
                        <Col sm={4}>
                            <FormGroup validationState={this.state.validationStateTech}>
                                <ControlLabel>Technical Contact</ControlLabel>
                                <FormGroup>
                                    <Select simpleValue options={this.state.technicalContactOptions}
                                        clearable={true}
                                        searchable={true}
                                        value={this.props.data.api.technicalContactId}
                                        onChange={this.handleTechnicalContactChange.bind(this)}
                                        placeholder={"Select Technical Contact"} />
                                </FormGroup>
                            </FormGroup>
                        </Col>
                    </Form >
                </Row>
                <Row>
                    <Form>
                        <Col sm={4}>
                            {this.apiLastAccessedDisplay()}
                        </Col>
                        <Col sm={4}>
                            <FormGroup validationState={this.state.validationStateFunc}>
                                <ControlLabel>Functional Contact</ControlLabel>
                                <FormGroup >
                                    <Select simpleValue options={this.state.functionalContactOptions}
                                        clearable={true}
                                        searchable={true}
                                        value={this.props.data.api.functionalContactId}
                                        onChange={this.handleFunctionalContactChange.bind(this)}
                                        placeholder={"Select Functional Contact"} />
                                </FormGroup>
                            </FormGroup>
                        </Col>
                    </Form >
                </Row>
            </Grid>

        </div>);
    }
}

export default AddEditSystemContact