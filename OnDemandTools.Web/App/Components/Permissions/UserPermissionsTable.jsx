import React from 'react';
import Moment from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
import AddEditUserPermissions from 'Components/Permissions/AddEditUserPermissions';
import * as permissionActions from 'Actions/Permissions/PermissionActions';
require('react-bootstrap-table/css/react-bootstrap-table.css');

class UserPermissionTable extends React.Component {

    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = {
            newPermissionModel: {},
            showModal: false,
            showAddEditModel: false,
            showDeleteModal: false,
            permission: "",
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
                onSortChange: this.onSortChange.bind(this)
            }
        }
    }

    ///<summary>
    /// on clicking sort arrow in any page of the table should take to the First page in the pagination.
    ///</summary>
    onSortChange() {

    }


    ///<summary>
    // Format the userName, first Name, and Last name column
    ///</summary>
    stringFormat(val) {
        return <p>  {val} </p>;
    }



    lastLoginFormat(val, rowData) {
        if (rowData.portal.lastLoginTime == null) {
            return <p>{"never"}</p>;
        }
        else {
            var dateFormat = Moment(rowData.portal.lastLoginTime).format('lll');
            return <p> {dateFormat}</p>
        }
    }

    onActiveCheckboxChange(val, event) {
        console.log(val);
        console.log(event.target.checked);
    }

    ///<summary>
    //Format the action column
    ///</summary>
    actionFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" title="Edit User" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o fa-lg"></i>
                </button>

                <label class="switch gridSwitch">
                  <input type="checkbox" checked={rowData.portal.isActive} onChange={(event) => this.onActiveCheckboxChange(rowData, event)} />
                    <span class="slider round"></span>
                </label>
            </div>
        );
    }


    //<summary>
    // React modal pop up control edit permission  event handled
    ///</summary>
    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, permission: val });
    }

    ///<summary>
    // React modal pop up control edit permission is closed
    ///</summary>
    closeAddEditModel() {
        this.setState({ showAddEditModel: false, permission: this.state.newPermissionModel });
    }

    ///<summary>
    // React modal pop up control create new permission event handles
    ///</summary>
    createNewPermissionModel() {
        this.setState({ showAddEditModel: true, permission: jQuery.extend(true, {}, this.state.newPermissionModel) });
    }

    componentDidMount() {
       
        let promise = permissionActions.getNewUserPermission();
        promise.then(response => {
            this.setState({
                newPermissionModel: response
            });
        }).catch(error => {
            this.setState({
                newPermissionModel: {}
            });
        });
    }

    render() {
        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Actions") {
                return <TableHeaderColumn width="100px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Last Login") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.lastLoginFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.dataField == "userName" || item.dataField == "firstName" || item.dataField == "lastName") {
                return <TableHeaderColumn dataField={item.dataField} key={index.toString()} dataSort={item.sort} dataFormat={this.stringFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }

        }.bind(this));

        return (
            <div>
                <button class="btn-link pull-right addMarginRight" title="New User" onClick={(event) => this.createNewPermissionModel(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New User</span>
                </button>
                <BootstrapTable ref="WorkflowPermissionTable"
                    data={this.props.RowData}
                    striped={true}
                    hover={true}
                    keyField={this.props.KeyField}
                    pagination={true}
                    options={this.state.options}
                >
                    {row}
                </BootstrapTable>
                 <AddEditUserPermissions data={this.state}  handleClose={this.closeAddEditModel.bind(this)} />
            </div>)
    }

}

export default UserPermissionTable;