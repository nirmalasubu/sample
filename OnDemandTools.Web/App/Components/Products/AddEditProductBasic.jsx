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
            productUnModifiedData: {},
            validationStateName: null,
            validationStateDescription: null,
            validationStateExternalId: null,
            validationStateTag: null,
            showError: false,
            componentJustMounted: true,
            allTags: this.getAllTags(),
            suggestions: [],
            tagValue: "",
            delimit : [9, 13, 32]
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
        if(tags.length>0)
        {
            for (var x=0; x < this.state.allTags.length; x++) {
                if(this.state.allTags[x].name==tags[0].name){
                    if(!this.isTagExist(this.state.allTags[x]))
                        allTag.push(this.state.allTags[x]);
                }
            }
        }

        this.setState({suggestions:allTag});
    }

    /// <summary>
    /// This method is to check the tag exist for the product
    /// </summary>
    isTagExist(tag)
    {
        var product = this.state.productModel;
        for(var i=0; i < product.tags.length; i++)
        {
            if(product.tags[i].name.toLowerCase()==tag.name.toLowerCase())
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
            productModel: product,
            productUnModifiedData: jQuery.extend(true, {}, product)
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

        this.validateForm();
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
    }

    /// <summary>
    /// This method checks whether the product already exist
    /// </summary>
    isProductUnique(currentProduct) {
        for (var x = 0; x < this.props.products.length; x++) {
            if (this.props.products[x].id != currentProduct.id) {
                if (this.props.products[x].name == currentProduct.name || 
                    this.props.products[x].externalId == currentProduct.externalId) {
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

    /// <summary>
    /// This method validates the name, description and external id fields
    /// and updates the validationstate
    /// </summary>
    validateForm(tagVals) {
        var name = this.state.productModel.name.trim();
        var description = this.state.productModel.description.trim();
        var externalId = this.state.productModel.externalId;
        if(tagVals==undefined)
            tagVals = this.state.tagValue;
        var tag = {name:tagVals};
        var hasError = false;
        var hasNameError = ((name == "" || !this.isProductUnique(this.state.productModel)));
        var hasDesError = (description == "");
        var hasExternalError = (externalId!=""?!validator.isUUID(externalId):false);
        var hasTagError = ((tagVals.trim().length>0 && tagVals.trim().length<3) || (tagVals.trim().length>2?this.isTagExist(tag):false))

        if(hasNameError || hasDesError || hasExternalError || hasTagError)
            hasError = true;
        else
            hasError = false;

        this.setState({
            validationStateName: hasNameError ? 'error' : null,
            validationStateDescription: hasDesError ? 'error' : null,
            validationStateExternalId: hasExternalError ? 'error' : null,
            validationStateTag: hasTagError ? 'error' : null,
        });

        var tagVal = tagVals.trim().length>0?tagVals:null;

        this.props.validationStates(hasError, tagVal);
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

        this.validateForm();
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

        this.validateForm();
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

        this.validateForm();
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
        tag.name=tag.name.trim();
        if(tag.name.length>2)
        {
            tag.name=tag.name.replace(/ /g, '-');
            if(!this.isTagExist(tag))
            {
                let product = this.state.productModel;
                product.tags.push({
                    id: product.tags.length + 1,
                    name: tag.name
                });
                this.setState({productModel: product, tagValue:""});
                this.validateForm("");
            }
        }
    }

    /// <summary>
    /// this method is to handle the on change of the tags input value
    /// </summary>
    handleInputChange(value){
        this.setState({tagValue:value});

        if(value.length>2){
            this.getTagsBy(value);
        } 
        this.validateForm(value);
    }

    render() {
        var disable = true;
        if (this.props.permissions) {
            disable = this.props.permissions.disableControl;
        }

        var tags = null;
        if(disable)
        {
            var tagsList = [];
            for (var i = 0; i < this.state.productModel.tags.length; i++) {
                tagsList.push(this.state.productModel.tags[i].name);
            }

            tags = <FormControl
                    bsClass="form-control product-input-left"
                    type="text"
                    disabled={disable}
                    value={tagsList}
                />
        }
        else {
            tags = <ReactTags tags={this.state.productModel.tags}
            classNames = {this.state.validationStateTag==null?CLASS_NAMES:CLASS_NAMES_ERRORS}
            id = "inputTags"
            suggestions={this.state.suggestions}
            delimiters = {this.state.delimit}
            placeholder="Enter 3 or more letters"
            autocomplete={true}
            minQueryLength={3}
            allowNew={true}
            handleDelete={this.handleDelete.bind(this)}
            handleAddition={this.handleAddition.bind(this)}
            handleInputChange={this.handleInputChange.bind(this)}
                />;
        }

        var msg = "";
        if (this.state.showError)
            msg = (<label data-ng-show="showError" class="alert alert-danger"><strong>Error!</strong> Product already exists. Please use a unique product name and external id.</label>);

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
            <Col md={4} bsClass="col-md-prod-4 col" >
                                <FormGroup
                                    controlId="externalId" validationState={this.state.validationStateExternalId}>
                                    <label class="control-label" style={{paddingRight:21, fontWeight:"bold"}}>External ID</label>
                                    <FormControl
                                        bsClass="form-control product-input-left"
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
                                    controlId="mappingId">
                                    <label class="control-label" title="an identifier from another system that maps to this product like a Turniverse feed id" style={{paddingRight:10, fontWeight:"bold"}}>Mapping ID</label>
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
                            <Col md={4} bsClass="col-md-prod-4 col" >
                                <FormGroup
                                    controlId="productName" validationState={this.state.validationStateName}>
                                    <label class="control-label" style={{paddingRight:1, fontWeight:"bold"}}>Product Name</label>
                                    <FormControl
                                        disabled={disable}
                                        maxLength="150"
                                        bsClass="form-control product-input-left"
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
                                    <label class="control-label" style={{paddingRight:10, fontWeight:"bold"}}>Description</label>
                                    <FormControl
                                        disabled={disable}
                                        bsClass="form-control product-input"
                                        maxLength="150"
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
                            <Col md={4} bsClass="col-md-prod-4 col">
                                <FormGroup
                                        controlId="prdTags">
                                        <label class="control-label" style={{paddingRight:67, fontWeight:"bold", float:"left"}}>Tags</label>
                                        {tags}
                                </FormGroup>
                            </Col>
                            <Col md={4}>                                
                                <FormGroup
                                            controlId="contents">
                                            <Checkbox disabled={disable} inline name="Ad" onChange={this.handleCheckboxChange.bind(this)}
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

const CLASS_NAMES = {
    root: 'react-tags react-tags-product',
    rootFocused: 'is-focused',
    selected: 'react-tags__selected',
    selectedTag: 'react-tags__selected-tag',
    selectedTagName: 'react-tags__selected-tag-name',
    search: 'react-tags__search',
    searchInput: 'react-tags__search-input',
    suggestions: 'react-tags__suggestions',
    suggestionActive: 'is-active',
    suggestionDisabled: 'is-disabled'
};

const CLASS_NAMES_ERRORS = {
    root: 'react-tags react-tags-product_error',
    rootFocused: 'is-focused',
    selected: 'react-tags__selected',
    selectedTag: 'react-tags__selected-tag',
    selectedTagName: 'react-tags__selected-tag-name',
    search: 'react-tags__search_error',
    searchInput: 'react-tags__search-input',
    suggestions: 'react-tags__suggestions',
    suggestionActive: 'is-active',
    suggestionDisabled: 'is-disabled'
};

export default AddEditProductBasic