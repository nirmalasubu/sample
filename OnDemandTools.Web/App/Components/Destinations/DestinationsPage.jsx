import React from 'react';
import { connect } from 'react-redux';
import * as destinationActions from 'Actions/Destination/DestinationActions';
import DestinationFilter from 'Components/Destinations/DestinationFilter';
import DestinationTable from 'Components/Destinations/DestinationTable';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    return {
        destinations: store.destinations,
        user: store.user
    };
})
class DestinationPage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            stateQueue: [],

            filterValue: {
                code: "",
                description: "",
                content: ""
            },

            columns: [{ "label": "Code", "dataField": "name", "sort": true },
            { "label": "Description", "dataField": "description", "sort": true },
            { "label": "Content", "dataField": "content", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "name"
        }
    }

    //callback function to get the filter value from filter component
    handleFilterUpdate(filtersValue, type) {
        var valuess = this.state.filterValue;
        if (type == "CD")
            valuess.code = filtersValue;

        if (type == "DS")
            valuess.description = filtersValue;

        if (type == "CN")
            valuess.content = filtersValue;

        if (type == "CL") {
            valuess.code = "";
            valuess.description = "";
            valuess.content = "";
        }

        this.setState({
            filterValue: valuess
        });
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(destinationActions.fetchDestinations());

        document.title = "ODT - Destinations";
    }

    //filter the states using filter values
    getFilterVal(destinations, filterVal) {
        if (filterVal.code != undefined) {
            var code = filterVal.code.toLowerCase();
            var description = filterVal.description.toLowerCase();
            var content = filterVal.content;

            var filteredDestinations = destinations;

            if (code != "") {
                filteredDestinations = filteredDestinations.filter(function (dest) {
                    return dest.name.toLowerCase().indexOf(code) != -1
                });
            }

            if (description != "") {
                filteredDestinations = filteredDestinations.filter(function (dest) {
                    return dest.description.toLowerCase().indexOf(description) != -1
                });
            }

            if (content == "") return filteredDestinations;

            if (content == "None") {
                filteredDestinations = filteredDestinations.filter(function (dest) {
                    return dest.content == null || (
                        dest.content.highDefinition == false
                        && dest.content.standardDefinition == false
                        && dest.content.cx == false
                        && dest.content.nonCx == false)
                });
            }
            else if (content == "HD") {
                filteredDestinations = filteredDestinations.filter(function (dest) {
                    return dest.content != null && dest.content.highDefinition == true
                });
            }
            else if (content == "SD") {
                filteredDestinations = filteredDestinations.filter(function (dest) {
                    return dest.content != null && dest.content.standardDefinition == true
                });
            }
            else if (content == "C3") {
                filteredDestinations = filteredDestinations.filter(function (dest) {
                    return dest.content != null && dest.content.cx == true
                });
            }
            else if (content == "NonC3") {
                filteredDestinations = filteredDestinations.filter(function (dest) {
                    return dest.content != null && dest.content.nonCx == true
                });
            }

            return filteredDestinations;
        }
        else
            return destinations;
    };

    render() {
        if (this.props.user.portal == undefined) {
            return <div>Loading...</div>;
        }
        else if (!this.props.user.portal.modulePermissions.Destinations.canRead) {
            return <h3>Unauthorized to view this page</h3>;
        }

        var filteredDestinations = this.getFilterVal(this.props.destinations, this.state.filterValue);

        return (
            <div>
                <PageHeader pageName="Destinations" />
                <DestinationFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <DestinationTable RowData={filteredDestinations} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}




export default DestinationPage;


