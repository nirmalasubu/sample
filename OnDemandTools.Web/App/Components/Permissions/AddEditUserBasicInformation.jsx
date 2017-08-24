import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';


@connect((store) => {
    return {

    };
})
/// <summary>
/// Sub component of product page to  add ,edit product destination details
/// </summary>
class AddEditUserBasicInformation extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            userBasicInfoModel:"",
            userBasicInfounmodifiedModel:"",
            componentJustMounted: true,
            isChecked: true
        });
    }
      
    componentWillMount() {
        this.setState({
            userBasicInfoModel: this.props.data
        });
        console.log(" componentWillMount userBasicInfoModel :"+JSON.stringify(this.state.userBasicInfoModel));
    }

    componentDidMount() {
        var model = this.state.userBasicInfoModel;
        if (this.state.userBasicInfoModel.id == null) {
            this.setState({ userBasicInfoModel: model, componentJustMounted: true });
        }
        console.log("componentDidMount userBasicInfoModel :"+JSON.stringify(this.state.userBasicInfoModel));
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        this.setState({
            userBasicInfoModel: nextProps.data
        }, function () {
            if (this.state.componentJustMounted) {
                this.setState({ componentJustMounted: false }, function () {
                    //this.validateForm();
                });
            }
        });
        console.log("componentWillReceiveProps userBasicInfoModel :"+JSON.stringify(this.state.userBasicInfoModel));
    }

    lastloginDisplay()
    {
        if (this.state.userBasicInfoModel.id != null)
        {
            return( <FormGroup controlId="lastlogin " >
                                <ControlLabel>last login</ControlLabel>
                                <FormControl type="text" />
                            </FormGroup>);
        }
    }

    activeDateDisplay()
    {
        if (this.state.userBasicInfoModel.id != null)
        {
            return(<FormGroup controlId="activeDate" >
                                    <ControlLabel>Active Date</ControlLabel>
                                    <FormControl type="text" />
                                </FormGroup>);
        }
    }
    activeStatusChange()
    {
        this.setState({
            isChecked: !this.state.isChecked // flip boolean value
        });
        
    }

    render() {
        return (
            <div>
                <Grid >
                    <Row>
                        <Form>
                            <Col sm={4}>
                                <FormGroup controlId="userId">
                                    <ControlLabel>User ID</ControlLabel>
                                    <FormControl type="text" value= {this.state.userBasicInfoModel.name} ref="inputUserId" placeholder="Enter email for valid user id" />
                                </FormGroup>
                            </Col>
                            <Col sm={4}>
                                {this.activeDateDisplay()}
                            </Col>
                        </Form >
                    </Row>
                    <Row>
                        <Col sm={2}>
                            <FormGroup controlId="activeStatus" >
                                <ControlLabel>Active Status</ControlLabel>
                                <div>
                                    <label class="switch">
                                        <input type="checkbox"  value= {this.state.userBasicInfoModel.isChecked}  onChange={(event) => this.activeStatusChange()} />
                                        <span class="slider round"></span>
                                    </label>
                                </div>
                            </FormGroup>
                        </Col>
                        <Col sm={2}>
                            <FormGroup controlId="isAdmin" >
                                <ControlLabel>Admin</ControlLabel>
                                <div>
                                    <label class="switch">
                                        <input type="checkbox"    onChange={(event) => this.isAdminChange()} />
                                        <span class="slider round"></span>
                                    </label>
                                </div>
                            </FormGroup>
                        </Col>
                        <Col sm={4}>
                            {this.lastloginDisplay()}
                           
                        </Col>
                    </Row>
                </Grid>
            </div>
        )
                                }
                                }

export default AddEditUserBasicInformation