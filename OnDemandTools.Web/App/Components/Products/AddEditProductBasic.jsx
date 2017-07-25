import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';
import InfoOverlay from 'Components/Common/InfoOverlay';
import ReactTags from 'react-tag-autocomplete';
import 'react-tag-autocomplete/example/styles.css';

@connect((store) => {
    return {
        products : store.products
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
            componentJustMounted: true,
            allTags: this.getAllTags(),
            suggestions: []
        });
    }

    /// <summary>
    /// This method is to get all tags from products for tags suggestion
    /// </summary>
    getAllTags() {    
        var tags = [];
        var i=1;
        for (var x = 0; x < this.props.products.length; x++) {
            for (var y = 0; y < this.props.products[x].tags.length; y++) {
                if ( this.isTagMissing(tags, this.props.products[x].tags[y])) {
                    var tag={id:i,name:this.props.products[x].tags[y].name};
                    tags.push(tag);
                    i++;
                }
            }
        }

        return tags;
    }

    /// <summary>
    /// This method is to get tags by name from products for tags suggestion
    /// </summary>
    getTagsBy(query) {
        var regExp = new RegExp(query.toLowerCase());        
        var tags = [];
        var allTag = [];

        for (var x = 0; x < this.props.products.length; x++) {
            for (var y = 0; y < this.props.products[x].tags.length; y++) {
                if ((regExp.test(this.props.products[x].tags[y].name.toLowerCase()) && this.isTagMissing(tags, this.props.products[x].tags[y]))) {
                    tags.push(this.props.products[x].tags[y]);
                }                    
            }
        }
        for (var x=0; x < this.state.allTags.length; x++) {
            if(this.state.allTags[x].name==tags[0].name){
                if(!this.isTagExist(this.state.allTags[x]))
                    allTag.push(this.state.allTags[x]);
            }
        }

        this.setState({suggestions:allTag});
    }

    /// <summary>
    /// This method is to check the tag exist for the product
    /// </summary>
    isTagExist(tag)
    {
        console.log(tag.name);
        var product = this.state.productModel;
        for(var i=0; i < product.tags.length; i++)
        {
            if(product.tags[i].name==tag.name)
                return true;
        }

        return false;
    }

    /// <summary>
    /// This method is to avoid duplication of tags
    /// </summary>
    isTagMissing(tags, tag) {
        for (var x = 0; x < tags.length; x++) {
            if (tags[x].name.toLowerCase() == tag.name.toLowerCase())
                return false;
        }
        return true;
    }

    /// <summary>
    /// This method sets the state with the product details before the component gets mounted
    /// </summary> 
    componentWillMount() {
        var i=1;
        let product = this.props.data;
        product.tags.map(function(tag){
            tag.id=i;
            i++;
        }
        );

        this.setState({
            productModel: product
        });
    }

    /// <summary>
    /// This method sets the state after the component gets mounted
    /// </summary> 
    componentDidMount() {
        var model = this.state.productModel;
        if (this.state.productModel.id == null) {
            this.setState({ productModel: model, componentJustMounted: true });
        }
    }

    /// <summary>
    /// receives prop changes to update state
    /// </summary>    
    componentWillReceiveProps(nextProps) {
        //this.setState({
        //    suggestions: nextProps.suggestions
        //}, function () {
            //if (this.state.componentJustMounted) {
            //    this.setState({ componentJustMounted: false }, function () {
            //        this.validateForm();
            //    });
            //}
        //});
        //console.log(this.state.suggestions);
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
    /// Updating the state on change of dynamic ad trigger checkbox
    /// </summary>
    handleCheckboxChange(event) {
        var checked = event.target.checked;

        var model = this.state.productModel;
        model.dynamicAdTrigger = checked;

        this.setState({
            productModel: model
        });

        //this.validateForm();

        //this.props.updateDestination(this.state.destinationModel);
    }

    /// <summary>
    /// this method is to delete the tags in the state
    /// </summary>
    handleDelete(i) {
        let product = this.state.productModel;
        product.tags.splice(i, 1);
        this.setState({productModel: product});
    }
    
    /// <summary>
    /// this method is to handle the addition of tags to the state
    /// </summary>
    handleAddition(tag) {
        let product = this.state.productModel;
        product.tags.push({
            id: product.tags.length + 1,
            name: tag.name
        });
        this.setState({productModel: product});
    }

    /// <summary>
    /// this method is to handle the on change of the tags input value
    /// </summary>
    handleInputChange(value){
        if(value.length>2){
            this.getTagsBy(value);
        }
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
                                        placeholder="If blank, guid will be generated."
                                        onChange={this.handleExternalChange.bind(this)}
                                    />
                                </FormGroup>
                            </Col>
                            <Col md={4}>
                                <FormGroup
                                    controlId="mappingId" validationState={this.state.validationStateMappingId}>
                                    <span title="an identifier from another system that maps to this product like a Turniverse feed id" style={{paddingRight:10}}>Mapping ID</span>
                                    <FormControl
                                        bsClass="form-control product-input"
                                        type="number"
                                        disabled={this.state.productModel.id == null ? false : true}
                                        value={this.state.productModel.mappingId}
                                        ref="inputMappingId"
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
                                        <ReactTags tags={this.state.productModel.tags}
                                            id = "inputTags"
                                            suggestions={this.state.suggestions}
                                            autocomplete={true}
                                            minQueryLength={3}
                                            allowNew={true}
                                            handleDelete={this.handleDelete.bind(this)}
                                            handleAddition={this.handleAddition.bind(this)}
                                            handleInputChange={this.handleInputChange.bind(this)}
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