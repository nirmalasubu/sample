import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { titleSearch } from 'Actions/TitleSearch/TitleSearchActions';
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

@connect((store) => {
    return {
        titleSearchResults: store.titleSearch
    };
})
class TitleFilter extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            processing: false
        }

    }

    handleChange(val) {
        var searchParam = this.inputTitleSearch.value;

        this.setState({ processing: true });

        this.searchPromise = this.props.dispatch(titleSearch(searchParam));

        this.searchPromise.then(message => {
            this.setState({ processing: false });
        })
        this.searchPromise.catch(error => {
            this.setState({ processing: false });
        });
    }

    titleIdFormat(val, row) {
        return <p> {val} </p>
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
                <Form inline>
                    <FormGroup controlId="code">
                        <FormControl type="text" inputRef={(input) => this.inputTitleSearch = input}
                            placeholder="Search..." />
                        {' '}
                        <Button onClick={this.handleChange.bind(this)} bsStyle="primary" disabled={this.state.processing} >
                            {this.state.processing ? " Processing" : "Search"}
                        </Button>

                    </FormGroup>
                </Form>
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

                            <BootstrapTable data={this.props.titleSearchResults.titles} striped hover pagination={true}>
                                <TableHeaderColumn isKey dataSort={true} dataField="titleId" dataFormat={this.titleIdFormat.bind(this)} >Title Id</TableHeaderColumn>                                
                            </BootstrapTable>
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