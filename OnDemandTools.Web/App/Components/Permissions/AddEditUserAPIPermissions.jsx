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
        var isget = ($.inArray('get', this.state.userPermissionModel.api.claims) > -1);
        var isPost = ($.inArray('post', this.state.userPermissionModel.api.claims) > -1);
        var isDelete = ($.inArray('delete', this.state.userPermissionModel.api.claims) > -1);

        this.setState({ 
            isGetChecked: isget,
            isPostChecked: isPost,
            isDeleteChecked: isDelete,
            componentJustMounted: true
        });
    }

    /// <summary>
    /// to activate and deactivate queue permissions
    /// </summary>
    activechkChange(value, event) {
        var model = this.state.userPermissionModel;
        var isGet = this.state.isGetChecked;
        var isPost = this.state.isPostChecked;
        var isDelete = this.state.isDeleteChecked;

        switch (value) {            
            case "get":
                isGet = !this.state.isGetChecked;
                if(isGet)
                {
                    if($.inArray(value, model.api.claims) == -1)
                        model.api.claims.push(value);
                }
                else
                    model.api.claims.splice($.inArray(value, model.api.claims), 1);
                break;
            case "post":
                isPost = !this.state.isPostChecked;
                if(isPost)
                {
                    if($.inArray(value, model.api.claims) == -1)
                        model.api.claims.push(value);
                }
                else
                    model.api.claims.splice($.inArray(value, model.api.claims), 1);
                break;
            case "delete":
                isDelete = !this.state.isDeleteChecked;
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
            userPermissionModel: model,
            isGetChecked : isGet,
            isPostChecked : isPost,
            isDeleteChecked : isDelete
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
                                <Col componentClass="td"><input type="checkbox" checked={this.state.isGetChecked}
                                    onChange={(event) => this.activechkChange("get", event)} 
                                    disabled={false} /></Col>
                                <Col componentClass="td"><input type="checkbox" checked={(this.state.isPostChecked)}
                                    onChange={(event) => this.activechkChange("post", event)} 
                                    disabled={false} /></Col>
                                <Col componentClass="td"><input type="checkbox" checked={this.state.isDeleteChecked}
                                    onChange={(event) => this.activechkChange("delete", event)} 
                                    disabled={false} /></Col>
                            </Row>
                        </tbody>
                    </Grid>
                </div>
            </div>
        )
    }
}

export default AddEditUserAPIPermissions