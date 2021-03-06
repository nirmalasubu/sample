﻿import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';



@connect((store) => {
    return {
        config: store.config
    };
})
/// <summary>
/// Sub component of user permission page to  add ,edit user portal details
/// </summary>
class AddEditUserPortalPermissions extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            userPortalPermissionModel: "",
            userPortalPermissionunmodifiedModel: "",
            componentJustMounted: true
        });

    }

    componentWillMount() {
        this.setState({
            userPortalPermissionModel: this.props.data
        });
    }

    componentDidMount() {
        var model = this.state.userPortalPermissionModel;

        if (this.state.userPortalPermissionModel.id == null) {
            this.setState({ userPortalPermissionModel: model, componentJustMounted: true });
        }
        this.setState({ userPortalPermissionunmodifiedModel: jQuery.extend(true, {}, this.props.data) });
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
    /// to activate and deactivate portal permissions
    /// </summary>
    activechkChange(key, value, event) {
        var model = this.state.userPortalPermissionModel;
        model.portal.modulePermissions[key][value] = !this.state.userPortalPermissionModel.portal.modulePermissions[key][value];

        switch(value) {
            case "canEdit":
                {
                    model.portal.modulePermissions[key]["canRead"] = true;
                }
                break;
            case "canAdd":
                {
                    model.portal.modulePermissions[key]["canRead"] = true;
                    model.portal.modulePermissions[key]["canEdit"] = true;
                }
                break;
            case "canDelete":
                {
                    model.portal.modulePermissions[key]["canRead"] = true;
                    model.portal.modulePermissions[key]["canAdd"] = true;
                    model.portal.modulePermissions[key]["canEdit"] = true;
                }
                break;
            default:
                {
                    if(model.portal.modulePermissions[key][value])
                    model.portal.modulePermissions[key]["canRead"] = true;
                }
        }

        var unmodifiedModel = this.state.userPortalPermissionunmodifiedModel;
        if (!model.portal.modulePermissions["DeliveryQueues"]["canRead"])  // if read  is false  restore back to original state.
        {
            Object.keys(model.portal.deliveryQueuePermissions).map(function (key, index) {
                model.portal.deliveryQueuePermissions[key].canRead = unmodifiedModel.portal.deliveryQueuePermissions[key].canRead;
                model.portal.deliveryQueuePermissions[key].canAdd = false;
                model.portal.deliveryQueuePermissions[key].canEdit = false;
                model.portal.deliveryQueuePermissions[key].canDelete = false;
            });
        }
        this.setState({
            userPortalPermissionModel: model
        });

        this.props.updatePermission(model);
    }

    /// <summary>
    /// to display portal module names
    /// </summary>
    constructPortalDisplayName(key) {

        for (var i = 0; i < this.props.config.portalModules.length; i++) {
            if (this.props.config.portalModules[i].moduleName == key) {
                return <p>{this.props.config.portalModules[i].moduleDisplayName}</p>
            }
        }
    }

    /// <summary>
    /// portal checkbox construction based on permission types
    /// </summary>
    constructPortalCheckbox(key, permissionType) {
        var isAdmin = this.state.userPortalPermissionModel.portal.isAdmin;
        var row = this.state.userPortalPermissionModel.portal.modulePermissions;
        for (var i = 0; i < this.props.config.portalModules.length; i++) {
            if (this.props.config.portalModules[i].moduleName == key) {
                switch (permissionType) {
                    case "canRead":
                        return (<input type="checkbox" checked={row[key][permissionType]}
                            disabled={isAdmin || !this.props.config.portalModules[i].modulePermission[permissionType] || (row[key]["canAdd"] || row[key]["canEdit"] || row[key]["canDelete"])}
                            onChange={(event) => this.activechkChange(key, permissionType, event)} />);
                    case "canEdit":
                        return (<input type="checkbox" checked={row[key][permissionType]}
                            disabled={isAdmin || !this.props.config.portalModules[i].modulePermission[permissionType] || (row[key]["canAdd"] || row[key]["canDelete"])}
                            onChange={(event) => this.activechkChange(key, permissionType, event)} />);
                    case "canAdd":
                        return (<input type="checkbox" checked={row[key][permissionType]}
                            disabled={isAdmin || !this.props.config.portalModules[i].modulePermission[permissionType] || row[key]["canDelete"]}
                            onChange={(event) => this.activechkChange(key, permissionType, event)} />);
                    default:
                        return <input type="checkbox" checked={row[key][permissionType]} disabled={isAdmin || !this.props.config.portalModules[i].modulePermission[permissionType]} onChange={(event) => this.activechkChange(key, permissionType, event)} />
                }
            }
        }
    }


    render() {
        let row = null;
        let vals = null;
        row = this.state.userPortalPermissionModel.portal.modulePermissions;

        vals = Object.keys(row).map(function (key, index) {
            return (<Row componentClass="tr" key={index.toString()}>
                <Col componentClass="td" class="user-permission-portal-module">{this.constructPortalDisplayName(key)}</Col>
                <Col componentClass="td">{this.constructPortalCheckbox(key, "canRead")}</Col>
                <Col componentClass="td"> {this.constructPortalCheckbox(key, "canEdit")}</Col>
                <Col componentClass="td">{this.constructPortalCheckbox(key, "canAdd")}</Col>
                <Col componentClass="td">{this.constructPortalCheckbox(key, "canDelete")}</Col>
            </Row>)
        }.bind(this));

        return (
            <div>
                <div className="clearBoth modalTableContainer">
                    <Grid componentClass="table" class="user-permission-portal-table" >
                        <thead>
                            <Row componentClass="tr">
                                <Col componentClass="th" class="user-permission-portal-module"><label>Module</label></Col>
                                <Col componentClass="th"> <label>Read</label></Col>
                                <Col componentClass="th"><label >Edit</label></Col>
                                <Col componentClass="th"><label>Add</label></Col>
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

export default AddEditUserPortalPermissions