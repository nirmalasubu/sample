import React from 'react';
import { connect } from 'react-redux';

import * as permissionActions from 'Actions/Permissions/PermissionActions';
import PageHeader from 'Components/Common/PageHeader';
import PermissionsTable from 'Components/Permissions/PermissionsTable';

@connect((store) => {
    ///<summary>
    /// Retrieve filtered list of permission based on filterValue like permission name, description and destination
    /// which are defined in Redux store
    ///</summary>
    var filteredPermissionValues = getFilterVal(store.permissions, store.filterPermissions);
    return {
        permissions: store.permissions,
        filteredPermissions: store.permissions
    };
})
class PermissionsPage extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            permission: [],
            filterValue: {
                name: "",
                description: "",
                user: ""
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
    handleFilterUpdate(filtersValue, type) {

        var stateFilterValue = this.state.filterValue;

        this.setState({
            filterValue: stateFilterValue
        });
        this.props.dispatch(permissionActions.filterPermissionSuccess(this.state.filterValue));

    }

    componentDidMount() {
        this.props.dispatch(permissionActions.fetchPermissionRecords());

        document.title = "ODT - User Management";

    }

    render() {
        return (
            <div>
                <PageHeader pageName="User Management" />
                <PermissionsTable RowData={this.props.filteredPermissions} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

// The goal of this function is to filter 'permission' (which is stored in Redux store)
// based on user provided filter criteria and return the refined 'permission' list.
// If no filter criteria is provided then return the full 'permission' list
const getFilterVal = (permissions, filterVal) => {
    return permissions;
};

///<summary>
// returns  true  if any of the search value matches description 
///</summary>
const matchDescription = (objdescription, description) => {

    if (objdescription == null) // skip the null description values 
        return false;

    return objdescription.toLowerCase().indexOf(description) != -1;
};
export default PermissionsPage



