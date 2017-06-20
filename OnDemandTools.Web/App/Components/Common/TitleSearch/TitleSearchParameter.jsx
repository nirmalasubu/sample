import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
class PageHeader extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {

        var filterRows = [];

        if (this.props.parameters != null && this.props.parameters.length > 0) {
            var maxRowsToDisplay = 7;

            if (this.props.parameters.length < 7)
                maxRowsToDisplay = this.props.parameters.length;

            for (var i = 0; i < maxRowsToDisplay; i++) {
                var filter = this.props.parameters[i];
                filterRows.push(<Radio name={this.props.name}> {filter.name + " (" + filter.count + ")"} <br /></Radio>);
            }
        }

        return (
            <div>
                <ControlLabel>{this.props.name}</ControlLabel>
                <FormGroup>
                    {filterRows}
                </FormGroup>
            </div>
        )
    }
}

export default PageHeader