import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { titleSearch, filterTitles } from 'Actions/TitleSearch/TitleSearchActions';
import { clearTitles } from 'Actions/TitleSearch/TitleSearchActions';
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
            processing: false,
            selectedTitleTypeFilter: "",
            selectedSeriesFilter: "",
            titlesToRemove: [],
            titlesToAdd: []
        }
    }

    handleChange(val) {

        this.props.dispatch(clearTitles());

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

    onTitleTypeChanged(value) {
        this.setState({ selectedTitleTypeFilter: value, selectedSeriesFilter: "" });
        var filter = {};
        filter.isTitleType = true;
        filter.value = value;
        this.props.dispatch(filterTitles(filter));
    }

    onSeriesChanged(value) {
        this.setState({ selectedSeriesFilter: value, selectedTitleTypeFilter: "" });
        var filter = {};
        filter.isTitleType = false;
        filter.value = value;
        this.props.dispatch(filterTitles(filter));
    }

    onSelectedTitlesRowSelect(row, isSelected, e) {
        var titlesToRemoveTemp = this.state.titlesToRemove;
        var titleFoundIdx = -1;

        for (var i = 0; i < titlesToRemoveTemp.length; i++) {
            if (titlesToRemoveTemp[i].titleId == row.titleId) {
                titleFoundIdx = i;
            }
        }

        if (isSelected) {
            if (titleFoundIdx == -1) {
                titlesToRemoveTemp.push(row);
            }
        }
        else {
            if (titleFoundIdx > -1) {
                titlesToRemoveTemp.splice(titleFoundIdx, 1);
            }
        }
        this.setState({ titlesToRemove: titlesToRemoveTemp });
    }

    onAvailableTitlesRowSelect(row, isSelected, e) {
        var titlesToAddTemp = this.state.titlesToAdd;
        var titleFoundIdx = -1;

        for (var i = 0; i < titlesToAddTemp.length; i++) {
            if (titlesToAddTemp[i].titleId == row.titleId) {
                titleFoundIdx = i;
            }
        }

        if (isSelected) {
            if (titleFoundIdx == -1) {
                titlesToAddTemp.push(row);
            }
        }
        else {
            if (titleFoundIdx > -1) {
                titlesToAddTemp.splice(titleFoundIdx, 1);
            }
        }
        this.setState({ titlesToAdd: titlesToAddTemp });
    }

    onAddTitleButtonClick() {
        this.props.onAddTitles(this.state.titlesToAdd);
        this.setState({ titlesToAdd: [] })
    }

    onRemoveTitleButtonClick() {
        this.props.onRemoveTitles(this.state.titlesToRemove);
        this.setState({ titlesToRemove: [] })
    }

    render() {

        const availableGridProps = {
            mode: 'checkbox',
            bgColor: 'yellow', // you should give a bgcolor, otherwise, you can't recognize which row has been selected
            hideSelectColumn: true,  // enable hide selection column.
            clickToSelect: true,  // you should enable clickToSelect, otherwise, you can't select column.
            onSelect: this.onAvailableTitlesRowSelect.bind(this)
        };

        const selectedGridProps = {
            mode: 'checkbox',
            bgColor: 'yellow', // you should give a bgcolor, otherwise, you can't recognize which row has been selected
            hideSelectColumn: true,  // enable hide selection column.
            clickToSelect: true,  // you should enable clickToSelect, otherwise, you can't select column.
            onSelect: this.onSelectedTitlesRowSelect.bind(this)
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
                        <TitleSearchParameter
                            name="Title Type"
                            onValueChange={this.onTitleTypeChanged.bind(this)}
                            parameters={this.props.titleSearchResults.titleTypeFilterParameters}
                            selectedItem={this.state.selectedTitleTypeFilter} />
                        {' '}
                        <TitleSearchParameter
                            name="Series"
                            onValueChange={this.onSeriesChanged.bind(this)}
                            parameters={this.props.titleSearchResults.seriesFilterParameters}
                            selectedItem={this.state.selectedSeriesFilter} />
                    </FormGroup>
                </Form>
                <Grid fluid={true}>
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
                            <BootstrapTable
                                containerStyle={{ margin: 0 }}
                                tableContainerClass='react-bs-table_filter'
                                selectRow={availableGridProps}
                                data={this.props.titleSearchResults.titles}
                                striped hover pagination={true}>
                                <TableHeaderColumn width="80px"  isKey dataSort={true} dataField="titleId" dataFormat={this.titleIdFormat.bind(this)} >Id</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="titleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Title</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="seriesTitleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Series</TableHeaderColumn>
                                <TableHeaderColumn width="70px" dataSort={true} dataField="releaseYear" dataFormat={this.titleIdFormat.bind(this)} >Year</TableHeaderColumn>
                                <TableHeaderColumn width="80px" dataSort={true} dataField="titleId" dataFormat={this.titleTypeFormat.bind(this)} >Type</TableHeaderColumn>
                            </BootstrapTable>
                        </Col>
                        <Col md={1} className="filterButtons">
                            <Button bsStyle="primary" disabled={this.state.titlesToAdd == 0} onClick={this.onAddTitleButtonClick.bind(this)}>
                                Add >
                        </Button>
                            <br /><br />
                            <Button bsStyle="primary" disabled={this.state.titlesToRemove == 0} onClick={this.onRemoveTitleButtonClick.bind(this)} >
                                Remove
                        </Button>
                        </Col>
                        <Col md={5}>
                            <BootstrapTable
                                containerStyle={{ margin: 0 }}
                                tableContainerClass='react-bs-table_filter'
                                selectRow={selectedGridProps}
                                data={this.props.selectedTitles}
                                striped hover pagination={true}>
                                <TableHeaderColumn width="80px" isKey dataSort={true} dataField="titleId" dataFormat={this.titleIdFormat.bind(this)} >Id</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="titleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Title</TableHeaderColumn>
                                <TableHeaderColumn dataSort={true} dataField="seriesTitleNameSortable" dataFormat={this.titleIdFormat.bind(this)} >Series</TableHeaderColumn>
                                <TableHeaderColumn width="70px" dataSort={true} dataField="releaseYear" dataFormat={this.titleIdFormat.bind(this)} >Year</TableHeaderColumn>
                                <TableHeaderColumn width="80px" dataSort={true} dataField="titleId" dataFormat={this.titleTypeFormat.bind(this)} >Type</TableHeaderColumn>
                            </BootstrapTable>
                        </Col>
                    </Row>
                </Grid>
            </div>
        )
    }
}

export default TitleSearch