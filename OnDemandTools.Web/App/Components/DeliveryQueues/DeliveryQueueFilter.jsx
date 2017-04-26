import React from 'react';
import { Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
class DeliveryQueueFilter extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <Form inline>
                    <ControlLabel>Filter</ControlLabel>
                    {' '}
                    <FormGroup controlId="queueName">
                        <FormControl type="text" placeholder="Queue Name" />
                    </FormGroup>
                    {' '}
                    <FormGroup controlId="contactName">
                        <FormControl type="text" placeholder="Contact Name" />
                    </FormGroup>
                    {' '}
                    <FormGroup controlId="queueId">
                        <FormControl type="text" placeholder="Queue ID" />
                    </FormGroup>
                    {' '}
                    <Button bsStyle="primary">
                        Clear All Filters
    </Button>
                </Form>
            </div>
        )
    }
}

export default DeliveryQueueFilter