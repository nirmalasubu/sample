import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';

class AddEditDestinationBasic extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {

  }

  handleTextChange(event) {
    var value = event.target.value;
  }

  handleCheckboxChange(event) {
    var checked = event.target.checked;

    var model = this.props.data;

    switch (event.target.name) {
      case "hd":
        model.content.highDefinition = checked;
        break;
      case "sd":
        model.content.standardDefinition = checked;
        break;
      case "cx":
        model.content.cx = checked;
        break;
      case "nonCx":
        model.content.nonCx = checked;
        break;
      case "auditDelivery":
        model.auditDelivery = checked;
        break;
      default:
        throw "Checkbox binding not found for control" + event.target.name;
    }


    this.props.data = model;
  }

  render() {
    return (
      <div>
        <Grid>
          <Form>
            <Row>
              <Col md={4} >
                <FormGroup
                  controlId="destinationCode">
                  <ControlLabel>Destination Code</ControlLabel>
                  <FormControl
                    type="text"
                    value={this.props.data.name}
                    ref="inputFriendlyName"
                    placeholder="Enter Destination Code"
                    onChange={this.handleTextChange.bind(this)}
                  />
                </FormGroup>
              </Col>
              <Col md={4}>
                <FormGroup
                  controlId="contactEmail">
                  <ControlLabel>Destination Description</ControlLabel>
                  <FormControl
                    type="text"
                    value={this.props.data.description}
                    placeholder="Enter Destination Description"
                    onChange={this.handleTextChange.bind(this)}
                  />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={4} >
                <FormGroup
                  controlId="contents">
                  <ControlLabel>Content</ControlLabel>
                  <Checkbox name="hd" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.props.data.content.highDefinition}>HD</Checkbox>
                  <Checkbox name="sd" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.props.data.content.standardDefinition}> SD </Checkbox>
                  <Checkbox name="cx" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.props.data.content.cx}>C(X)</Checkbox>
                  <Checkbox name="nonCx" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.props.data.content.nonCx}> Non-C(X)</Checkbox>
                </FormGroup>
              </Col>
              <Col md={4}>
                <FormGroup
                  controlId="options">
                  <ControlLabel>Options</ControlLabel>
                  <Checkbox name="auditDelivery" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.props.data.auditDelivery}> Audit Delivery</Checkbox>
                </FormGroup>
              </Col>
            </Row>
          </Form>
        </Grid>
      </div>
    )
  }
}

export default AddEditDestinationBasic