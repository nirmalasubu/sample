import React from 'react';
import { connect } from 'react-redux';
import * as productActions from 'Actions/Product/ProductActions';
import ProductTable from 'Components/Products/ProductTable';
import CategoryFilter from 'Components/Categories/CategoryFilter';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    ///<summary>
    /// Retrieve filtered list of products based on filterValue like product name, description, tags and destination
    /// which are defined in Redux store
    ///</summary>
    //var filteredCategoryValues = getFilterVal(store.categories, store.filterCategory);
    return {
        products: store.products
        //filteredCategories: (filteredCategoryValues!=undefined?filteredCategoryValues:store.destinations)
    };
})
class ProductsPage extends React.Component {
    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
       
        this.state = {
            stateQueue: [],

            filterValue: {
                productName: "",
                Description: "",
                tag: ""
            },

            columns: [{ "label": "Product", "dataField": "name", "sort": true },
            { "label": "Description", "dataField": "description", "sort": false },
            { "label": "Destinations", "dataField": "destinations", "sort": false },
            { "label": "Tags", "dataField": "tags", "sort": false },
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
        //var stateFilterValue = this.state.filterValue;
        //if (type == "CN")
        //    stateFilterValue.categoryName = filtersValue;

        //if (type == "DS")
        //    stateFilterValue.destination = filtersValue;

        //if (type == "CL") {
        //    stateFilterValue.categoryName = "";
        //    stateFilterValue.destination = "";
        //}
        //this.setState({
        //    filterValue: stateFilterValue
        //});
        //this.props.dispatch(categoryActions.filterCategorySuccess(this.state.filterValue));  
    }

    //called on the page load
    componentDidMount() {
        //this.props.dispatch(categoryActions.filterCategorySuccess(this.state.filterValue));
        this.props.dispatch(productActions.fetchProducts());

        document.title = "ODT - Products";
    }

    render() {
        return (
            <div>               
                <PageHeader pageName="Products" />
                <ProductTable RowData={this.props.products} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

///<summary>
// The goal of this function is to filter 'Categories' (which is stored in Redux store)
// based on user provided filter criteria and return the refined 'categories' list.
// If no filter criteria is provided then return the full 'categories' list
///</summary>
const getFilterVal = (categories, filterVal) => {
    //if(filterVal.categoryName!=undefined)
    //{
    //    var categoryName = filterVal.categoryName.toLowerCase();
    //    var destination = filterVal.destination.toLowerCase();
    //    return (categories.filter(obj=> (categoryName != "" ? obj.name.toLowerCase().indexOf(categoryName) != -1 : true)
    //          &&(destination!=""?matchDestinations(obj.destinations,destination) :true)  
    //        ));
    //}
    //else
    //    return categories;
};

///<summary>
// returns  true  if any of the search value  destination matches destination list 
///</summary>
const matchDestinations = (objDestinations,destination) => {
    //var destinationNames=[];
    //objDestinations.map(function(item){
    //    destinationNames.push(item.name);
    //});
    //var flag= false;
    //destinationNames.map(function(name){
    //    if(name.toLowerCase().indexOf(destination)!=-1)
    //    {
    //        flag=true;
    //    }
    //});
    //return flag;
};
export default ProductsPage;