import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';


class AddEditDestinationBasic extends React.Component {

    constructor(props) {
        super(props);

        this.state = ({
            destinationModel: {},
            validationStateName: "",
            validationStateDescription: "",
            validationStateExternalId: ""
        });
    }

    validateForm() {
        var name = this.state.destinationModel.name;
        var description = this.state.destinationModel.description;
        var hasError = false;

        this.setState({
            validationStateName: (name!=undefined && (name == "" || name.length < 3 || name.length > 5)) ? 'error' : '',
            validationStateDescription: (description == "") ? 'error' : ''
        });

        this.props.validationStates(name, description);
    }

    componentWillMount() {
        this.setState({
            destinationModel:this.props.data
        });        
    }

    componentDidMount() {
        var model = this.state.destinationModel;
        if(this.state.destinationModel.id==null)
        {
            model.externalId = this.getUnusedExternalId();

            this.setState({ destinationModel : model });
        }

        this.validateForm();
    }

    getUnusedExternalId() {
        var externalId = 1;

        while (true) {

            if (!this.isExternaldUsed(externalId)) {
                break;
            }

            externalId++;
        }

        return externalId;
    }

    isExternaldUsed(externalId) {
        for (var x = 0; x < this.props.destinations.length; x++) {
            if (externalId == this.props.destinations[x].externalId) {
                return true;
            }
        }

        return false;
    }

    handleTextChange(event) {       

        var model = this.state.destinationModel;
                
        model.name = event.target.value.toUpperCase();

        this.setState({
            destinationModel:model
        });

        this.validateForm();

        this.props.data = this.state.destinationModel;
    }

    handleDescriptionChange(event){
        var model = this.state.destinationModel;
        model.description = event.target.value;

        this.setState({
            destinationModel:model
        });

        this.validateForm();

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

        this.validateForm();

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
                  controlId="destinationCode" validationState={this.state.validationStateName}>
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
                  controlId="destinationDescription" validationState={this.state.validationStateDescription}>
                    <ControlLabel>Destination Description</ControlLabel>
                    <FormControl bsClass="form-control form-control-modal" componentClass="textarea" value={this.state.destinationModel.description} placeholder="Results"
                    onChange={this.handleDescriptionChange.bind(this)}  />
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