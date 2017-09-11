﻿import React from 'react';
import ReactDOM from 'react-dom';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';


@connect((store) => {
    return {
        config: store.config,
        destinations: store.destinations
    };
})
/// <summary>
/// Sub component of user permission page to  add ,edit user deliveryQueue permission details
/// </summary>
class AddEditUserDestinationPermissions extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            userPermissionModel: "",
            userPermissionunmodifiedModel: "",
            componentJustMounted: true,
            filterValue : {
                code : "",
                description : ""
            }
        });

    }

    componentWillMount() {
        this.setState({
            userPermissionModel: this.props.data
        });
    }

    componentDidMount() {
        var model = this.state.userPermissionModel;
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        
    }

    activechkChange(value, event) {
        var model = this.state.userPermissionModel;
        var isGet = !($.inArray(value, model.api.destinations) > -1);

        if(isGet)
        {
            if($.inArray(value, model.api.destinations) == -1)
                model.api.destinations.push(value);
        }
        else
            model.api.destinations.splice($.inArray(value, model.api.destinations), 1);        

        this.setState({
            userPermissionModel: model
        });

        this.props.updatePermission(model);
    }

    /// <summary>
    /// to display destination description
    /// </summary>
    constructDescription(item) {
        for (var i = 0; i < this.props.destinations.length; i++) {
            if (this.props.destinations[i].name == item) {
                return <p>{this.props.destinations[i].description}</p>
            }
        }
    }

    /// <summary>
    /// handles the filter value onchange
    /// </summary>
    handleChange(chr, event) {
        var filterState = this.state.filterValue;

        if(chr=="CD")
            filterState.code = event.target.value;
        
        if(chr=="DS")
            filterState.description = event.target.value;

        this.setState({filterValue : filterState});
    }

    // The goal of this function is to filter 'destinations'
    // based on user provided filter criteria and return the refined 'destination' list.
    // If no filter criteria is provided then return the full 'destination' list
    applyFilter(permissions, filter) {        
        var filteredPermissions = permissions;
        if (filter.code != undefined && permissions != undefined) {
            var code = filter.code.toLowerCase();
            var description = filter.description.toLowerCase();

            var filteredDestination = (this.props.destinations.filter(obj=> (code != "" ? obj.name.toLowerCase().indexOf(code) != -1 : true)
                  &&(description != "" ? obj.description.toLowerCase().indexOf(description) != -1 : true)
                ));
            if(filteredDestination.length > 0)
            {
                return filteredDestination.filter(function(e){return (this.indexOf(e.name) < 0); } , filteredPermissions.api.destinations);
            }
        }

        return this.props.destinations;
    }

    clearFilter()
    {
        this.inputQueue.value = "";
        var filterState = this.state.filterValue;
        filterState.queueName = "";

        this.setState({ filterValue: filterState });

        //Moves the scroll bar to top if there is too many contents
        let node = ReactDOM.findDOMNode(this.refs.queueContainer);
        if (node) {
            node.scrollTop = 0;
        }
    }

    render() {
        let row = null;
        let vals = null;
        row = this.applyFilter(this.state.userPermissionModel, this.state.filterValue);        

        if(row.length > 0)
        {
            vals = row.map(function (item, index) {
                return (<Row componentClass="tr" key={index.toString()}>
                    <Col componentClass="td" class="user-permission-portal-module">{item.name}</Col>
                    <Col componentClass="td">{item.description}</Col>
                    <Col componentClass="td"> <input type="checkbox" checked={false}
                        onChange={(event) => this.activechkChange(item.name, event)}
                        disabled={this.state.userPermissionModel.portal.isAdmin} /></Col>
                </Row>)
            }.bind(this));
        }
        else{
            vals = (
                    <Row componentClass="tr">
                        <Col componentClass="td" colSpan={3} >There is no data to display</Col>
                    </Row>
                );
        }

        return (
            <div>
                <br/>
                <Form inline>
                  <ControlLabel>Filter by: </ControlLabel>
                    {' '}
                    <FormGroup controlId="name">
                      <FormControl type="text" inputRef={(input) => this.inputCode = input} onChange={(event) => this.handleChange("CD", event)} placeholder="Code" />
                    </FormGroup>
                    {' '}
                    <FormGroup controlId="description">
                      <FormControl type="text" inputRef={(input) => this.inputDes = input} onChange={(event) => this.handleChange("DS", event)} placeholder="Description" />
                    </FormGroup>
                    {' '}
                    <Button onClick={this.clearFilter.bind(this)} bsStyle="primary">
                        Clear Filter
                    </Button>
                </Form>
                <div className="clearBoth modalTableContainer" ref="queueContainer">
                    <Grid componentClass="table" class="user-permission-portal-table" >
                        <thead>
                            <Row componentClass="tr">
                                <Col componentClass="th" ><label>Code</label></Col>
                                <Col componentClass="th" class="user-permission-portal-module"> <label>Description</label></Col>
                                <Col componentClass="th"><label >Get</label></Col>
                            </Row>
                        </thead>
                        <tbody>
                            {vals}
                        </tbody>
                    </Grid>
                </div>

            </div>
        )
    }
}

export default AddEditUserDestinationPermissions