import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { connect } from 'react-redux';

@connect((store) => {
    return {
        config: store.config
    };
})
class DestinationPropertiesForm extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {

  }

  resetForm() {
    
  }

  render() {

    var rows = [];
    for (var i = 0; i < this.props.config.brands.length; i++) {
        rows.push(<img class="brandImage" alt={this.props.config.brands[i]} src={"../images/brands/" + this.props.config.brands[i] + ".gif"} />);
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
           <ControlLabel> Brands</ControlLabel>
           {rows}
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.props.handleClose}>Close</Button>
          <Button className="btn btn-primary btn-large">Save</Button>
        </Modal.Footer>
      </Modal>
    )
  }
}

export default DestinationPropertiesForm