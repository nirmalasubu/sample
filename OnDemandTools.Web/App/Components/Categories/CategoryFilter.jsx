import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

// Sub component used within category page to search the category and destination  values
class CategoryFilter extends React.Component
{
    
    constructor(props) {
        super(props);
    }

    ///<summary>
    // React to filter control changes and bubble up
    // to parent component for further handling
    ///</summary>
    handleChange(val) {
        var cd = this.inputCategory.value;
        this.props.updateFilter(cd, "CN");  // CN is for Category Name

        var ds = this.inputDestination.value;
        this.props.updateFilter(ds, "DS");   //DS is for Destination
        
    }

    ///<summary>
    // React to 'Clear All Filter' event and
    // bubble up to parent component for further handling
    ///</summary>
    clearFilter (){
        this.inputCategory.value = "";
        this.inputDestination.value = "";

        this.props.updateFilter("", "CL");  // CL is for clear
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
        {' '}
        <FormGroup controlId="category">
                <FormControl type="text" inputRef = {(input) => this.inputCategory = input } onChange={this.handleChange.bind(this) }  placeholder="Category Name" />
            </FormGroup>
        {' '}
        <FormGroup controlId="destination">
            <FormControl type="text" inputRef = {(input) => this.inputDestination = input } onChange={this.handleChange.bind(this) }  placeholder="Destination" />
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

export default CategoryFilter;