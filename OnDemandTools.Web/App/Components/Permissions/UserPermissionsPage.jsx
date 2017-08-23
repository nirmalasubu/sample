import React from 'react';
import { connect } from 'react-redux';

import * as permissionActions from 'Actions/Permissions/PermissionActions';
import PageHeader from 'Components/Common/PageHeader';
import PermissionsTable from 'Components/Permissions/UserPermissionsTable';
import PermissionsFilter from 'Components/Permissions/UserPermissionsFilter';

@connect((store) => {
    ///<summary>
    /// Retrieve filtered list of permission based on filterValue like permission name, description and destination
    /// which are defined in Redux store
    ///</summary>
    var filteredValues = getFilterVal(store.permissions, store.filterPermission);
    return {
        permissions: store.permissions,
        filteredPermissions: filteredValues == undefined ? store.permissions : filteredValues
    };
})
class UserPermissionsPage extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            permission: [],
            filterValue: {
                name: "",
                userId: "",
                includeInactive: false
            },

            columns: [{ "label": "User Id", "dataField": "userName", "sort": true },
            { "label": "First Name", "dataField": "firstName", "sort": false },
            { "label": "Last Name", "dataField": "lastName", "sort": false },
            { "label": "Last Login", "dataField": "userName", "sort": false },
            { "label": "Actions", "dataField": "userName", "sort": false }
            ],
            keyField: "userName"
        }
    }

    ///<summary>
    ///callback function to get the filter value from filter component
    ///</summary>
    handleFilterUpdate(filterValue, type) {

        var stateFilterValue = this.state.filterValue;

        switch (type) {
            case "Name":
                stateFilterValue.name = filterValue;
                break;
            case "UserId":
                stateFilterValue.userId = filterValue;
                break;
            case "IncludeInactive":
                stateFilterValue.includeInactive = filterValue;
                break;
            case "Clear":
                stateFilterValue.name = "";
                stateFilterValue.userId = "";
                stateFilterValue.includeInactive = false;
                break;
        }

        this.setState({
            filterValue: stateFilterValue
        });
        this.props.dispatch(permissionActions.filterPermissionSuccess(stateFilterValue));

    }

    componentDidMount() {
        this.props.dispatch(permissionActions.fetchPermissionRecords());
        document.title = "ODT - User Management";

    }

    render() {
        return (
            <div>
                <PageHeader pageName="User Management" />
                <PermissionsFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <PermissionsTable RowData={this.props.filteredPermissions} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

// The goal of this function is to filter 'permission' (which is stored in Redux store)
// based on user provided filter criteria and return the refined 'permission' list.
// If no filter criteria is provided then return the full 'permission' list
const getFilterVal = (permissions, filterVal) => {
    if (filterVal.name != undefined) {

        var filteredRows = permissions;

        if (filterVal.name != '') {
            filteredRows = filteredRows.filter(obj => obj.userName.toLowerCase().indexOf(filterVal.name.toLowerCase() > -1));
        }

        return filteredRows;

    }
    return permissions;
};

export default UserPermissionsPage



