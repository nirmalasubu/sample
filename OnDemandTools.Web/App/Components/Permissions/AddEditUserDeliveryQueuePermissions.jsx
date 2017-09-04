import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';

@connect((store) => {
    return {
        config: store.config,
        queues: store.queues
    };
})
/// <summary>
/// Sub component of user permission page to  add ,edit user deliveryQueue permission details
/// </summary>
class AddEditUserDeliveryQueuePermissions extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            userQueuePermissionModel: "",
            userQueuePermissionunmodifiedModel: "",
            componentJustMounted: true,
            filterValue : {
                queueName : ""
            }
        });

    }

    componentWillMount() {
        // Dispatch another action to asynchronously fetch full list of queue data
        // from server. Once it is fetched, the data will be stored
        // in redux store
        this.props.dispatch(queueActions.fetchQueues());

        this.setState({
            userQueuePermissionModel: this.props.data
        });
    }

    componentDidMount() {
        var model = this.state.userQueuePermissionModel;
        if (this.state.userQueuePermissionModel.id == null) {
            this.setState({ userQueuePermissionModel: model, componentJustMounted: true });
        }
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        this.setState({
            userPortalPermissionModel: nextProps.data
        }, function () {
            if (this.state.componentJustMounted) {
                this.setState({ componentJustMounted: false }, function () {

                });
            }
        });

    }

    /// <summary>
    /// to activate and deactivate queue permissions
    /// </summary>
    activechkChange(key, value, event) {
        var model = this.state.userQueuePermissionModel;

        model.portal.deliveryQueuePermissions[key][value] = !this.state.userQueuePermissionModel.portal.deliveryQueuePermissions[key][value];

        if ((value == "canAdd" || value == "canEdit" || value == "canDelete") && model.portal.deliveryQueuePermissions[key][value]) {
            model.portal.deliveryQueuePermissions[key]["canRead"] = true;
        }

        this.setState({
            userQueuePermissionModel: model
        });

        this.props.updatePermission(model);
    }

    /// <summary>
    /// to display queue names
    /// </summary>
    constructQueueDisplayName(key) {
        for (var i = 0; i < this.props.queues.length; i++) {
            if (this.props.queues[i].name == key) {
                return <p>{this.props.queues[i].friendlyName}</p>
            }
        }
    }

    /// <summary>
    /// to enable and disable  permission checkbox
    /// </summary>
    isQueuecheckboxEnabled(key) {
        if (this.state.userQueuePermissionModel.portal.isAdmin)
            return true;
        else
            return false;
    }

    /// <summary>
    /// handles the filter value onchange
    /// </summary>
    handleChange(event) {
        var filterState = this.state.filterValue;
        filterState.queueName = event.target.value;
        this.setState({filterValue : filterState});
    }

    // The goal of this function is to filter 'queues'
    // based on user provided filter criteria and return the refined 'queue' list.
    // If no filter criteria is provided then return the full 'queue' list
    applyFilter(permissions, filter) {
        const newObj = {};

        if (filter.queueName != undefined && permissions != undefined) {
            var filteredPermissions = permissions;

            var queueName = filter.queueName.toLowerCase();

            if (queueName != "") {
                
                $.each(this.props.queues, function( key, value ) { 
                    if(value.friendlyName.toLowerCase().indexOf(queueName) > -1)
                    {
                        newObj[value.name] = filteredPermissions[value.name];
                    }
                });

                return newObj;
            }
        }

        return permissions;
    }

    clearFilter()
    {
        this.inputQueue.value = "";
        var filterState = this.state.filterValue;
        filterState.queueName = "";

        this.setState({ filterValue: filterState });
    }

    render() {
        let row = null;
        let vals = null;
        row = this.applyFilter(this.state.userQueuePermissionModel.portal.deliveryQueuePermissions, this.state.filterValue);

        if(Object.keys(row).length>0)
        {
            vals = Object.keys(row).map(function (key, index) {
                return (<Row componentClass="tr" key={index.toString()}>
                    <Col componentClass="td" class="user-permission-portal-module">{this.constructQueueDisplayName(key)}</Col>
                    <Col componentClass="td"><input type="checkbox" checked={row[key].canRead}
                        onChange={(event) => this.activechkChange(key, "canRead", event)} 
                        disabled={this.state.userQueuePermissionModel.portal.isAdmin} /></Col>
                    <Col componentClass="td"> <input type="checkbox" checked={row[key].canAdd}
                        onChange={(event) => this.activechkChange(key, "canAdd", event)}
                        disabled={true} /></Col>
                    <Col componentClass="td"><input type="checkbox" checked={row[key].canEdit}
                        onChange={(event) => this.activechkChange(key, "canEdit", event)}
                        disabled={true} /></Col>
                    <Col componentClass="td"><input type="checkbox" checked={row[key].canDelete}
                        onChange={(event) => this.activechkChange(key, "canDelete", event)}
                        disabled={true} /></Col>
                </Row>)
            }.bind(this));
        }
        else{
            vals = (
                    <Row componentClass="tr">
                        <Col componentClass="td" colSpan={5} >There is no data to display</Col>
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
                      <FormControl type="text" inputRef={(input) => this.inputQueue = input} onChange={(event) => this.handleChange(event)} placeholder="Queue Name" />
                    </FormGroup>
                    {' '}
                    <Button onClick={this.clearFilter.bind(this)} bsStyle="primary">
                        Clear All Filters
                    </Button>
                </Form>
                <div className="clearBoth modalTableContainer">
                    <Grid componentClass="table" class="user-permission-portal-table" >
                        <thead>
                            <Row componentClass="tr">
                                <Col componentClass="th" class="user-permission-portal-module"><label>Queue Name</label></Col>
                                <Col componentClass="th"> <label>Read</label></Col>
                                <Col componentClass="th"><label >Add</label></Col>
                                <Col componentClass="th"><label>Edit</label></Col>
                                <Col componentClass="th"><label>Delete</label></Col>
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

export default AddEditUserDeliveryQueuePermissions