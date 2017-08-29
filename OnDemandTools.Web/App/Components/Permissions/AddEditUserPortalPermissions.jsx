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
        this.activeStatusChange=this.activeStatusChange.bind(this);
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

    activeStatusChange()
    {
        console.log("sdafsda");
            
    }

    rowConstruct(){

       
    }
    activechkChange(key,value,event)
    {
        var model=this.state.userPortalPermissionModel;
            
        model.portal.modulePermissions[key][value]=!this.state.userPortalPermissionModel.portal.modulePermissions[key][value];
        
        this.setState({
            userPortalPermissionModel: model
        });
        this.props.updatePermission(model);
    }
    render() {
        let row = null;
        let vals=null;
        row = this.state.userPortalPermissionModel.portal.modulePermissions;
        vals=Object.keys(row).map(function(key,index) {
          
            return (<Row componentClass="tr" key={index.toString()}>
    
                    <Col componentClass="td">{key}</Col>
                    <Col componentClass="td">
                         <input type="checkbox"  checked={row[key].canRead} onChange={(event) => this.activechkChange(key, "canRead", event)} />
                                     </Col>
                    <Col componentClass="td"> <input type="checkbox"  checked={row[key].canAdd} onChange={(event) => this.activechkChange(key, "canAdd", event)} /></Col>
                    <Col componentClass="td"><input type="checkbox"  checked={row[key].canEdit} onChange={(event) => this.activechkChange(key, "canEdit", event)} /></Col>
                     <Col componentClass="td"><input type="checkbox"  checked={row[key].canDelete} onChange={(event) => this.activechkChange(key, "canDelete", event)} /></Col>
           
                </Row>)
        }.bind(this));
      
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