import React from 'react';
import Moment from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
import AddEditUserPermissions from 'Components/Permissions/AddEditUserPermissions';
import * as permissionActions from 'Actions/Permissions/PermissionActions';
import UserInactivateModal from 'Components/Permissions/SystemInactivateModal';
require('react-bootstrap-table/css/react-bootstrap-table.css');

@connect((store) => {
    return {

    }
})
class SystemPermissionsTable extends React.Component {

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
    systemContactFormat(val, rowData) {
        return <p>  {rowData.api.technicalContactId} </p>;
    }

    nameFormat(val) {
        return <p>{val}</p>;
    }
        
    apiFormat(val, rowData) {
        return <p>  {rowData.api.apiKey} </p>;
    }

    lastAccessFormat(val, rowData) {
        if (rowData.api.lastAccessTime == null) {
            return <p>{"never"}</p>;
        }
        else {

            var d = new Date(rowData.api.lastAccessTime);
            var year = d.getFullYear();
            if(year<2000)
            {
                return <p>{"never"}</p>;
            }
            else
            {
                var dateFormat = Moment(rowData.api.lastAccessTime).format('lll');
                return <p> {dateFormat}</p>
            }
        }
    }

    ///<summary>
    // Custom sort logic for Last Login time
    ///</summary>
    lastAccessLoginCustomSort(a, b, sortOrder) {
        var date1 = new Date(b.api.lastAccessTime);
        var date2 = new Date(a.api.lastAccessTime);

        var time1 = date1.getTime();
        var time2 = date2.getTime();

        if (sortOrder == "desc") {
            return time1 - time2;
        }
        else {
            return time2 - time1;
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
            model.api.isActive = true;
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
        
        if (rowData.api.isActive) {
            onOffToolTip = "This system is active in ODT";
        }
        else {
            onOffToolTip = "This system is inactive in ODT"
        }

        return (
            <div>
                <button class="btn-link" title="Edit System" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o fa-lg"></i>
                </button>

                <label class="switch gridSwitch" title={onOffToolTip}>
                    <input type="checkbox" checked={rowData.api.isActive} onChange={(event) => this.onActiveCheckboxChange(rowData, event)} />
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
            else if (item.label == "API Last Accessed") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort sortFunc={this.lastAccessCustomSort} dataFormat={this.lastAccessFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "System Contact") {
                return <TableHeaderColumn key={index++} dataSort={item.sort} dataFormat={this.systemContactFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "API Key") {
                return <TableHeaderColumn key={index++} dataSort={item.sort} dataFormat={this.apiFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.dataField == "userName") {
                return <TableHeaderColumn dataField={item.dataField} key={index.toString()} dataSort={item.sort} dataFormat={this.nameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }

        }.bind(this));

        return (
            <div>
                <button class="btn-link pull-right addMarginRight" title="New System" onClick={(event) => this.createNewPermissionModel(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New System</span>
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
                 <AddEditUserPermissions data={this.state}  handleClose={this.closeAddEditModel.bind(this)} />
                <UserInactivateModal data={this.state} handleClose={this.closeInactivateModal.bind(this)} />
            </div>)
    }

}

export default SystemPermissionsTable;