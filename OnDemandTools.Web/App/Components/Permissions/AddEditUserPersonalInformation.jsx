import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import ReactPhoneInput from 'react-phone-number-input';
import rrui from 'react-phone-number-input/rrui.css'

@connect((store) => {
    return {

    };
})
/// <summary>
/// Sub component of product page to  add ,edit product destination details
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
            phone:""
        });
    }
    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        this.setState({
            personalInfoModel: nextProps.data
        }, function () {
            if (this.state.componentJustMounted) {
                this.setState({ componentJustMounted: false }, function () {
                });
            }
        });
        console.log("personalInfoModel"+JSON.stringify(this.state.personalInfoModel));

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
       
        console.log("componentDidMount personalInfoModel :"+JSON.stringify(this.state.personalInfoModel));
    }

    apiLastAccessedDisplay()
    {
        if (this.state.personalInfoModel.id != null)
        {
            return(  <FormGroup controlId="api last Accessed" >
                                    <ControlLabel>API last Accessed</ControlLabel>
                                    <FormControl type="text" ref="inputAPIlastAccessed" placeholder="API last Accessed" />
                                </FormGroup>);
        }
    }

    contactforDisplay()
    {
        if (this.state.personalInfoModel.id != null)
        {
            return(<FormGroup controlId="contactfor" >
                                <ControlLabel>Contact for</ControlLabel>
                                <FormControl type="text" ref="inputContactfor" placeholder="Contact for" />
                            </FormGroup>);
        }
    }

    /// <summary>
    /// Updating the user/description/name in the state on change of text
    /// </summary>
    handleTextChange(value,event) {
     
        var model = this.state.personalInfoModel;
        if(value=="firstName")
            model.firstName = event.target.value.trimLeft();;
        if(value=="lastName")
            model.lastName = event.target.value.trimLeft();;
        if(value=="phoneNumber")
            model.phoneNumber = event.target.value();

        this.setState({
            personalInfoModel: model
        });
        this.props.updatePermission(model);
    }


    activeApiChange()
    {
        var model = this.state.personalInfoModel;
        model.api.isActive = !this.state.personalInfoModel.api.isActive;

        this.setState({
            personalInfoModel: model
        });
        this.props.updatePermission(model);
    }


    render() {
        
        return (
            <div>
                <Grid >
                    <Row>
                        <Form>
                            <Col sm={3}>
                                <FormGroup controlId="firstName">
                                    <ControlLabel>First Name</ControlLabel>
                                    <FormControl type="text" ref="inputFirstName" placeholder="First Name"  value={this.state.personalInfoModel.firstName}
                                     onChange={(event) =>this.handleTextChange("firstName", event)}/>
                                </FormGroup>
                            </Col>
                            <Col sm={3}>
                                <FormGroup controlId="lastName" >
                                    <ControlLabel>Last Name</ControlLabel>
                                    <FormControl type="text" ref="inputLastName" placeholder="Last Name"  value={this.state.personalInfoModel.lastName}
                                     onChange={(event) =>this.handleTextChange("lastName", event)}/>
                                </FormGroup>
                            </Col>
                            <Col sm={2}>
                                <FormGroup controlId="phoneNumber" >

                                  
                                
                                    </FormGroup>
                                </Col>
                            </Form >
                        </Row>
                        <Row>
                            <Form>
                                <Col sm={3}>
                                    <FormGroup controlId="userAPIKey" >
                                        <ControlLabel>User API Key</ControlLabel>
                                        <FormControl type="text" ref="inputUserAPIKey" placeholder="User API Key" />
                                    </FormGroup>
                                </Col>
                                <Col sm={3}>
                                    <FormGroup controlId="activeAPI" >
                                        <ControlLabel>Active API:</ControlLabel>
                                        <div>
                                            <label class="switch">
                                                <input type="checkbox" checked= {this.state.personalInfoModel.api.isActive}  onChange={(event) => this.activeApiChange()} />
                                                <span class="slider round"></span>
                                            </label>
                                        </div>
                                    </FormGroup>
                                </Col>
                                <Col sm={2}>
                                                {this.apiLastAccessedDisplay()}
                                </Col>
                            </Form >
                        </Row>
                        <Row>
                            <Col sm={3}>
                                                {this.contactforDisplay()}
                            </Col>
                        </Row>
                    </Grid>
                </div>
            )
                            }
                                            }

export default AddEditUserPersonalInformation