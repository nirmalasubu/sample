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
        
        if(typeof val == "string")
            this.props.updateFilter(val, "CN");
        else
            this.props.updateFilter(this.state.contentValue, "CN");
        
    }

    handleSelectChange(value){
        if(value == undefined)
            value ="";
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
            <FormControl type="text" inputRef = {(input) => this.inputDescription = input } onChange={this.handleChange.bind(this) }  placeholder="Description" />
        </FormGroup>
        {' '}
        <FormGroup controlId="content">
            <Select simpleValue className="destination-select-control" options={this.state.options} 
            onChange={this.handleSelectChange.bind(this)} 
            clearable={false}
            value={this.state.contentValue}
            searchable={false}
            placeholder="Content" />
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