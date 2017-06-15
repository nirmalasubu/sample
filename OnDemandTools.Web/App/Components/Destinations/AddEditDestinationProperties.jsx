import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover } from 'react-bootstrap';
import DestinationPropertiesFilter from 'Components/Destinations/DestinationPropertiesFilter';
import RemovePropertiesModal from 'Components/Destinations/RemovePropertiesModal';


class AddEditDestinationProperties extends React.Component {

  constructor(props) {
    super(props);
    this.state = ({
      destinationDetails: {},
      showAddEditPropertiesFilter: false,
      showPropertiesDeleteModal: false,
      destinationPropertiesRow: {},
      propertyIndexToRemove: -1
    });
  }

  componentDidMount() {
    this.setState({
      destinationDetails: this.props.data
    });
  }

  componentWillMount() {
    this.setState({
      destinationDetails: this.props.data
    });
  }

  openPropertiesFilter(row) {
    this.setState({ showAddEditPropertiesFilter: true, destinationPropertiesRow: row });
  }

  closePropertiesFilter() {
    this.setState({ showAddEditPropertiesFilter: false });
  }

  RemovePropertiesModel(value) {
    var array = [];
    array = this.state.destinationDetails;
    array.properties.splice(value, 1);
    this.setState({ destinationDetails: array });
    this.props.data = this.state.destinationDetails;
    this.setState({ showPropertiesDeleteModal: false });

  }
  AddNewProperty() {
    var newProperty = { name: "", value: "", brands: [], titleIds: [], seriesIds: [], titles: [] }
    var array = [];
    array = this.state.destinationDetails;
    array.properties.push(newProperty);
    array.properties.reverse();
    this.setState({ destinationDetails: array });
    this.props.data = this.state.destinationDetails;

  }

  openPropertiesDeleteModel(item) {
    this.setState({ showPropertiesDeleteModal: true, propertyIndexToRemove: item });
  }

  closePropertiesDeleteModel() {
    this.setState({ showPropertiesDeleteModal: false });
  }

  handlePropertyNameChange(event) {
    var model = this.state.destinationDetails;
    model.properties[event.target.id].name = event.target.value;
    this.setState({ destinationDetails: model });
    this.props.data = this.state.destinationDetails;

  }

  handlePropertyValueChange(event) {
    var model = this.state.destinationDetails;
    model.properties[event.target.id].value = event.target.value;
    this.setState({ destinationDetails: model });
    this.props.data = this.state.destinationDetails;

  }



  render() {
    let row = null;
    if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object) {
      if (Object.keys(this.state.destinationDetails.properties).length !== 0 && this.state.destinationDetails.properties != Object) {
        row = this.state.destinationDetails.properties.map(function (item, index) {
          return (<div class="destination-properties-RowMargin"><Row >
            <Col sm={3} > <input type="text" id={index} value={item.name} onChange={this.handlePropertyNameChange.bind(this)} /> </Col>
            <Col sm={3} ><input type="text" id={index} value={item.value} onChange={this.handlePropertyValueChange.bind(this)} /></Col>
            <Col sm={4} >
              {item.brands.map(function (name, index) {
                var path = "images/brands/" + name + ".gif"
                return (<img src={path} />);
              })}

              {item.titles.map(function (title, index) {
                return (<span>{title.name}</span>);
              })}
            </Col>
            <Col sm={2} >
              <button class="btn-link" title="Edit Filter" onClick={(event) => this.openPropertiesFilter(item, event)} >
                <i class="fa fa-pencil-square-o"></i>
              </button>
              <button class="btn-link" title="Delete Property" onClick={(event) => this.openPropertiesDeleteModel(index, event)} >
                <i class="fa fa-trash"></i></button>
            </Col>
          </Row></div>);
        }.bind(this));
      }
      else {
        row = <Row><Col sm={12}><p>No properties available</p></Col></Row>

      }
    }

    return (
      <div>
        <div>
          <button class="destination-properties-addnew" title="Add New" onClick={(event) => this.AddNewProperty(event)}>
            Add  New
                        </button>
        </div>
        <div >
          <Grid fluid={true}>
            <Row>
              <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
              <Col sm={3} ><label class="destination-properties-label">Value</label></Col>
              <Col sm={4} ><label class="destination-properties-label">Filter</label></Col>
              <Col sm={2} ><label class="destination-properties-label">Actions</label></Col>
            </Row>
            <div class="destination-height">
              {row}
            </div>
          </Grid>
        </div>
        <DestinationPropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} />
        <RemovePropertiesModal data={this.state} handleClose={this.closePropertiesDeleteModel.bind(this)} handleRemoveAndClose={this.RemovePropertiesModel.bind(this)} />
      </div>
    )
  }
}

export default AddEditDestinationProperties