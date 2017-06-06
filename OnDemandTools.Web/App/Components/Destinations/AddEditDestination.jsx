import React from 'react';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Tabs, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import AddEditDestinationBasic from 'Components/Destinations/AddEditDestinationBasic'
import AddEditDestinationDeliverables from 'Components/Destinations/AddEditDestinationDeliverables'
import AddEditDestinationProperties from 'Components/Destinations/AddEditDestinationProperties'

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
                    <AddEditDestinationBasic data={this.props.data.destinationDetails} />
                    <Tabs defaultActiveKey={1} >
                        <Tab eventKey={1} title="Properties">
                            <AddEditDestinationProperties data={this.props.data.destinationDetails} />
                        </Tab>
                        <Tab eventKey={2} title="Deliverables">
                            <AddEditDestinationDeliverables data={this.props.data.destinationDetails} />
                        </Tab>
                    </Tabs>
                    <NotificationContainer />
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.props.handleClose}>Close</Button> <Button onClick={this.props.handleClose}>Save</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default AddEditDestinationModel;