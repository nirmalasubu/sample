import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button, Checkbox } from 'react-bootstrap';
import $ from 'jquery';
import Select from 'react-select';
import 'react-select/dist/react-select.css';

// Sub component used within product page to search the product, tags and description values
class SystemPermissionsFilter extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      includeInactive: false
    };

  }

  ///<summary>
  // React to filter control changes and bubble up
  // to parent component for further handling
  ///</summary>
  handleChange(val) {

    var sn = this.inputName.value;
    this.props.updateFilter(sn, "Name");

    var uid = this.inputSystemId.value;
    this.props.updateFilter(uid, "SystemId");

    this.props.updateFilter(this.state.includeInactive, "IncludeInactive");

  }

  ///<summary>
  // React to 'Clear All Filter' event and
  // bubble up to parent component for further handling
  ///</summary>
  clearFilter() {
    this.inputName.value = "";
    this.inputSystemId.value = "";

    this.setState({ includeInactive: false });

    this.props.updateFilter("", "Clear");
  }

  // React to checkbox 'Include Inactive' event and
  // bubble up to parent component for further handling
  handleCheckBoxChange(event) {
    var checked = event.target.checked;
    this.state.includeInactive = checked;
    this.handleChange();
  }

  render() {
    return (
      <div>
        <Form inline>
          <ControlLabel>Filter by: </ControlLabel>
          {' '}
          <FormGroup controlId="systemId">
            <FormControl type="text" inputRef={(input) => this.inputSystemId = input} onChange={this.handleChange.bind(this)} placeholder="System ID" />
          </FormGroup>
          {' '}
          <FormGroup controlId="name">
            <FormControl type="text" inputRef={(input) => this.inputName = input} onChange={this.handleChange.bind(this)} placeholder="System Contact" />
          </FormGroup>
          {' '}
          <FormGroup controlId="isActive">
            <Checkbox name="active" onChange={this.handleCheckBoxChange.bind(this)}
              checked={this.state.includeInactive}> Include Inactive</Checkbox>
          </FormGroup>
          {' '}
          <Button onClick={this.clearFilter.bind(this)} bsStyle="primary">
            Clear All Filters
        </Button>
        </Form>
      </div>
    )
  }
}

export default SystemPermissionsFilter;