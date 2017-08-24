import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';


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

        });
    }


    render() {

        return (
            <div>
                <Grid >
                    <Row>
                        <Form>
                            <Col sm={3}>
                                <FormGroup controlId="FirstName">
                                    <ControlLabel>First Name</ControlLabel>
                                    <FormControl type="text" ref="inputFirstName" placeholder="First Name" />
                                </FormGroup>
                            </Col>
                            <Col sm={3}>
                                <FormGroup controlId="LastName" >
                                    <ControlLabel>Last Name</ControlLabel>
                                    <FormControl type="text" ref="inputLastName" placeholder="Last Name" />
                                </FormGroup>
                            </Col>
                            <Col sm={2}>
                                <FormGroup controlId="Phone Number" >
                                    <ControlLabel>Phone Number</ControlLabel>
                                    <FormControl type="text" ref="inputPhoneNumber" placeholder="Phone Number" />
                                </FormGroup>
                            </Col>
                        </Form >
                    </Row>
                    <Row>
                        <Form>
                            <Col sm={3}>
                                <FormGroup controlId="User API Key" >
                                    <ControlLabel>User API Key</ControlLabel>
                                    <FormControl type="text" ref="inputUserAPIKey" placeholder="User API Key" />
                                </FormGroup>
                            </Col>
                            <Col sm={3}>
                                <FormGroup controlId="ActiveAPI" >
                                    <ControlLabel>Active API:</ControlLabel>
                                    <div>
                                        <label class="switch">
                                            <input type="checkbox" />
                                            <span class="slider round"></span>
                                        </label>
                                    </div>
                                </FormGroup>
                            </Col>
                            <Col sm={2}>
                                <FormGroup controlId="API last Accessed" >
                                    <ControlLabel>API last Accessed</ControlLabel>
                                    <FormControl type="text" ref="inputAPIlastAccessed" placeholder="API last Accessed" />
                                </FormGroup>
                            </Col>
                        </Form >
                    </Row>
                    <Row>
                        <Col sm={3}>
                            <FormGroup controlId="Contact for" >
                                <ControlLabel>Contact for</ControlLabel>
                                <FormControl type="text" ref="inputContactfor" placeholder="Contact for" />
                            </FormGroup>
                        </Col>
                    </Row>
                </Grid>
            </div>
        )
    }
}

export default AddEditUserPersonalInformation