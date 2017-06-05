import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';

class DestinationPropertiesForm extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {

  }

  handleTextChange(event) {
    var value = event.target.value;
  }
  
  render() {
    return (
      <div>
        <Grid>
          <Form>
            <Row>
              <Col md={2} >
                <FormGroup
                  controlId="name">
                  <ControlLabel>Name</ControlLabel>
                  <FormControl
                    type="text"
                    value={this.props.data.name}
                    placeholder="Enter Property Name"
                    onChange={this.handleTextChange.bind(this)}
                  />
                </FormGroup>
              </Col>
              <Col md={2}>
                <FormGroup
                  controlId="value">
                  <ControlLabel>Value</ControlLabel>
                  <FormControl
                    type="text"
                    value={this.props.data.value}
                    placeholder="Enter Property Value"
                    onChange={this.handleTextChange.bind(this)}
                  />
                </FormGroup>
              </Col>
            </Row>            
          </Form>
        </Grid>
      </div>
    )
  }
}

export default DestinationPropertiesForm