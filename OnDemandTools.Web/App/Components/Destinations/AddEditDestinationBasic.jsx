import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';

@connect((store) => {
    return {
        config: store.config,
        destinations:store.destinations
    };
})
class AddEditDestinationBasic extends React.Component {

    constructor(props) {
        super(props);

        this.state = ({
            destinationModel: {}
        });
    }

    componentWillMount() {
        this.setState({
            destinationModel:this.props.data
        });
    }

    getUnusedExternalId() {
        var externalId = 1;

        while (true) {

            if (!isExternaldUsed(externalId)) {
                break;
            }

            externalId++;
        }

        return externalId;
    }

    isExternaldUsed(externalId) {
        for (var x = 0; x < this.props.destinations.length; x++) {
            if (externalId == destinations[x].externalId) {
                return true;
            }
        }

        return false;
    }

    handleTextChange(event) {
        var model = this.state.destinationModel;
        model.name = event.target.value;

        this.setState({
            destinationModel:model
        });

        this.props.data = this.state.destinationModel;
    }

    handleDescriptionChange(event){
        var model = this.state.destinationModel;
        model.description = event.target.value;

        this.setState({
            destinationModel:model
        });

        this.props.data = this.state.destinationModel;
    }

    handleCheckboxChange(event) {
        var checked = event.target.checked;

        var model = this.state.destinationModel;

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

        this.setState({
            destinationModel:model
        });

        this.props.data = this.state.destinationModel;
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
                    value={this.state.destinationModel.name}
                    ref="inputFriendlyName"
                    placeholder="Enter Destination Code"
                    onChange={this.handleTextChange.bind(this)}
                  />
                    </FormGroup>
                  </Col>
                  <Col md={4}>

                  </Col>
                </Row>
                <Row>
                  <Col md={4} >
                    <FormGroup
                    controlId="destinationDescription">
                    <ControlLabel>Destination Description</ControlLabel>
                    <FormControl bsClass="form-control form-control-modal" componentClass="textarea" value={this.state.destinationModel.description} placeholder="Results"
                    onChange={this.handleDescriptionChange.bind(this)} />
                  </FormGroup>
                </Col>
                <Col md={4}>
                  <ControlLabel>Content</ControlLabel>
                  <Form inline>
                    <FormGroup
                    controlId="contents">
                    <Checkbox name="hd" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.state.destinationModel.content.highDefinition}>HD</Checkbox>
                <Checkbox name="sd" className="marginLeftRight" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.state.destinationModel.content.standardDefinition}> SD </Checkbox>
                <Checkbox name="cx" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.state.destinationModel.content.cx}>C(X)</Checkbox>
                <Checkbox name="nonCx" className="marginLeftRight" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.state.destinationModel.content.nonCx}> Non-C(X)</Checkbox>
              </FormGroup>
            </Form>
             <ControlLabel>Options</ControlLabel>
            <FormGroup
                    controlId="options">                 
                    <Checkbox name="auditDelivery" onChange={this.handleCheckboxChange.bind(this)}
                    checked={this.state.destinationModel.auditDelivery}> Audit Delivery</Checkbox>
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