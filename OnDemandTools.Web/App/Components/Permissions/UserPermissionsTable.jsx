import React from 'react';
import Moment from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
import AddEditUserPermissions from 'Components/Permissions/AddEditUserPermissions';
import * as permissionActions from 'Actions/Permissions/PermissionActions';
import UserInactivateModal from 'Components/Permissions/UserInactivateModal';
require('react-bootstrap-table/css/react-bootstrap-table.css');

@connect((store) => {
    return {

    }
})
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
            showInactiveModal: false,
            inActiveModal: {},
            permission: "",
            options: {
                defaultSortName: 'userName',
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

            var d = new Date(rowData.portal.lastLoginTime);
            var year = d.getFullYear();
            if (year < 2000) {
                return <p>{"never"}</p>;
            }
            else {

                var dateFormat = Moment(rowData.portal.lastLoginTime).format('lll');
                return <p> {dateFormat}</p>
            }
        }
    }

    onActiveCheckboxChange(rowValue, event) {
        if (event.target.checked == false) {
            this.setState({
                showInactiveModal: true,
                inActiveModal: rowValue
            });
        }
        else {
            var model = rowValue;
            model.portal.isActive = true;
            this.props.dispatch(permissionActions.savePermission(model));
        }
    }

    closeInactivateModal() {
        this.setState({ showInactiveModal: false })
    }

    ///<summary>
    //Format the action column
    ///</summary>
    actionFormat(val, rowData) {

        var onOffToolTip = "";

        if (rowData.portal.isActive) {
            onOffToolTip = "This user is active in ODT";
        }
        else {
            onOffToolTip = "This user is inactive in ODT"
        }

        return (
            <div>
                <button class="btn-link" title="Edit User" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o fa-lg"></i>
                </button>

                <label class="switch gridSwitch" title={onOffToolTip}>
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

    ///<summary>
    // Custom sort logic for Last Login time
    ///</summary>
    lastLoginCustomSort(a, b, sortOrder) {
        var date1 = new Date(b.portal.lastLoginTime);
        var date2 = new Date(a.portal.lastLoginTime);

        var time1 = date1.getTime();
        var time2 = date2.getTime();

        if (sortOrder == "desc") {
            return time1 - time2;
        }
        else {
            return time2 - time1;
        }
    }

    render() {
        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Actions") {
                return <TableHeaderColumn width="100px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Last Login") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort sortFunc={this.lastLoginCustomSort} dataFormat={this.lastLoginFormat.bind(this)}>{item.label}</TableHeaderColumn>
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
                <BootstrapTable ref="PermissionTable"
                    data={this.props.RowData}
                    striped={true}
                    hover={true}
                    keyField={this.props.KeyField}
                    pagination={true}
                    options={this.state.options}
                >
                    {row}
                </BootstrapTable>
                <AddEditUserPermissions data={this.state} handleClose={this.closeAddEditModel.bind(this)} />
                <UserInactivateModal data={this.state} handleClose={this.closeInactivateModal.bind(this)} />
            </div>)
    }

}

export default UserPermissionTable;