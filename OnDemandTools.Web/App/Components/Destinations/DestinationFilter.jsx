import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

class DestinationFilter extends React.Component
{
    
    constructor(props) {
        super(props);

        this.state = {
            options: [
                { value: 'HD', label: 'HD' },
                { value: 'SD', label: 'SD' },
                { value: 'C3', label: 'C(X)' },
                { value: 'NonC3', label: 'Non-C(X)' },
            ],
            contentValue: ""
        };
      
    }
    handleChange(val) {
        var cd = this.inputCode.value;
        this.props.updateFilter(cd, "CD");

        var ds = this.inputDescription.value;
        this.props.updateFilter(ds, "DS");

        var cn
        if(typeof val == "string")
            cn = val;
        else
            cn = "";
        this.props.updateFilter(cn, "CN");
    }

    handleSelectChange(value){
        this.setState({
            contentValue: value
        });
        this.handleChange(value);
    }

    clearFilter (){
        this.inputCode.value = "";
        this.inputDescription.value = "";

        this.setState({
            contentValue: ""
        });

        this.props.updateFilter("", "CL");
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
        {' '}
        <FormGroup controlId="code">
                <FormControl type="text" inputRef = {(input) => this.inputCode = input } onChange={this.handleChange.bind(this) }  placeholder="Code" />
            </FormGroup>
        {' '}
        <FormGroup controlId="description">
            <FormControl type="text" inputRef = {(input) => this.inputDescription = input } onChange={this.handleChange.bind(this) }  placeholder="Descritpion" />
        </FormGroup>
        {' '}
        <FormGroup controlId="content">
            <Select simpleValue options={this.state.options} onChange={this.handleSelectChange.bind(this)} value={this.state.contentValue} placeholder="Content" />
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

export default DestinationFilter;