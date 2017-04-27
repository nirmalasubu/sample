import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import $ from 'jquery';

var DeliveryQueueFilter = React.createClass({
    handleChange: function() {
        var qn = this.inputQueueName.value;
        this.props.updateFilter(qn, "QN");

        var cn = this.inputContactName.value;
        this.props.updateFilter(cn, "CN");

        var qi = this.inputQueueId.value;
        this.props.updateFilter(qi, "QI");
    },

    clearFilter : function(){
        this.inputQueueName.value = "";
        this.inputContactName.value = "";
        this.inputQueueId.value = "";

        this.props.updateFilter("", "CL");
    },

    render: function() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter by: </ControlLabel>
        {' '}
        <FormGroup controlId="queueName">
            <FormControl type="text" ref="filterInput" inputRef = {(input) => this.inputQueueName = input } onChange={this.handleChange} placeholder="Queue Name" />
        </FormGroup>
        {' '}
        <FormGroup controlId="contactName">
            <FormControl type="text" inputRef = {(input) => this.inputContactName = input } onChange={this.handleChange}  placeholder="Contact Name" />
        </FormGroup>
        {' '}
        <FormGroup controlId="queueId">
            <FormControl type="text" inputRef = {(input) => this.inputQueueId = input } onChange={this.handleChange} placeholder="Queue ID" />
        </FormGroup>
        {' '}
        <Button onClick={this.clearFilter} bsStyle="primary">
            Clear All Filters
        </Button>
    </Form>
</div>
        )
    }
});

export default DeliveryQueueFilter;