import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

// Sub component used within product page to search the product, tags and description values
class WorkflowStatusesFilter extends React.Component
{
    
    constructor(props) {
        super(props);
    }

    ///<summary>
    // React to filter control changes and bubble up
    // to parent component for further handling
    ///</summary>
    handleChange(val) {
        
        var sn = this.inputName.value;
        this.props.updateFilter(sn, "SN");  // SN is for Status Name

        var ds = this.inputDescription.value;
        this.props.updateFilter(ds, "DS");   //DS is for Descritption

        var us = this.inputUser.value;
        this.props.updateFilter(us, "US");   //US is for user
        
    }

    ///<summary>
    // React to 'Clear All Filter' event and
    // bubble up to parent component for further handling
    ///</summary>
    clearFilter (){
        this.inputName.value = "";
        this.inputDescription.value = "";
        this.inputUser.value = "";

        this.props.updateFilter("", "CL");  // CL is for clear
    }

    ///<summary>
    /// capitalize the status name text
    ///</summary>
    ConvertToUpperCase(){
        this.inputName.value=this.inputName.value.toUpperCase();
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
        {' '}
        <FormGroup controlId="name">
                <FormControl type="text" inputRef = {(input) => this.inputName = input } onKeyUp={this.ConvertToUpperCase.bind(this)} onChange={this.handleChange.bind(this) }  placeholder="Status Name" />
            </FormGroup>
        {' '}
        <FormGroup controlId="description">
            <FormControl type="text" inputRef = {(input) => this.inputDescription = input } onChange={this.handleChange.bind(this) }  placeholder="Description" />
        </FormGroup>
        {' '}
        <FormGroup controlId="user">
            <FormControl type="text" inputRef = {(input) => this.inputUser = input } onChange={this.handleChange.bind(this) }  placeholder="User Group" />
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

export default WorkflowStatusesFilter;