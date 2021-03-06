import React from 'react';
import { connect } from 'react-redux';

import * as permissionActions from 'Actions/Permissions/PermissionActions';
import PageHeader from 'Components/Common/PageHeader';
import PermissionsTable from 'Components/Permissions/System/SystemPermissionsTable';
import PermissionsFilter from 'Components/Permissions/System/SystemPermissionsFilter';

@connect((store) => {
    return {
        permissions: store.permissions,
        user: store.user
    };
})
class SystemPermissionsPage extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            permission: [],
            currentUserPermissions: { canAdd: false, canRead: false, canEdit: false, canAddOrEdit: false, disableControl: true },
            filterValue: {
                name: "",
                systemId: "",
                includeInactive: false,
                portalUsers: []
            },

            columns: [{ "label": "System ID", "dataField": "userName", "sort": true },
            { "label": "API Key", "dataField": "ApiKey", "sort": false },
            { "label": "System Contact", "dataField": "FunctionalContactId", "sort": false },
            { "label": "API Last Accessed", "dataField": "LastAccessTime", "sort": true },
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
            case "SystemId":
                stateFilterValue.systemId = filterValue;
                break;
            case "IncludeInactive":
                stateFilterValue.includeInactive = filterValue;
                break;
            case "Clear":
                stateFilterValue.name = "";
                stateFilterValue.systemId = "";
                stateFilterValue.includeInactive = false;
                break;
        }

        this.setState({
            filterValue: stateFilterValue
        });
    }

    componentDidMount() {
        this.props.dispatch(permissionActions.fetchPermissionRecords("system"));

        let promise = permissionActions.getPortalUsers();

        promise.then(response => {
            this.setState({ portalUsers: response });
        });
        document.title = "ODT - System Management";

        if (this.props.user && this.props.user.portal) {
            this.setState({ currentUserPermissions: this.props.user.portal.modulePermissions.SystemManagement })
        }
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        if (nextProps.user && nextProps.user.portal) {
            this.setState({ currentUserPermissions: nextProps.user.portal.modulePermissions.SystemManagement });
        }
    }

    // The goal of this function is to filter 'permission' (which is stored in Redux store)
    // based on user provided filter criteria and return the refined 'permission' list.
    // If no filter criteria is provided then return the full 'permission' list
    applyFilter(permissions, filter) {
        if (filter.name != undefined) {
            var filteredPermissions = permissions;
            if (filter.includeInactive == false) {
                filteredPermissions = filteredPermissions.filter(function (permission) {
                    return permission.api.isActive
                });
            }

            var name = filter.name.toLowerCase();

            if (name != "") {
                filteredPermissions = filteredPermissions.filter(function (permission) {
                    return (permission.api.technicalContactUser != null ? permission.api.technicalContactUser.firstName.toLowerCase().indexOf(name) > -1 : false) ||
                        (permission.api.technicalContactUser != null ? permission.api.technicalContactUser.lastName.toLowerCase().indexOf(name) > -1 : false) ||
                        (permission.api.technicalContactUser != null ? (permission.api.technicalContactUser.firstName.toLowerCase() + " " + permission.api.technicalContactUser.lastName.toLowerCase()).indexOf(name) > -1 : false)
                });
            }


            var systemId = filter.systemId.toLowerCase();

            if (systemId != "") {
                filteredPermissions = filteredPermissions.filter(function (permission) {
                    return permission.userName.toLowerCase().indexOf(systemId) > -1
                });
            }

            return filteredPermissions;

        }
        return permissions;
    }

    render() {

        if (this.props.user.portal == undefined) {
            return <div>Loading...</div>;
        }
        else if (!this.state.currentUserPermissions.canRead) {
            return <h3>Unauthorized to view this page</h3>;
        }

        var filteredRows = this.applyFilter(this.props.permissions, this.state.filterValue);

        return (
            <div>
                <PageHeader pageName="System Management" />
                <PermissionsFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <PermissionsTable RowData={filteredRows} portalUsers={this.state.portalUsers} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

export default SystemPermissionsPage