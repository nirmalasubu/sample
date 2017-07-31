import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

// Sub component used within distribution page to search the current airing ids and prefix values
class DistributionFilter extends React.Component
{
    
    constructor(props) {
        super(props);
    }

    ///<summary>
    // React to filter control changes and bubble up
    // to parent component for further handling
    ///</summary>
    handleChange(val) {
        var cd = this.inputPrefix.value;
        this.props.updateFilter(cd, "CD");  // CD is for code prefix

        var ds = this.inputAiringId.value;
        this.props.updateFilter(ds, "AI");   //DS is for airing id
        
    }

    ///<summary>
    // React to 'Clear All Filter' event and
    // bubble up to parent component for further handling
    ///</summary>
    clearFilter (){
        this.inputPrefix.value = "";
        this.inputAiringId.value = "";

        this.props.updateFilter("", "CL");  // CL is for clear
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
        {' '}
        <FormGroup controlId="code">
                <FormControl type="text" inputRef = {(input) => this.inputPrefix = input } onChange={this.handleChange.bind(this) }  placeholder="Code" />
            </FormGroup>
        {' '}
        <FormGroup controlId="airingid">
            <FormControl type="text" inputRef = {(input) => this.inputAiringId = input } onChange={this.handleChange.bind(this) }  placeholder="Airing ID" />
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

export default DistributionFilter;