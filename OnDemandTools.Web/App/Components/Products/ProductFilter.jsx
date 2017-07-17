import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

// Sub component used within product page to search the product, tags and description values
class ProductFilter extends React.Component
{
    
    constructor(props) {
        super(props);
    }

    ///<summary>
    // React to filter control changes and bubble up
    // to parent component for further handling
    ///</summary>
    handleChange(val) {
        var cd = this.inputProduct.value;
        this.props.updateFilter(cd, "PN");  // PN is for Product Name

        var ds = this.inputDescription.value;
        this.props.updateFilter(ds, "DS");   //DS is for Descritption

        var tg = this.inputTag.value;
        this.props.updateFilter(tg, "TG");   //TG is for Tag
        
    }

    ///<summary>
    // React to 'Clear All Filter' event and
    // bubble up to parent component for further handling
    ///</summary>
    clearFilter (){
        this.inputProduct.value = "";
        this.inputDescription.value = "";
        this.inputTag.value = "";

        this.props.updateFilter("", "CL");  // CL is for clear
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
        {' '}
        <FormGroup controlId="product">
                <FormControl type="text" inputRef = {(input) => this.inputProduct = input } onChange={this.handleChange.bind(this) }  placeholder="Product" />
            </FormGroup>
        {' '}
        <FormGroup controlId="description">
            <FormControl type="text" inputRef = {(input) => this.inputDescription = input } onChange={this.handleChange.bind(this) }  placeholder="Description" />
        </FormGroup>
        {' '}
        <FormGroup controlId="tag">
            <FormControl type="text" inputRef = {(input) => this.inputTag = input } onChange={this.handleChange.bind(this) }  placeholder="Tag" />
        </FormGroup>
        {' '}
        <Button onClick={this.clearFilter.bind(this)} bsStyle="primary">
            Clear All Filters
        </Button>
    </Form>
</div>
       )
    }
}

export default ProductFilter;