import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
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
    }

    closeAddEditModel() { 
    }

    descriptionFormat(val) {
    }

    categoryNameFormat(val, rowData) {
        return (
            '<p data-toggle="tooltip">' + val + ' ( <a href="#">' + rowData.destinations.length + '</a> ) ' + '</p>'
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
            else if (item.label == "Description") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} >{item.label}</TableHeaderColumn>
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
                <BootstrapTable data={this.props.RowData} striped={true} hover={true} keyField={this.props.KeyField} pagination={true} options={this.state.options}>
                    {row}
                </BootstrapTable>               
            </div>)
    }

}

export default CategoryTable;

