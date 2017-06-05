﻿import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { getNewQueue } from 'Actions/Destination/DestinationActions';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import AddEditDestinationModel from 'Components/Destinations/AddEditDestination';


class DestinationTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            showModal: false,
            newDestinationModel: {},
            showAddEditModel: false,
            destinationDetails: "",
            modalActionType: "",
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
        this.setState({ showAddEditModel: true, destinationDetails: this.state.newDestinationModel });
    }

    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, destinationDetails: val });
    }

    closeAddEditModel() {
        this.setState({ showAddEditModel: false });
    }

    contentFormat(val, rowData) {
        var content = [];
        if (rowData.content) {
            if (rowData.content.highDefinition)
                content.push("HD");
            if (rowData.content.standardDefinition)
                content.push("SD");
            if (rowData.content.cx)
                content.push("C(X)");
            if (rowData.content.nonCx)
                content.push("Non-C(X)");
        }
        return '<p data-toggle="tooltip">' + content.toString() + '</p>';
    }

    descriptionFormat(val) {
        return '<p data-toggle="tooltip" class="shortDesc">' + val + '</p>';
    }

    codeFormat(val, rowData) {
        return (
            '<p data-toggle="tooltip" class="shortDesc">' + val + '</p>'
        );
    }

    actionFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" title="Edit Destination" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Delete Destination" >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
    }


    render() {

        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Code") {
                return <TableHeaderColumn width="150px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.codeFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Description") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.descriptionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="100px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.contentFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }

        }.bind(this));
        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="Create New Destination" > <i class="fa fa-plus-square fa-2x"  ></i> </button>
                </div>
                <BootstrapTable data={this.props.RowData} striped={true} hover={true} keyField={this.props.KeyField} pagination={true} options={this.state.options}>
                    {row}
                </BootstrapTable>

                <AddEditDestinationModel data={this.state} handleClose={this.closeAddEditModel.bind(this)} />
            </div>)
    }

}

export default DestinationTable;

