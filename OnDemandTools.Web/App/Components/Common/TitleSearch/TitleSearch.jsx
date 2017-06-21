import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { titleSearch } from 'Actions/TitleSearch/TitleSearchActions';
import { connect } from 'react-redux';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import TitleSearchParameter from 'Components/Common/TitleSearch/TitleSearchParameter';

@connect((store) => {
    return {
        titleSearchResults: store.titleSearch
    };
})
class TitleSearch extends React.Component {
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

    titleTypeFormat(val, row) {
        return <p> {row.titleType.name} </p>
    }

    render() {

        const selectRowProp = {
            mode: 'checkbox'
        };

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
                        {' '}
                        <TitleSearchParameter inline name="Title Type" parameters={this.props.titleSearchResults.titleTypeFilterParameters} />
                        {' '}
                        <TitleSearchParameter inline name="Series" parameters={this.props.titleSearchResults.seriesFilterParameters} />
                    </FormGroup>
                </Form>
                <Grid>
                    <Row>
                        <Col md={5} >
                            <ControlLabel>Available Title/Series</ControlLabel>
                        </Col>
                        <Col md={1} >

                        </Col>
                        <Col md={5} >
                            <ControlLabel>Selected Title/Series</ControlLabel>
                        </Col>
                    </Row>
                    <Row>
                        <Col md={5} >
                            <BootstrapTable selectRow={selectRowProp} data={this.props.titleSearchResults.titles} striped hover pagination={true}>
                                <TableHeaderColumn isKey dataSort={true} dataField="titleId" dataFormat={this.titleIdFormat.bind(this)} >Id</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="titleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Title</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="seriesTitleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Series</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="releaseYear" dataFormat={this.titleIdFormat.bind(this)} >Year</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="titleId" dataFormat={this.titleTypeFormat.bind(this)} >Type</TableHeaderColumn>
                            </BootstrapTable>
                        </Col>
                        <Col md={1}>
                            <Button bsStyle="primary" >
                                Add >
                        </Button>
                            <br /><br />
                            <Button bsStyle="primary" >
                                Remove
                        </Button>
                        </Col>
                        <Col md={5}>
                            <BootstrapTable data={this.props.titleSearchResults.titles} striped hover pagination={true}>
                                <TableHeaderColumn isKey dataSort={true} dataField="titleId" dataFormat={this.titleIdFormat.bind(this)} >Id</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="titleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Title</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="seriesTitleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Series</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="releaseYear" dataFormat={this.titleIdFormat.bind(this)} >Year</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="titleId" dataFormat={this.titleTypeFormat.bind(this)} >Type</TableHeaderColumn>
                            </BootstrapTable>
                        </Col>
                    </Row>
                </Grid>
            </div>
        )
    }
}

export default TitleSearch