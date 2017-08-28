import React from 'react';
import { connect } from 'react-redux';

import * as permissionActions from 'Actions/Permissions/PermissionActions';
import PageHeader from 'Components/Common/PageHeader';
import PermissionsTable from 'Components/Permissions/SystemPermissionsTable';
import PermissionsFilter from 'Components/Permissions/UserPermissionsFilter';

@connect((store) => {
    return {
        permissions: store.permissions
    };
})
class SystemPermissionsPage extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            permission: [],
            filterValue: {
                name: "",
                userId: "",
                includeInactive: false
            },

            columns: [{ "label": "System Id", "dataField": "userName", "sort": true },
            { "label": "API Key", "dataField": "ApiKey", "sort": false },
            { "label": "System Contact", "dataField": "FunctionalContactId", "sort": true },
            { "label": "API Last Accessed", "dataField": "LastAccessTime", "sort": false },
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
    }

    componentDidMount() {
        this.props.dispatch(permissionActions.fetchPermissionRecords("system"));
        document.title = "ODT - System Management";

    }

    // The goal of this function is to filter 'permission' (which is stored in Redux store)
    // based on user provided filter criteria and return the refined 'permission' list.
    // If no filter criteria is provided then return the full 'permission' list
    applyFilter(permissions, filter) {
        if (filter.name != undefined) {
            var filteredPermissions = permissions;
            console.log(filteredPermissions);
            if (filter.includeInactive == false) {
                filteredPermissions = filteredPermissions.filter(function (permission) {
                    return permission.api.isActive
                });
            }

            var name = filter.name.toLowerCase();

            if (name != "") {
                filteredPermissions = filteredPermissions.filter(function (permission) {
                    return permission.firstName.toLowerCase().indexOf(name) > -1 ||
                        permission.lastName.toLowerCase().indexOf(name) > -1
                });
            }


            var userId = filter.userId.toLowerCase();

            if (userId != "") {
                filteredPermissions = filteredPermissions.filter(function (permission) {
                    return permission.userName.toLowerCase().indexOf(userId) > -1
                });
            }

            return filteredPermissions;

        }
        return permissions;
    }

    render() {
        var filteredRows = this.applyFilter(this.props.permissions, this.state.filterValue);

        return (
            <div>
                <PageHeader pageName="System Management" />
                <PermissionsTable RowData={filteredRows} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

export default SystemPermissionsPage