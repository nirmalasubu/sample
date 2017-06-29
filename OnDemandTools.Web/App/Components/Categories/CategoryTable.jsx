import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Button } from 'react-bootstrap';
import CategoryExpandTable from 'Components/Categories/CategoryExpandTable';
import AddEditCategory from 'Components/Categories/AddEditCategory';
require('react-bootstrap-table/css/react-bootstrap-table.css');


class CategoryTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            newCategoryModel: "",
            showModal: false,
            showAddEditModel: false,
            categoryDetails: "",
            options: {
                defaultSortName: 'name',
                defaultSortOrder: 'asc',

                sizePerPageList: [{
                    text: '10 ', value: 10
                }, {
                    text: '25 ', value: 25
                }, {
                    text: '50 ', value: 50
                },
                {
                    text: 'All ', value: 10000000
                }]
            }
        }
    }

    componentDidMount() {
    }

    openCreateNewDestinationModel() {

    }

    openModel(val) {
    }

    closeModel() {
    }

    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, categoryDetails: val });
    }

    closeAddEditModel() {
        this.setState({ showAddEditModel: false, categoryDetails: this.state.newCategoryModel });
    }

    descriptionFormat(val) {
    }

    isExpandableRow(row) {
        if (row.destinations.length > 0) return true;
        else return false;
    }

    expandComponent(row) {
        return (
            <CategoryExpandTable data={row} />
        );
    }

    categoryNameFormat(val, rowData) {
        return (
            <p data-toggle="tooltip"> {val}</p>
        );
    }

    destinationFormat(val, rowData) {
        var rows = [];

        for (var idx = 0; idx < rowData.destinations.length; idx++) {
            rows.push(<Button key={idx.toString()}> {rowData.destinations[idx].name} </Button>);
        }
        return (
            <div>
                {rows}
            </div>
        );

    }

    actionFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" title="Edit Destination" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Delete Destination" onClick={(event) => this.openModel(rowData, event)} >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
    }

    render() {

        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Name") {
                return <TableHeaderColumn width="250px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.categoryNameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Destinations") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.destinationFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="100px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} >{item.label}</TableHeaderColumn>
            }

        }.bind(this));
        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="New Category" onClick={(event) => this.openCreateNewDestinationModel(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New Category</span>
                    </button>
                </div>
                <BootstrapTable
                    expandableRow={this.isExpandableRow}
                    expandComponent={this.expandComponent}
                    data={this.props.RowData}
                    striped={true}
                    hover={true}
                    keyField={this.props.KeyField}
                    pagination={true}
                    options={this.state.options}>
                    {row}
                </BootstrapTable>

                <AddEditCategory data={this.state} handleClose={this.closeAddEditModel.bind(this)} />
            </div>)
    }

}

export default CategoryTable;

