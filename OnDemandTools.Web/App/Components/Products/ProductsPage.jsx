import React from 'react';
import { connect } from 'react-redux';
import * as productActions from 'Actions/Product/ProductActions';
import ProductTable from 'Components/Products/ProductTable';
import ProductFilter from 'Components/Products/ProductFilter';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    return {
        products: store.products,
        user: store.user
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
                description: "",
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
    // Function to handle filtering of product data. This handler
    // is used within the product filter sub component. Filtering is currently
    // supported for product name, tags and description.
    /// </summary>
    handleFilterUpdate(filtersValue, type) {
        var stateFilterValue = this.state.filterValue;
        if (type == "PN")
            stateFilterValue.productName = filtersValue;

        if (type == "DS")
            stateFilterValue.description = filtersValue;

        if (type == "TG")
            stateFilterValue.tag = filtersValue;

        if (type == "CL") {
            stateFilterValue.productName = "";
            stateFilterValue.description = "";
            stateFilterValue.tag = "";
        }
        this.setState({
            filterValue: stateFilterValue
        });
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(productActions.fetchProducts());

        document.title = "ODT - Products";
    }

    // The goal of this function is to filter 'products' (which is stored in Redux store)
    // based on user provided filter criteria and return the refined 'product' list.
    // If no filter criteria is provided then return the full 'product' list
    applyFilter(products, filterVal) {
        if(filterVal.productName!=undefined)
        {
            var productName = filterVal.productName.toLowerCase();
            var description = filterVal.description.toLowerCase();
            var tag = filterVal.tag.toLowerCase();
            return (products.filter(obj=> (productName != "" ? obj.name.toLowerCase().indexOf(productName) != -1 : true)
                  &&(description != "" ? obj.description.toLowerCase().indexOf(description) != -1 : true)
                  &&(tag!="" ? this.matchTags(obj.tags,tag) : true)  
                ));
        }
        else
            return products;
    }

    ///<summary>
    // returns  true  if any of the search value tag matches tags list 
    ///</summary>
    matchTags(objTags,tag){
        var tagTexts=[];
        objTags.map(function(item){
            tagTexts.push(item.name);
        });
        var flag= false;
        tagTexts.map(function(text){
            if(text.toLowerCase().indexOf(tag)!=-1)
            {
                flag=true;
            }
        });
        return flag;
    }

    render() {
        if (this.props.user.portal == undefined) {
            return <div>Loading...</div>;
        }
        else if (!this.props.user.portal.modulePermissions.Products.canRead) {
            return <h3>Unauthorized to view this page</h3>;
        }

        var filteredRows = this.applyFilter(this.props.products, this.state.filterValue);

        return (
            <div>               
                <PageHeader pageName="Products" />
                <ProductFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <ProductTable RowData={filteredRows} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

export default ProductsPage;