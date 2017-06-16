import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';

// Sub component used within Delivery queues page
class DeliveryQueueFilter extends React.Component
{
    // Define default component state information. 
    constructor(props) {
        super(props);

        this.state = {
            isInActive: false
        };
      
    }

    // React to filter control changes and bubble up
    // to parent component for further handling
    handleChange() {
        var qn = this.inputQueueName.value;
        this.props.updateFilter(qn, "QN");

        var cn = this.inputContactName.value;
        this.props.updateFilter(cn, "CN");

        var qi = this.inputQueueId.value;
        this.props.updateFilter(qi, "QI");

        this.props.updateFilter(this.state.isInActive, "CB");
    }

    // React to checkbox 'Include Inactive' event and
    // bubble up to parent component for further handling
    handleCheckBoxChange(event){
        var checked = event.target.checked;
        this.state.isInActive = checked;
        this.handleChange();
    }

    // React to 'Clear All Filter' event and
    // bubble up to parent component for further handling
    clearFilter (){
        this.inputQueueName.value = "";
        this.inputContactName.value = "";
        this.inputQueueId.value = "";
        this.state.isInActive=false;

        this.props.updateFilter("", "CL");
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
                    {' '}
                    <FormGroup controlId="queueName">
                            <FormControl type="text" inputRef = {(input) => this.inputQueueName = input } onChange={this.handleChange.bind(this) }  placeholder="Queue Name" />
                        </FormGroup>
                    {' '}
                    <FormGroup controlId="contactName">
                        <FormControl type="text" inputRef = {(input) => this.inputContactName = input } onChange={this.handleChange.bind(this) }  placeholder="Contact Name" />
                    </FormGroup>
                    {' '}
                    <FormGroup controlId="queueId">
                        <FormControl type="text" inputRef = {(input) => this.inputQueueId = input } onChange={this.handleChange.bind(this) }  placeholder="Queue ID" />
                    </FormGroup>
                    {' '}
                    <FormGroup
                        controlId="isActive">
                        <Checkbox name="active" onChange={this.handleCheckBoxChange.bind(this) }
                        checked={this.state.isInActive}> Include Inactive</Checkbox>
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

export default DeliveryQueueFilter;