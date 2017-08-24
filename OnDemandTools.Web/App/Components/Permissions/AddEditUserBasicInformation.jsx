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
class AddEditUserBasicInformation extends React.Component {
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
                            <Col sm={4}>
                                <FormGroup controlId="UserId">
                                    <ControlLabel>User ID</ControlLabel>
                                    <FormControl type="text" ref="inputUserId" placeholder="Enter email for valid user id" />
                                </FormGroup>
                            </Col>
                            <Col sm={4}>
                                <FormGroup controlId="ActiveDate" >
                                    <ControlLabel>Active Date</ControlLabel>
                                    <FormControl type="text" />
                                </FormGroup>
                            </Col>
                        </Form >
                    </Row>
                    <Row>
                        <Col sm={2}>
                            <FormGroup controlId="ActiveStatus" >
                                <ControlLabel>Active Status</ControlLabel>
                                <div>
                                    <label class="switch">
                                        <input type="checkbox" />
                                        <span class="slider round"></span>
                                    </label>
                                </div>
                            </FormGroup>
                        </Col>
                        <Col sm={2}>
                            <FormGroup controlId="IsAdmin" >
                                <ControlLabel>Admin</ControlLabel>
                                <div>
                                    <label class="switch">
                                        <input type="checkbox" />
                                        <span class="slider round"></span>
                                    </label>
                                </div>
                            </FormGroup>
                        </Col>
                        <Col sm={4}>
                            <FormGroup controlId="lastlogin " >
                                <ControlLabel>last login</ControlLabel>
                                <FormControl type="text" />
                            </FormGroup>
                        </Col>
                    </Row>
                </Grid>
            </div>
        )
    }
}

export default AddEditUserBasicInformation