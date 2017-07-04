﻿import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { getNewDestination } from 'Actions/Destination/DestinationActions';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import AddEditDestinationModel from 'Components/Destinations/AddEditDestination';
import RemoveDestinationModal from 'Components/Destinations/RemoveDestinationModal';
import * as destinationAction from 'Actions/Destination/DestinationActions';

@connect((store) => {
    return {
        filterDestination: store.filterDestination
    };
})

class DestinationTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            showModal: false,
            newDestinationModel: {},
            showAddEditModel: false,
            showWarningModel: false,
            destinationDetails: "",
            modalActionType: "",
            options: {
                defaultSortName: 'name',
                defaultSortOrder: 'asc',
                noDataText: this._setLoadingOption(),

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

    _setLoadingOption()
    {
        if(this.props.RowData.length<=0 && (this.props.filterDestination.length<=0 || (this.props.filterDestination.code=="" && this.props.filterDestination.description=="" && this.props.filterDestination.content=="")))
            return (<div><i class="fa fa-spinner fa-pulse fa-fw margin-bottom"></i> <i>Loading...</i></div>);
        else
            return(<i>There is no data to display</i>);
    }

    componentDidMount() {
        let promise = getNewDestination();
        promise.then(message => {
            this.setState({
                newDestinationModel: message
            });
        }).catch(error => {
            this.setState({
                newDestinationModel: {}
            });
        });
    }

    openCreateNewDestinationModel() {
        this.setState({ showAddEditModel: true, destinationDetails: jQuery.extend(true, {}, this.state.newDestinationModel) });
    }

    openModel(val) {
        this.setState({ showModal: true, destinationDetails: val });
    }

    closeModel() {
        this.setState({ showModal: false });
    }

    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, destinationDetails: val });
    }

    closeAddEditModel() {        
        this.setState({ showAddEditModel: false, destinationDetails: this.state.newDestinationModel });
    }

    contentSortFormat(val, rowData) {
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
        return content.toString();
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

    revertSortFunc(a, b, order) {   // order is desc or asc
        //if (order === 'desc') {
        //    return a.price - b.price;
        //} else {
        //    return b.price - a.price;
        //}
        console.log(a.content.highDefinition + " " + b.content.highDefinition + "/n" +
                    a.name + " " + b.name);
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

                <button class="btn-link" title="Delete Destination" onClick={(event) => this.openModel(rowData, event)} >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
    }

    render() {
        if(this.props.RowData.length<=0 && (this.props.filterDestination.length<=0 || (this.props.filterDestination.code=="" && this.props.filterDestination.description=="" && this.props.filterDestination.content=="")))
            this.state.options.noDataText = <div><i class="fa fa-spinner fa-pulse fa-fw margin-bottom"></i> <i>Loading...</i></div>;
        else
            this.state.options.noDataText = <i>There is no data to display</i>;

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
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} sortFunc={ this.revertSortFunc.bind(this) } dataFormat={this.contentSortFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }

        }.bind(this));
        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="New Destination" onClick={(event) => this.openCreateNewDestinationModel(event)}>
                        <i class="fa fa-plus-square fa-2x"></i> 
                        <span class="addVertialAlign"> New Destination</span>
                    </button>
                </div>
                <BootstrapTable data={this.props.RowData} striped={true} hover={true} keyField={this.props.KeyField} pagination={true} options={this.state.options}>
                    {row}
                </BootstrapTable>

                <AddEditDestinationModel data={this.state} handleClose={this.closeAddEditModel.bind(this)} />
                <RemoveDestinationModal data={this.state} handleClose={this.closeModel.bind(this)} />                
            </div>)
    }

}

export default DestinationTable;

