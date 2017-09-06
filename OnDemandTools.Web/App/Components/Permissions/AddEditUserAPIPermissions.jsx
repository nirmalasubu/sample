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
class AddEditUserAPIPermissions extends React.Component {
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
            isGetChecked : false,
            isPostChecked : false,
            isDeleteChecked : false
        });

    }

    componentWillMount() {
        this.setState({
            userPermissionModel: this.props.data
        });
    }

    componentDidMount() {

    }

    /// <summary>
    /// to activate and deactivate queue permissions
    /// </summary>
    activechkChange(value, event) {
        var model = this.state.userPermissionModel;
        var isGet = ($.inArray('get', model.api.claims) > -1);
        var isPost = ($.inArray('post', model.api.claims) > -1);
        var isDelete = ($.inArray('delete', model.api.claims) > -1);

        switch (value) {            
            case "get":
                isGet = !($.inArray('get', model.api.claims) > -1);
                if(isGet)
                {
                    if($.inArray(value, model.api.claims) == -1)
                        model.api.claims.push(value);
                }
                else
                    model.api.claims.splice($.inArray(value, model.api.claims), 1);
                break;
            case "post":
                isPost = !($.inArray('post', model.api.claims) > -1);
                if(isPost)
                {
                    if($.inArray(value, model.api.claims) == -1)
                        model.api.claims.push(value);
                }
                else
                    model.api.claims.splice($.inArray(value, model.api.claims), 1);
                break;
            case "delete":
                isDelete = !($.inArray('delete', model.api.claims) > -1);
                if(isDelete)
                {
                    if($.inArray(value, model.api.claims) == -1)
                        model.api.claims.push(value);
                }
                else
                    model.api.claims.splice($.inArray(value, model.api.claims), 1);
                break;
        }

        this.setState({
            userPermissionModel: model
        });

        this.props.updatePermission(model);
    }

    /// <summary>
    /// to enable and disable  permission checkbox
    /// </summary>
    isQueuecheckboxEnabled(key) {
        if (this.state.userPermissionModel.portal.isAdmin)
            return true;
        else
            return false;
    }

    render() {
        var isget = ($.inArray('get', this.state.userPermissionModel.api.claims) > -1);
        var isPost = ($.inArray('post', this.state.userPermissionModel.api.claims) > -1);
        var isDelete = ($.inArray('delete', this.state.userPermissionModel.api.claims) > -1);
        return (
            <div>
                <div className="clearBoth">
                    <Grid componentClass="table" class="user-permission-portal-table" >
                        <thead>
                            <Row componentClass="tr">
                                <Col componentClass="th" class="user-permission-portal-module"><label>Resource</label></Col>
                                <Col componentClass="th"> <label>Get</label></Col>
                                <Col componentClass="th"><label >Post</label></Col>
                                <Col componentClass="th"><label>Delete</label></Col>
                            </Row>
                        </thead>
                        <tbody>
                            <Row componentClass="tr" >
                                <Col componentClass="td" class="user-permission-portal-module">API</Col>
                                <Col componentClass="td"><input type="checkbox" checked={isget}
                                    onChange={(event) => this.activechkChange("get", event)} 
                                    disabled={this.state.userPermissionModel.portal.isAdmin} /></Col>
                                <Col componentClass="td"><input type="checkbox" checked={isPost}
                                    onChange={(event) => this.activechkChange("post", event)} 
                                    disabled={this.state.userPermissionModel.portal.isAdmin} /></Col>
                                <Col componentClass="td"><input type="checkbox" checked={isDelete}
                                    onChange={(event) => this.activechkChange("delete", event)} 
                                    disabled={this.state.userPermissionModel.portal.isAdmin} /></Col>
                            </Row>
                        </tbody>
                    </Grid>
                </div>
            </div>
        )
    }
}

export default AddEditUserAPIPermissions