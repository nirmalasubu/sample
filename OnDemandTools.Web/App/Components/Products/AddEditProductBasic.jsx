import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';
import InfoOverlay from 'Components/Common/InfoOverlay';

@connect((store) => {
    return {
        destinations: store.destinations
    };
})
/// <summary>
/// Sub component of product page to  add ,edit product basic info details
/// </summary>
class AddEditProductBasic extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            productModel: {},
            validationStateName: null,
            validationStateDescription: null,
            validationStateExternalId: null,
            validationStateMappingId: null,
            showError: false,
            componentJustMounted: true
        });
    }

    /// <summary>
    /// receives prop changes to update state
    /// </summary>    
    componentWillReceiveProps(nextProps) {
        //this.setState({
        //    destinationModel: nextProps.data
        //}, function () {
        //    if (this.state.componentJustMounted) {
        //        this.setState({ componentJustMounted: false }, function () {
        //            this.validateForm();
        //        });
        //    }
        //});
    }

    isCodeUnique(currentDestination) {
        //for (var x = 0; x < this.props.destinations.length; x++) {
        //    if (this.props.destinations[x].id != currentDestination.id) {
        //        if (this.props.destinations[x].name == currentDestination.name) {
        //            this.setState({
        //                showError: true
        //            });

        //            return false;
        //        }
        //        else {
        //            this.setState({
        //                showError: false
        //            });
        //        }
        //    }
        //}
        //return true;
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

    /// <summary>
    /// This method sets the state with the product details before the component gets mounted
    /// </summary> 
    componentWillMount() {
        this.setState({
            destinationModel: this.props.data
        });
    }

    /// <summary>
    /// This method sets the state after the component gets mounted
    /// </summary> 
    componentDidMount() {
        var model = this.state.destinationModel;
        if (this.state.destinationModel.id == null) {
            this.setState({ destinationModel: model, componentJustMounted: true });
        }
    }

    /// <summary>
    /// Updating the state on change of External ID
    /// </summary>
    handleExternalChange(event) {
        var model = this.state.productModel;
        model.externalId = event.target.value;
        this.setState({
            productModel: model
        });

        //this.validateForm();

        //this.props.updateDestination(this.state.destinationModel);
    }

    /// <summary>
    /// Updating the state on change of Mapping ID
    /// </summary>
    handleMappingChange(event) {
        var model = this.state.productModel;
        model.mappingId = event.target.value;
        this.setState({
            productModel: model
        });

        //this.validateForm();

        //this.props.updateDestination(this.state.destinationModel);
    }

    /// <summary>
    /// Updating the state on change of Product Name
    /// </summary>
    handleNameChange(event) {
        var model = this.state.productModel;
        model.name = event.target.value
        this.setState({
            productModel: model
        });

        //this.validateForm();

        //this.props.updateDestination(this.state.destinationModel);
    }

    /// <summary>
    /// Updating the state on change of Description
    /// </summary>
    handleDescriptionChange(event) {
        var model = this.state.productModel;
        model.description = event.target.value;

        this.setState({
            productModel: model
        });

        //this.validateForm();

        //this.props.updateDestination(this.state.destinationModel);
    }

    /// <summary>
    /// Updating the state on change of product tags
    /// </summary>
    handleTagsChange(event) {
        var tag = {text:""};
        tag.text = event.target.value;
        var model = this.state.productModel;
        model.tags.push(tag);

        this.setState({
            productModel: model
        });

        //this.validateForm();

        //this.props.updateDestination(this.state.destinationModel);
    }

    handleCheckboxChange(event) {
        var checked = event.target.checked;

        var model = this.state.destinationModel
        model.dynamicAdTrigger = checked;

        this.setState({
            destinationModel: model
        });

        //this.validateForm();

        //this.props.updateDestination(this.state.destinationModel);
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
                                    controlId="externalId" validationState={this.state.validationStateExternalId}>
                                    <span style={{paddingRight:30}}>External ID</span>
                                    <FormControl
                                        bsClass="form-control product-input"
                                        type="text"
                                        disabled={this.state.productModel.id == null ? false : true}
                                        value={this.state.productModel.externalId}
                                        ref="inputExternalId"
                                        placeholder="External ID"
                                        onChange={this.handleExternalChange.bind(this)}
                                    />
                                </FormGroup>
                            </Col>
                            <Col md={4}>
                                <FormGroup
                                    controlId="mappingId" validationState={this.state.validationStateMappingId}>
                                    <span style={{paddingRight:10}}>Mapping ID</span>
                                    <FormControl
                                        bsClass="form-control product-input"
                                        type="text"
                                        disabled={this.state.productModel.id == null ? false : true}
                                    value={this.state.productModel.mappingId}
                                ref="inputMappingId"
                                placeholder="Mapping ID"
                                onChange={this.handleMappingChange.bind(this)}
                                    />
                                </FormGroup>
                            </Col>
                        </Row>
                        <Row>
                            <Col md={4} >
                                <FormGroup
                                    controlId="productName" validationState={this.state.validationStateName}>
                                    <span style={{paddingRight:10}}>Product Name</span>
                                    <FormControl
                                        bsClass="form-control product-input"
                                        type="text"
                                        disabled={this.state.productModel.id == null ? false : true}
                                        value={this.state.productModel.name}
                                        ref="inputProductName"
                                        placeholder="Product Name"
                                        onChange={this.handleNameChange.bind(this)}
                                                />
                                </FormGroup>
                            </Col>
                            <Col md={4} >
                                <FormGroup
                                    controlId="prdDescription" validationState={this.state.validationStateDescription}>
                                    <span style={{paddingRight:10}}>Description</span>
                                    <FormControl
                                        bsClass="form-control product-input"
                                        type="text"
                                        value={this.state.productModel.description}
                                        ref="inputDescription"
                                        placeholder="Description"
                                        onChange={this.handleDescriptionChange.bind(this)}
                                    />
                                </FormGroup>
                            </Col>                                                        
                        </Row>
                        <Row>
                            <Col md={4} >
                                <FormGroup
                                        controlId="prdTags" validationState={this.state.validationStateTags}>
                                        <span style={{paddingRight:67, fontWeight:20}}>Tags</span>
                                        <FormControl
                                            bsClass="form-control product-input"
                                            type="text"
                                            value={this.state.productModel.description}
                                            ref="inputTags"
                                            placeholder="Tags"
                                            onChange={this.handleTagsChange.bind(this)}
                                        />
                                </FormGroup>
                            </Col>
                            <Col md={4}>                                
                                <FormGroup
                                    controlId="contents">
                                    <Checkbox inline name="Ad" onChange={this.handleCheckboxChange.bind(this)}
                                        checked={this.state.productModel.dynamicAdTrigger}>Dynamic Ad Trigger</Checkbox>
                                </FormGroup>
                            </Col>
                        </Row>
                    </Form>
                </Grid>
            </div>
        )
    }
}

export default AddEditProductBasic