import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { getNewQueue } from 'Actions/Destination/DestinationActions';
require('react-bootstrap-table/css/react-bootstrap-table.css');


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
        //let promise = getNewDestination();
        //promise.then(message => {
        //    this.setState({
        //        newDestinationModel: message
        //    });
        //}).catch(error => {
        //    this.setState({
        //        newDestinationModel: {}
        //    });
        //});
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
        var content=[];
        if(rowData.content)
        {
            if(rowData.content.highDefinition)
                content.push("HD");
            if(rowData.content.standardDefinition)
                content.push("SD");
            if(rowData.content.cx)
                content.push("C(X)");
            if(rowData.content.nonCx)
                content.push("Non-C(X)");
        }
        return '<p data-toggle="tooltip">' + content.toString() + '</p>';
    }

    descriptionFormat(val) {
        return '<p data-toggle="tooltip" title="' + val + '">' + val + '</p>';
    }

    codeFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" > {val} </button><br/>
            </div>
            );
    }


    render() {
        
        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Code") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.codeFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Description") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.descriptionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.contentFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }

        }.bind(this));
        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" >Create New Queue</button>
                </div>
                <BootstrapTable data={this.props.RowData} striped={true} hover={true} keyField={this.props.KeyField} pagination={true} options={this.state.options}>
                    {row}
                </BootstrapTable>
            </div>)
    }

}

export default DestinationTable;

