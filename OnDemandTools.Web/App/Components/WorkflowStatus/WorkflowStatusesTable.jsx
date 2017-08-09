import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import AddEditWorkflowStatus from 'Components/WorkflowStatus/AddEditWorkflowStatus';
import RemoveWorkflowStatusModal from 'Components/WorkflowStatus/RemoveWorkflowStatusModal';
import { getNewStatus } from 'Actions/Status/StatusActions';

class WorkflowStatusesTable extends React.Component {

    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = {
            newStatusModel: {},
            showModal: false,
            showAddEditModel: false,
            showDeleteModal: false,
            status: "",
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
                }],
                onSortChange :this.onSortChange.bind(this)
            }
        }
    }

    ///<summary>
    /// on clicking sort arrow in any page of the table should take to the First page in the pagination.
    ///</summary>
    onSortChange() {
        const sizePerPage = this.refs.WorkflowStatusTable.state.sizePerPage;
        this.refs.WorkflowStatusTable.handlePaginationData(1, sizePerPage);
    }

    ///<summary>
    /// status description to display in the grid.
    ///</summary>
    descriptionFormat(val) {
        if (val != null) {
            const popoverDescLeft = (
                <Popover id="popover-positioned-left">
                    <div class="TitleOverlay-height"> {val} </div>
                </Popover>
            );
            if (val.length > 40) {
                return (
                    <OverlayTrigger trigger={['hover']} rootClose placement="bottom" overlay={popoverDescLeft}>
                        <div className="cursorPointer">
                            {val.substring(0, 20)} <i class="fa fa-ellipsis-h"></i>
                        </div>
                    </OverlayTrigger>
                );
            }
            else
                return '<p>' + val + '</p>';
        }
    }

    ///<summary>
    // Format the status name column
    ///</summary>
    StatusNameFormat(val) {
        return '<p>' + val + '</p>';
    }

    ///<summary>
    //Format the user group column
    ///</summary>
    userFormat(val) {
        return '<p>' + val + '</p>';
    }

    ///<summary>
    //Format the action column
    ///</summary>
    actionFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" title="Edit Status" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Delete Status" onClick={(event) => this.openDeleteModel(rowData, event)} >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
    }

    ///<summary>
    // when delete product button event handled
    ///</summary>
    openDeleteModel(val) {
        this.setState({ showDeleteModal: true, status: val });
    }

    ///<summary>
    // React modal pop up control delete status is closed
    ///</summary>
    closeDeleteModel() {
        this.setState({ showDeleteModal: false });
    }

    //<summary>
    // React modal pop up control edit status  event handled
    ///</summary>
    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, status: val });
    }

    ///<summary>
    // React modal pop up control edit status is closed
    ///</summary>
    closeAddEditModel() {
        this.setState({ showAddEditModel: false, status: this.state.newStatusModel });
    }

    ///<summary>
    // React modal pop up control create new status event handles
    ///</summary>
    createNewStatusModel() {
        this.setState({ showAddEditModel: true, status: jQuery.extend(true, {}, this.state.newStatusModel) });
    }

    componentDidMount() {
        let promise = getNewStatus();
        promise.then(response => {
            this.setState({
                newStatusModel: response
            });
        }).catch(error => {
            this.setState({
                newStatusModel: {}
            });
        });
    }
                                 
    render() {
        var row;
        row = this.props.ColumnData.map(function(item, index) {

            if (item.label == "Status Name") {
                return <TableHeaderColumn width="200px" dataField={item.dataField} key={index.toString()} dataSort={item.sort} dataFormat={this.StatusNameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Description") {
                return <TableHeaderColumn width="200px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.descriptionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "User Group") {
                return <TableHeaderColumn width="200px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.userFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn width="100px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }

        }.bind(this));

        var statusIdandNames=[];
       
        for (var i = 0; i < this.props.RowData.length; i++)
        {
            statusIdandNames.push({"id":this.props.RowData[i].id,"name":this.props.RowData[i].name});
        }
        return (
            <div>
                <button class="btn-link pull-right addMarginRight" title="New Status" onClick={(event) => this.createNewStatusModel(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New Status</span>
                </button>
                <BootstrapTable ref="WorkflowStatusTable"
                    data={this.props.RowData}
                    striped={true}
                    hover={true}
                    keyField={this.props.KeyField}
                    pagination={true}
                    options={this.state.options}
                    >
                    {row}
                </BootstrapTable>
                 <AddEditWorkflowStatus data={this.state} statusIdandNames={statusIdandNames}  handleClose={this.closeAddEditModel.bind(this)} />
                <RemoveWorkflowStatusModal data={this.state} handleClose={this.closeDeleteModel.bind(this)} />
            </div>)
    }

}

export default WorkflowStatusesTable;