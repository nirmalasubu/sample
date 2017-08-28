import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';



@connect((store) => {
    return {
        permissions: store.permissions
    };
})
/// <summary>
/// Sub component of product page to  add ,edit user basic details
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
        this.handleCheckboxChange.bind(this);
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

    handleCheckboxChange(event)
    {
       console.log("sdafsda"+ event)
            
    }

    rowConstruct(){

        let row = null;
        let vals=null;
        row = this.state.userPortalPermissionModel.portal.modulePermissions;
        vals=Object.keys(row).map(function(key,index) {
            //  console.log("row[key] "+ JSON.stringify(row[key].canRead="true"));
            return (<Row componentClass="tr">
                    <Col componentClass="td">{key}</Col>
                    <Col componentClass="td"><Checkbox checked={row[key].canRead} onChange={(event) => this.handleCheckboxChange(event)}/></Col>
                    <Col componentClass="td"><label >Add</label></Col>
                    <Col componentClass="td"><label>Edit</label></Col>
                     <Col componentClass="td"><label>Delete</label></Col>
                </Row>)
        });
    }
    render() {
        let row = null;
        let vals=null;
        row = this.state.userPortalPermissionModel.portal.modulePermissions;
        vals=Object.keys(row).map(function(key,index) {
          //  console.log("row[key] "+ JSON.stringify(row[key].canRead="true"));
            return (<Row componentClass="tr">
                    <Col componentClass="td">{key}</Col>
                    <Col componentClass="td"><Checkbox checked={row[key].canRead} onChange={(event) => this.handleCheckboxChange(event)}/></Col>
                    <Col componentClass="td"><label >Add</label></Col>
                    <Col componentClass="td"><label>Edit</label></Col>
                     <Col componentClass="td"><label>Delete</label></Col>
                </Row>)
        });
      
        return (
            <div>
        <div className="clearBoth modalTableContainer">
                    <Grid componentClass="table" class="user-permission-portal-table" >
                        <thead>
                            <Row componentClass="tr">
                                <Col componentClass="th"><label>Module</label></Col>
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

export default AddEditUserPortalPermissions