import React from 'react';
import { connect } from 'react-redux';

import * as statusActions from 'Actions/Status/StatusActions';
import PageHeader from 'Components/Common/PageHeader';
import WorkflowStatusesTable from 'Components/WorkflowStatus/WorkflowStatusesTable';
import WorkflowStatusesFilter from 'Components/WorkflowStatus/WorkflowStatusesFilter';

@connect((store) => {
    return {
        status: store.statuses,
        user: store.user
    }
})
class WorkflowStatuses extends React.Component {

    constructor(props) {

        super(props);

        this.state = {
            status: [],
            permissions: { canAdd: false, canRead: false, canEdit: false, canAddOrEdit: false, disableControl: true },
            filterValue: {
                name: "",
                description: "",
                user: ""
            },

            columns: [{ "label": "Status Name", "dataField": "name", "sort": true },
            { "label": "Description", "dataField": "description", "sort": false },
            { "label": "User Group", "dataField": "user", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "name"
        }
    }
    ///<summary>
    ///callback function to get the filter value from filter component
    ///</summary>
    handleFilterUpdate(filtersValue, type) {

        var stateFilterValue = this.state.filterValue;

        if (type == "SN")
            stateFilterValue.name = filtersValue;

        if (type == "DS")
            stateFilterValue.description = filtersValue;

        if (type == "US")
            stateFilterValue.user = filtersValue;

        if (type == "CL") {
            stateFilterValue.name = "";
            stateFilterValue.description = "";
            stateFilterValue.user = "";
        }
        this.setState({
            filterValue: stateFilterValue
        });
    }

    componentDidMount() {
        this.props.dispatch(statusActions.fetchStatus());
        document.title = "ODT - Workflow Statuses";

        if (this.props.user && this.props.user.portal) {
            this.setState({ permissions: this.props.user.portal.modulePermissions.WorkflowStatuses })
        }
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        if (nextProps.user && nextProps.user.portal) {
            this.setState({ permissions: nextProps.user.portal.modulePermissions.WorkflowStatuses });
        }
    }

    // The goal of this function is to filter 'status' (which is stored in Redux store)
    // based on user provided filter criteria and return the refined 'status' list.
    // If no filter criteria is provided then return the full 'status' list
    getFilterVal(statuses, filterVal) {

        if (filterVal.name != undefined) {
            var name = filterVal.name.toLowerCase();
            var description = filterVal.description.toLowerCase();
            var user = filterVal.user.toLowerCase();


            return statuses.filter(obj => ((name != "" ? obj.name.toLowerCase().indexOf(name) != -1 : true)
                && (description != "" ? matchDescription(obj.description, description) : true)
                && (user != "" ? obj.user.toLowerCase().indexOf(user) != -1 : true)
            ));
        }
        else
            statuses;
    };

    render() {

        if (this.props.user.portal == undefined) {
            return <div>Loading...</div>;
        }
        else if (!this.state.permissions.canRead) {
            return <h3>Unauthorized to view this page</h3>;
        }

        var filteredStatus = this.getFilterVal(this.props.status, this.state.filterValue);
        return (
            <div>
                <PageHeader pageName="Workflow Statuses" />
                <WorkflowStatusesFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <WorkflowStatusesTable RowData={filteredStatus} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

// The goal of this function is to filter 'status' (which is stored in Redux store)
// based on user provided filter criteria and return the refined 'status' list.
// If no filter criteria is provided then return the full 'status' list
const getFilterVal = (statuses, filterVal) => {

    if (filterVal.name != undefined) {
        var name = filterVal.name.toLowerCase();
        var description = filterVal.description.toLowerCase();
        var user = filterVal.user.toLowerCase();


        return statuses.filter(obj => ((name != "" ? obj.name.toLowerCase().indexOf(name) != -1 : true)
            && (description != "" ? matchDescription(obj.description, description) : true)
            && (user != "" ? obj.user.toLowerCase().indexOf(user) != -1 : true)
        ));
    }
    else
        statuses;
};

///<summary>
// returns  true  if any of the search value matches description 
///</summary>
const matchDescription = (objdescription, description) => {

    if (objdescription == null) // skip the null description values 
        return false;

    return objdescription.toLowerCase().indexOf(description) != -1;
};
export default WorkflowStatuses



