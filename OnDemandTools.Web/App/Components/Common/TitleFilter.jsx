import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
class TitleFilter extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <Grid>
                    <Row>
                        <Col md={2}>
                            <ControlLabel>Filter</ControlLabel>
                        </Col>
                        <Col md={4}>
                            <ControlLabel>Available Title/Series</ControlLabel>
                        </Col><Col md={4}>
                            <ControlLabel>Selected Title/Series</ControlLabel>
                        </Col>
                    </Row>
                </Grid>
            </div>
        )
    }
}

export default TitleFilter