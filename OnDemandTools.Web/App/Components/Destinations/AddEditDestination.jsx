import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {NotificationContainer, NotificationManager} from 'react-notifications';

@connect((store) => {
    return {
        config: store.config
    };
})
class AddEditDestinationModel extends React.Component {

    constructor(props) {
        super(props);
    }
    //called on the model load
    onOpenModel(destination) {
        
    }
    render() {
        return (
            <Modal bsSize="large" backdrop="static" onEntering={this.onOpenModel.bind(this, this.props.data.destinationDetails)} show={this.props.data.showAddEditModel} onHide={this.props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <p>Edit Destination {this.props.data.destinationDetails.name}</p>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                   <p>Testing</p>
                    <NotificationContainer/>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Close</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default AddEditDestinationModel;