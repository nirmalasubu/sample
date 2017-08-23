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
class AddEditProductDestination extends React.Component {
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
                    <FormGroup controlId="FirstName">
                    <ControlLabel>First Name:</ControlLabel>
                    <FormControl type="text"  value=  ref="inputFirstName" placeholder="Enter email for valid user id" />
                    </FormGroup>
                   </Col>
                   <Col sm={4}>
                   <FormGroup controlId="LastName" >
                    <ControlLabel>Last Name:</ControlLabel>
                     <FormControl type="text"  value=  ref="inputLastName" placeholder="Enter email for valid user id" />
                    </FormGroup>
                      <FormGroup controlId="LastName" >
                    <ControlLabel>Last Name:</ControlLabel>
                     <FormControl type="text"  value=  ref="inputLastName" placeholder="Enter email for valid user id" />
                    </FormGroup>
                     </Col>
                         </Form >
                        </Row>
                         <Row>
                              <Col sm={2}>
                         <FormGroup controlId="ActiveStatus" >
                    <ControlLabel>Active Status:</ControlLabel>
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
                    <ControlLabel>Admin:</ControlLabel>
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
                    <ControlLabel>last login:</ControlLabel>
                    <FormControl type="text"  value={this.state.permission.createdDateTime}  />
                    </FormGroup>
                     </Col>
                        </Row>
                       </Grid>
            </div>
        )
    }
}

export default AddEditProductDestination