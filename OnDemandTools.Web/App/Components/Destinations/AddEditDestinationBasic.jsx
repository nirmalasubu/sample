import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';

@connect((store) => {
    return {
        destinations: store.destinations
    };
})

class AddEditDestinationBasic extends React.Component {

    constructor(props) {
        super(props);

        this.state = ({
            destinationModel: {},
            validationStateName: null,
            validationStateDescription: null,
            showError: false
        });
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        this.setState({
            destinationModel: nextProps.data
        });
    }

    isCodeUnique(currentDestination) {
        for (var x = 0; x < this.props.destinations.length; x++) {
            if (this.props.destinations[x].id != currentDestination.id) {
                if (this.props.destinations[x].name == currentDestination.name) {
                    this.setState({
                        showError: true
                    });

                    return false;
                }
                else {
                    this.setState({
                        showError: false
                    });
                }
            }
        }
        return true;
    }

    validateForm() {
        var name = this.state.destinationModel.name;
        var description = this.state.destinationModel.description;
        var hasError = false;
        var hasNameError = (name != undefined
            && (name == "" || name.length < 3 || name.length > 5 || !validator.isAlpha(name)
                || !this.isCodeUnique(this.state.destinationModel)));
        var hasDesError = (description == "");

        this.setState({
            validationStateName: hasNameError ? 'error' : null,
            validationStateDescription: hasDesError ? 'error' : null
        });

        this.props.validationStates(hasNameError, hasDesError);
    }

    componentWillMount() {
        this.setState({
            destinationModel: this.props.data
        });
    }

    componentDidMount() {
        var model = this.state.destinationModel;
        if (this.state.destinationModel.id == null) {
            model.externalId = this.getUnusedExternalId();

            this.setState({ destinationModel: model });
        }
    }

    getUnusedExternalId() {
        var externalId = 1;

        while (true) {

            if (!this.isExternaldUsed(externalId)) {
                break;
            }

            externalId++;
        }

        return externalId;
    }

    isExternaldUsed(externalId) {
        for (var x = 0; x < this.props.destinations.length; x++) {
            if (externalId == this.props.destinations[x].externalId) {
                return true;
            }
        }

        return false;
    }

    handleTextChange(event) {

        var model = this.state.destinationModel;

        model.name = event.target.value.toUpperCase();

        this.setState({
            destinationModel: model
        });

        this.validateForm();

        this.props.updateDestination(this.state.destinationModel);
    }

    handleDescriptionChange(event) {
        var model = this.state.destinationModel;
        model.description = event.target.value;

        this.setState({
            destinationModel: model
        });

        this.validateForm();

        this.props.updateDestination(this.state.destinationModel);
    }

    handleCheckboxChange(event) {
        var checked = event.target.checked;

        var model = this.state.destinationModel;

        switch (event.target.name) {
            case "hd":
                model.content.highDefinition = checked;
                break;
            case "sd":
                model.content.standardDefinition = checked;
                break;
            case "cx":
                model.content.cx = checked;
                break;
            case "nonCx":
                model.content.nonCx = checked;
                break;
            case "auditDelivery":
                model.auditDelivery = checked;
                break;
            default:
                throw "Checkbox binding not found for control" + event.target.name;
        }

        this.setState({
            destinationModel: model
        });

        this.validateForm();

        this.props.updateDestination(this.state.destinationModel);
    }

    render() {

        var msg = "";
        if (this.state.showError)
            msg = (<label data-ng-show="showError" class="alert alert-danger"><strong>Error!</strong> Destination already exists. Please use a unique destination code. To view external ids in use, visit the <a href='http://eawiki/display/turniverse/Enumerations' target='wiki'>Enumerations</a> page.</label>);

        return (
            <div>
                <Grid>
                    <Form>
                        <Row>
                            <Col md={8}>
                                {msg}
                            </Col>
                        </Row>
                        <Row>
                            <Col md={4} >
                                <FormGroup
                                    controlId="destinationCode" validationState={this.state.validationStateName}>
                                    <ControlLabel>Destination Code</ControlLabel>
                                    <FormControl
                                        type="text"
                                        disabled={this.state.destinationModel.id == null ? false : true}
                                        value={this.state.destinationModel.name}
                                        ref="inputFriendlyName"
                                        placeholder="Enter 3 to 5 alphabetical letter for destination code"
                                        onChange={this.handleTextChange.bind(this)}
                                    />
                                </FormGroup>
                            </Col>
                            <Col md={4}>

                            </Col>
                        </Row>
                        <Row>
                            <Col md={4} >
                                <FormGroup
                                    controlId="destinationDescription" validationState={this.state.validationStateDescription}>
                                    <ControlLabel>Destination Description</ControlLabel>
                                    <FormControl bsClass="form-control form-control-modal" componentClass="textarea" value={this.state.destinationModel.description} placeholder="Results"
                                        onChange={this.handleDescriptionChange.bind(this)} />
                                </FormGroup>
                            </Col>
                            <Col md={4}>
                                <ControlLabel>Content</ControlLabel>

                                <FormGroup
                                    controlId="contents">
                                    <Checkbox inline name="hd" onChange={this.handleCheckboxChange.bind(this)}
                                        checked={this.state.destinationModel.content.highDefinition}> HD <a href="#"><span data-toggle="tooltip" data-placement="right" title="High definition content can be delivered to this destination." class="glyphicon glyphicon-info-sign"></span></a></Checkbox>
                                    <Checkbox inline name="sd" className="marginLeftRight" onChange={this.handleCheckboxChange.bind(this)}
                                        checked={this.state.destinationModel.content.standardDefinition}> SD <a href="#"><span data-toggle="tooltip" data-placement="right" title="Standard definition content can be delivered to this destination." class="glyphicon glyphicon-info-sign"></span></a></Checkbox>
                                    <Checkbox inline name="cx" onChange={this.handleCheckboxChange.bind(this)}
                                        checked={this.state.destinationModel.content.cx}> C(X) <a href="#"><span data-toggle="tooltip" data-placement="right" title="C(X) content can be delivered to this destination." class="glyphicon glyphicon-info-sign"></span></a></Checkbox>
                                    <Checkbox inline name="nonCx" className="marginLeftRight" onChange={this.handleCheckboxChange.bind(this)}
                                        checked={this.state.destinationModel.content.nonCx}> Non-C(X) <a href="#"><span data-toggle="tooltip" data-placement="right" title="Non-C(X) content can be delivered to this destination." class="glyphicon glyphicon-info-sign"></span></a></Checkbox>
                                </FormGroup>
                                <ControlLabel>Options</ControlLabel>
                                <FormGroup
                                    controlId="options">
                                    <Checkbox name="auditDelivery" onChange={this.handleCheckboxChange.bind(this)}
                                        checked={this.state.destinationModel.auditDelivery}> Audit Delivery <a href="#"><span data-toggle="tooltip" data-placement="right" title="Digital Fulfillment requires a completed status to this destination before the content is considered delivered." class="glyphicon glyphicon-info-sign"></span></a></Checkbox>
                                </FormGroup>
                            </Col>
                        </Row>
                    </Form>
                </Grid>
            </div>
        )
    }
}

export default AddEditDestinationBasic