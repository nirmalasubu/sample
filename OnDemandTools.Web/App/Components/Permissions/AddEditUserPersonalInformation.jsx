import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import Phone,{parsePhoneNumber,
    isValidPhoneNumber} from 'react-phone-number-input'
import rrui from 'react-phone-number-input/rrui.css'
import rpni from 'react-phone-number-input/style.css'
import Moment from 'moment';
import validator from 'validator';
import {fetchContactForRecords} from 'Actions/Permissions/PermissionActions';

@connect((store) => {
    return {

    };
})
/// <summary>
/// Sub component of user permission page to  add ,edit personal info details
/// </summary>
class AddEditUserPersonalInformation extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            personalInfoModel: "",
            componentJustMounted: true,
            phone: "",
            validationStateFirstName: null,
            validationStateLastName: null,
            validationStatePhoneNumber: null,
            validationStateExtension: null,
            contactFor:null
        });
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        this.setState({
            personalInfoModel: nextProps.data
        }, function () {
            if (this.state.componentJustMounted) {
                this.setState({ componentJustMounted: false }, function () {
                    this.validateForm();
                });
            }
        });


    }

    componentWillMount() {
        this.setState({
            personalInfoModel: this.props.data
        });
    }

    componentDidMount() {
        var model = this.state.personalInfoModel;
        if (this.state.personalInfoModel.id == null) {
            this.setState({ personalInfoModel: model, componentJustMounted: true });
        }
        else{
           
            let promise=fetchContactForRecords(this.state.personalInfoModel.id);
            promise.then(response => {
                this.setState({
                    contactFor: response
                });
            }).catch(error => {
                this.setState({
                    contactFor: {}
                });
            })
            
        }
        this.validateForm();
    }


    formatDate(val) {
        if (val == null) {
            return "never"
        }
        else {

            var d = new Date(val);
            var year = d.getFullYear();
            if (year < 2000) {
                return "never";
            }
            else {

                var dateFormat = Moment(val).format('lll');
                return Moment(val).format('lll');
            }
        }
    }
    /// <summary>
    /// to hide and show api last Accessed field
    /// </summary>
    apiLastAccessedDisplay() {
        if (this.state.personalInfoModel.id != null) {
            return (<FormGroup controlId="api last Accessed" >
                <ControlLabel>API last Accessed</ControlLabel>
                <FormControl type="text" ref="inputAPIlastAccessed"  defaultValue={ this.formatDate(this.state.personalInfoModel.api.lastAccessTime)} placeholder="API last Accessed" />
            </FormGroup>);
        }
    }

    /// <summary>
    /// to hide and show  contact for field
    /// </summary>
    contactforDisplay() {
        if (this.state.personalInfoModel.id != null) {
          
            if(this.state.contactFor!=null)
            {
               
                if(this.state.contactFor.technicalContactFor!="" && this.state.contactFor.functionalContactFor!="")
                {
                   
                    return (<Row><Col sm={3}><FormGroup controlId="technicaContactfor" >
                 <ControlLabel> Technical Contact for</ControlLabel>
              <FormControl type="text" ref="inputContactfor"  value={this.state.contactFor.technicalContactFor} placeholder="Contact for" />
                  </FormGroup></Col>
                  <Col sm={3}><FormGroup controlId="functionalContactfor" >
                 <ControlLabel> Functional Contact for</ControlLabel>
              <FormControl type="text" ref="inputContactfor"  value={this.state.contactFor.functionalContactFor} placeholder="Contact for" />
                  </FormGroup></Col></Row>
                  );
                         }

        if(this.state.contactFor.technicalContactFor!="")
            {
             
                return (<Row><Col sm={3}><FormGroup controlId="technicaContactfor" >
              <ControlLabel> Technical Contact for</ControlLabel>
              <FormControl type="text" ref="inputContactfor" value={this.state.contactFor.technicalContactFor} placeholder="Contact for" />
          </FormGroup></Col></Row>);
              }

              if(this.state.contactFor.functionalContactFor!="")
            {
                  return (<Row><Col sm={3}><FormGroup controlId="functionalContactfor" >
                         <ControlLabel> Functional Contact for</ControlLabel>
                     <FormControl type="text" ref="inputContactfor" placeholder="Contact for" />
                     </FormGroup></Col></Row>);
            }
            }
          
        }
    }

    /// <summary>
    /// Updating the first/last/name in the state on change of text
    /// </summary>
    handleTextChange(value, event) {

        var model = this.state.personalInfoModel;
      
        if (value == "firstName")
            model.firstName = event.target.value.trimLeft();
        if (value == "lastName")
            model.lastName = event.target.value.trimLeft();
        if (value == "phoneNumber")
            model.phoneNumber = event;
        if (value == "extension")
            model.extension = event.target.value;
          
      
        this.setState({
            personalInfoModel: model
        });
        this.validateForm();
        this.props.updatePermission(model);
    }


    /// <summary>
    /// updating active api on checkbox change
    /// </summary>
    activeApiChange() {
        var model = this.state.personalInfoModel;
        model.api.isActive = !this.state.personalInfoModel.api.isActive;

        this.setState({
            personalInfoModel: model
        });
        this.props.updatePermission(model);
    }

    /// <summary>
    /// validate the form
    /// </summary>
    validateForm() {

        var isvalidFirstName = (this.state.personalInfoModel.firstName != undefined) ?
            (this.state.personalInfoModel.firstName != "" && validator.isAlpha(this.state.personalInfoModel.firstName)) : false;
        var isvalidLastName = (this.state.personalInfoModel.lastName != undefined) ?
            (this.state.personalInfoModel.lastName != "" && validator.isAlpha(this.state.personalInfoModel.lastName)) : false;
        var isvalidPhoneNumber=this.state.personalInfoModel.phoneNumber==""||isValidPhoneNumber(this.state.personalInfoModel.phoneNumber)?true : false;
       
        var extn=/^(?:\d{1}|\d{2}|\d{3}|\d{4}|\d{5})$/;
        var isvalidextension=this.state.personalInfoModel.extension==""||this.state.personalInfoModel.extension.match(extn)!=null?true : false;

        var phoneAndExtensionvalidation=(this.state.personalInfoModel.extension!="" && this.state.personalInfoModel.phoneNumber!="")|| this.state.personalInfoModel.extension==""? true:false;
        this.setState({
            validationStateFirstName: isvalidFirstName ? null : 'error',
            validationStateLastName: isvalidLastName ? null : 'error',
            validationStatePhoneNumber: isvalidPhoneNumber && (phoneAndExtensionvalidation) ? null : 'error',
            validationStateExtension: isvalidextension ? null : 'error'
        });

        this.props.validationStates(isvalidFirstName, isvalidLastName,isvalidPhoneNumber && (phoneAndExtensionvalidation),isvalidextension);
    }


    render() {

        return (
            <div>
                <Grid >
                    <Row class="user-permission-personalinfo-row">
                        <Form>
                            <Col sm={3}>
                                <FormGroup controlId="firstName" validationState={this.state.validationStateFirstName}>
                                    <ControlLabel>First Name</ControlLabel>
                                    <FormControl type="text" ref="inputFirstName" placeholder="First Name" value={this.state.personalInfoModel.firstName}
                                        onChange={(event) => this.handleTextChange("firstName", event)} />
                                </FormGroup>
                            </Col>
                            <Col sm={3}>
                                <FormGroup controlId="lastName" validationState={this.state.validationStateLastName} >
                                    <ControlLabel>Last Name</ControlLabel>
                                    <FormControl type="text" ref="inputLastName" placeholder="Last Name" value={this.state.personalInfoModel.lastName}
                                        onChange={(event) => this.handleTextChange("lastName", event)} />
                                </FormGroup>
                            </Col>
                            <Col sm={3}>
                                <FormGroup controlId="phoneNumber"  validationState={this.state.validationStatePhoneNumber} >
                                    <ControlLabel>Phone Number </ControlLabel>
                                    
                                   <Phone class="user-permission-personalinfo-phone"
                                        placeholder="Enter phone number" country="US"
                                        value={this.state.personalInfoModel.phoneNumber}
                                        onChange={(event) => this.handleTextChange("phoneNumber", event)} />
                                
                                </FormGroup>
                            </Col>
                               <Col sm={1}>
                                                                <FormGroup controlId="Extension" class="user-permission-formgroup" validationState={this.state.validationStateExtension} >
                                    <ControlLabel>Extension </ControlLabel>
                                  <FormControl type="text" class="user-permission-personalinfo-extension" ref="inputExtension" placeholder="XXXXX" value={this.state.personalInfoModel.extension} 
                                    onChange={(event) => this.handleTextChange("extension", event)}/>
                                </FormGroup>
                            </Col>
                        </Form >
                    </Row>
                    <Row>
                        <Form>
                            <Col sm={3}>
                                <FormGroup controlId="userAPIKey" >
                                    <ControlLabel>User API Key</ControlLabel>
                                    <FormControl type="text" ref="inputUserAPIKey" placeholder="User API Key Automatically generated" value={this.state.personalInfoModel.api.apiKey} disabled="true" />
                                </FormGroup>
                            </Col>
                            <Col sm={2}>
                                <FormGroup controlId="activeAPI" >
                                    <ControlLabel>Active API</ControlLabel>
                                    <div>
                                        <label class="switch">
                                            <input type="checkbox" checked={this.state.personalInfoModel.api.isActive} onChange={(event) => this.activeApiChange()} />
                                            <span class="slider round"></span>
                                        </label>
                                    </div>
                                </FormGroup>
                            </Col>
                            <Col sm={3}>
                                {this.apiLastAccessedDisplay()}
                            </Col>
                        </Form >
                    </Row>
                   
                        
                            {this.contactforDisplay()}
                       
                    
                </Grid>
            </div>
        )
    }
}

export default AddEditUserPersonalInformation