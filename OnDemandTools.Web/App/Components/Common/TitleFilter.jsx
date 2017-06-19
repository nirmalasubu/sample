import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { titleSearch } from 'Actions/TitleSearch/TitleSearchActions';
import { connect } from 'react-redux';

@connect((store) => {
    return {
        titleSearchResults: store.titleSearch
    };
})
class TitleFilter extends React.Component {
    constructor(props) {
        super(props);

    }

    handleChange(val) {
        var searchParam = this.inputTitleSearch.value;
        if (searchParam.length > 2)
            this.props.dispatch(titleSearch(searchParam));

    }

    render() {

        var titleTypeRows = [];

        if (this.props.titleSearchResults.titleTypeFilterParameters != null) {

            var maxRowsToDisplay = 7;

            if (this.props.titleSearchResults.titleTypeFilterParameters.length < 7)
                maxRowsToDisplay = this.props.titleSearchResults.titleTypeFilterParameters.length;

            for (var i = 0; i < maxRowsToDisplay; i++) {
                var filter = this.props.titleSearchResults.titleTypeFilterParameters[i];
                titleTypeRows.push(<span> {filter.name + " (" + filter.count + ")"} <br /></span>);
            }
        }

        var seriesRows = [];

        if (this.props.titleSearchResults.seriesFilterParameters != null) {
            var maxRowsToDisplay = 7;

            if (this.props.titleSearchResults.seriesFilterParameters.length < 7)
                maxRowsToDisplay = this.props.titleSearchResults.seriesFilterParameters.length;

            for (var i = 0; i < maxRowsToDisplay; i++) {
                var filter = this.props.titleSearchResults.seriesFilterParameters[i];
                seriesRows.push(<span> {filter.name + " (" + filter.count + ")"}<br /></span>);
            }
        }

        return (
            <div>
                <FormGroup controlId="code">
                    <FormControl type="text" inputRef={(input) => this.inputTitleSearch = input}
                        placeholder="Search..."
                        onChange={this.handleChange.bind(this)}
                    />
                </FormGroup>
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
                    <Row>
                        <Col md={2}>
                            <ControlLabel>Title Type</ControlLabel><br />
                            {titleTypeRows}
                            <ControlLabel>Series</ControlLabel><br />
                            {seriesRows}
                        </Col>
                        <Col md={4}>
                        </Col>

                        <Col md={4}>
                        </Col>
                    </Row>
                </Grid>
            </div>
        )
    }
}

export default TitleFilter