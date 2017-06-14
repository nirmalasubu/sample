import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover } from 'react-bootstrap';
import DestinationPropertiesFilter from 'Components/Destinations/DestinationPropertiesFilter';



class AddEditDestinationProperties extends React.Component {

  constructor(props) {
    super(props);
    this.state = ({
      destinationProperties: {},
      showAddEditPropertiesFilter: false,
      destinationPropertiesRow: {}
    });
  }

  componentDidMount() {
    this.setState({
      destinationProperties: this.props.data
    });
  }

  openPropertiesFilter(row) {
    this.setState({ showAddEditPropertiesFilter: true, destinationPropertiesRow: row });
  }

  closePropertiesFilter() {
    this.setState({ showAddEditPropertiesFilter: false });    
  }


  handlePropertyNameChange(event) {
    var model = this.state.destinationProperties;
    model.properties[event.target.id].name = event.target.value;
    this.setState({ destinationProperties: model });
    this.props.data = this.state.destinationProperties;

  }

  handlePropertyValueChange(event) {
    var model = this.state.destinationProperties;
    model.properties[event.target.id].value = event.target.value;
    this.setState({ destinationProperties: model });
    this.props.data = this.state.destinationProperties;

  }

  render() {
    let row = null;
    if (Object.keys(this.state.destinationProperties).length != 0 && this.state.destinationProperties != Object) {

      row = this.state.destinationProperties.properties.map(function (item, index) {

        return (<div class="destination-properties-RowMargin"><Row >
          <Col sm={3} > <input type="text" id={index} defaultValue={item.name} onChange={this.handlePropertyNameChange.bind(this)} /> </Col>
          <Col sm={3} ><input type="text" id={index} defaultValue={item.value} onChange={this.handlePropertyValueChange.bind(this)} /></Col>
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
            <button class="btn-link" title="Edit Filter" onClick={(event) => this.openPropertiesFilter(row, event)} >
              <i class="fa fa-pencil-square-o"></i>
            </button>
            <button class="btn-link" title="Delete Property" >
              <i class="fa fa-trash"></i></button>
          </Col>
        </Row></div>);
      }.bind(this));
    }

    return (
      <div>
        <div>
          <button class="destination-properties-addnew" title="Add New" onClick={(event) => this.openCreateNewQueueModel(event)}>
            Add  New
                        </button>
        </div>
        <Grid fluid={true}>
          <Row>
            <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
            <Col sm={3} ><label class="destination-properties-label">Value</label></Col>
            <Col sm={4} ><label class="destination-properties-label">Filter</label></Col>
            <Col sm={2} ><label class="destination-properties-label">Actions</label></Col>
          </Row>
          {row}
        </Grid>

        <DestinationPropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} />
      </div>
    )
  }
}

export default AddEditDestinationProperties