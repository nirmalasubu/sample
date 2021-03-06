import React from 'react';
import { connect } from 'react-redux';
import * as categoryActions from 'Actions/Category/CategoryActions';
import CategoryTable from 'Components/Categories/CategoryTable';
import CategoryFilter from 'Components/Categories/CategoryFilter';
import * as destinationActions from 'Actions/Destination/DestinationActions';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    return {
        categories: store.categories,
        user: store.user
    };
})
class CategoriesPage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            stateQueue: [],

            filterValue: {
                categoryName: "",
                destination: "",

            },

            columns: [{ "label": "Name", "dataField": "name", "sort": true },
            { "label": "Destinations", "dataField": "destinations", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "name"
        }
    }

    ///<summary>
    // Function to handle filtering of Category data. This handler
    // is used within the Category filter sub component. Filtering is currently
    // supported for Category name and destination
    /// </summary>
    handleFilterUpdate(filtersValue, type) {
        var stateFilterValue = this.state.filterValue;
        if (type == "CN")
            stateFilterValue.categoryName = filtersValue;

        if (type == "DS")
            stateFilterValue.destination = filtersValue;

        if (type == "CL") {
            stateFilterValue.categoryName = "";
            stateFilterValue.destination = "";
        }
        this.setState({
            filterValue: stateFilterValue
        });

    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(categoryActions.fetchCategories());
        this.props.dispatch(destinationActions.fetchDestinations());  // required to update the store with destinations for the Add/edit catagories destination dropdown
        document.title = "ODT - Categories";
    }

    ///<summary>
    // The goal of this function is to filter 'Categories' (which is stored in Redux store)
    // based on user provided filter criteria and return the refined 'categories' list.
    // If no filter criteria is provided then return the full 'categories' list
    ///</summary>
    getFilterVal(categories, filterVal) {
        if (filterVal.categoryName != undefined) {
            var categoryName = filterVal.categoryName.toLowerCase();
            var destination = filterVal.destination.toLowerCase();
            return (categories.filter(obj => (categoryName != "" ? obj.name.toLowerCase().indexOf(categoryName) != -1 : true)
                && (destination != "" ? matchDestinations(obj.destinations, destination) : true)
            ));
        }
        else
            return categories;
    };

    render() {
        if (this.props.user.portal == undefined) {
            return <div>Loading...</div>;
        }
        else if (!this.props.user.portal.modulePermissions.Categories.canRead) {
            return <h3>Unauthorized to view this page</h3>;
        }

        var filteredCategories = this.getFilterVal(this.props.categories, this.state.filterValue);

        return (
            <div>
                <PageHeader pageName="Categories" />
                <CategoryFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <CategoryTable RowData={filteredCategories} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}


///<summary>
// returns  true  if any of the search value  destination matches destination list 
///</summary>
const matchDestinations = (objDestinations, destination) => {
    var destinationNames = [];
    objDestinations.map(function (item) {
        destinationNames.push(item.name);
    });
    var flag = false;
    destinationNames.map(function (name) {
        if (name.toLowerCase().indexOf(destination) != -1) {
            flag = true;
        }
    });
    return flag;
};
export default CategoriesPage;