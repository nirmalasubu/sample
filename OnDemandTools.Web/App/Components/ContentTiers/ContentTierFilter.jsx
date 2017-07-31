import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

// Sub component used within category page to search the category and product  values
class ContentTierFilter extends React.Component {

    constructor(props) {
        super(props);
    }

    ///<summary>
    // React to filter control changes and bubble up
    // to parent component for further handling
    ///</summary>
    handleChange(val) {
        var cd = this.inputContentTier.value;
        this.props.updateFilter(cd, "CN");  // CN is for ContentTier Name

        var ds = this.inputProduct.value;
        this.props.updateFilter(ds, "DS");   //DS is for Product

    }

    ///<summary>
    // React to 'Clear All Filter' event and
    // bubble up to parent component for further handling
    ///</summary>
    clearFilter() {
        this.inputContentTier.value = "";
        this.inputProduct.value = "";

        this.props.updateFilter("", "CL");  // CL is for clear
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
                    {' '}
                    <FormGroup controlId="category">
                        <FormControl type="text" inputRef={(input) => this.inputContentTier = input} onChange={this.handleChange.bind(this)} placeholder="ContentTier Name" />
                    </FormGroup>
                    {' '}
                    <FormGroup controlId="product">
                        <FormControl type="text" inputRef={(input) => this.inputProduct = input} onChange={this.handleChange.bind(this)} placeholder="Product" />
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

export default ContentTierFilter;