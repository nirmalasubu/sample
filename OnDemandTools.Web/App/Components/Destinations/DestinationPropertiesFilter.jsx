import React from 'react';
import ImageCheckBox from 'Components/Common/ImageCheckBox';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { connect } from 'react-redux';
import $ from 'jquery';

@connect((store) => {
  return {
    config: store.config
  };
})
class DestinationPropertiesForm extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      brandsSelection: {}
    }
  }
  componentDidMount() {

  }

  resetForm() {

    var brands = [];

    for (var i = 0; i < this.props.config.brands.length; i++) {
      var brandName = this.props.config.brands[i];
      var brandObject = {};
      brandObject.brandName = brandName;
      brandObject.selected = $.inArray(brandName, this.props.data.destinationPropertiesRow.brands) > -1;
      brands.push(brandObject);
    }   
    
    this.setState({ brandsSelection: brands });
  }

  handleBrandChange(brandName) {

    var brands = this.state.brandsSelection;

    for (var i = 0; i < brands.length; i++) {
      if (brands[i].brandName == brandName) {
          brands[i].selected = !brands[i].selected;
      }
    }

    this.setState({ brandsSelection: brands });
  }

  onClickSave()
  {
      var brands = this.state.brandsSelection
      var array = [];
      for (var i = 0; i < brands.length; i++) {
          if (brands[i].selected) {
              array.push(brands[i].brandName);
          }
      }
      this.props.data.destinationPropertiesRow.brands=[];
      this.props.data.destinationPropertiesRow.brands=array;

      this.props.handleClose();
  }

  render() {

    var rows = [];
    for (var i = 0; i < this.state.brandsSelection.length; i++) {
        var brand = this.state.brandsSelection[i];
        var nextRow = (((i+1)%3)==0)?<br/>:"";
        rows.push(<ImageCheckBox brandName={brand.brandName} selected={brand.selected} handleBrandChange={this.handleBrandChange.bind(this)} /> );
        rows.push(nextRow);
    }

    return (
      <Modal className="destinationPropertiesFilterModel" bsSize="large"
        backdrop="static"
        onEntering={this.resetForm.bind(this)}
        show={this.props.data.showAddEditPropertiesFilter}
        onHide={this.props.handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>
            <div>Filters</div>
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <div class="propFilterContainer">
            <ControlLabel> Brands</ControlLabel><br />
            {rows}
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.props.handleClose}>Close</Button>
          <Button className="btn btn-primary btn-large" onClick={this.onClickSave.bind(this)}>Save</Button>
        </Modal.Footer>
      </Modal>
    )
  }
}

export default DestinationPropertiesForm