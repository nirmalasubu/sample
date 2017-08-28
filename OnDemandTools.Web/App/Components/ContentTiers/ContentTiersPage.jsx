import React from 'react';
import { connect } from 'react-redux';
import * as contentTierActions from 'Actions/ContentTier/ContentTierActions';
import * as productActions from 'Actions/Product/ProductActions';
import ContentTierTable from 'Components/ContentTiers/ContentTierTable';
import ContentTierFilter from 'Components/ContentTiers/ContentTierFilter';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {   
    return {
        contentTiers: store.contentTiers
    };
})
class ContentTiersPage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            stateQueue: [],

            filterValue: {
                contentTierName: "",
                product: "",
            },

            columns: [{ "label": "Name", "dataField": "name", "sort": true },
            { "label": "Products", "dataField": "products", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "name"
        }
    }

    ///<summary>
    // Function to handle filtering of ContentTier data. This handler
    // is used within the ContentTier filter sub component. Filtering is currently
    // supported for ContentTier name and product
    /// </summary>
    handleFilterUpdate(filtersValue, type) {
        var stateFilterValue = this.state.filterValue;
        if (type == "CN")
            stateFilterValue.contentTierName = filtersValue;

        if (type == "DS")
            stateFilterValue.product = filtersValue;

        if (type == "CL") {
            stateFilterValue.contentTierName = "";
            stateFilterValue.product = "";
        }

        this.setState({
            filterValue: stateFilterValue
        });

    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(contentTierActions.fetchContentTiers());
        this.props.dispatch(productActions.fetchProducts());
        document.title = "ODT - Content Tiers";
    }

    ///<summary>
    // The goal of this function is to filter 'ContentTiers' (which is stored in Redux store)
    // based on user provided filter criteria and return the refined 'contentTiers' list.
    // If no filter criteria is provided then return the full 'contentTiers' list
    ///</summary>
    getFilterVal(contentTiers, filterVal) {
        if (filterVal != undefined && filterVal.contentTierName != undefined) {
            var contentTierName = filterVal.contentTierName.toLowerCase();
            var product = filterVal.product.toLowerCase();
            return (contentTiers.filter(obj => (contentTierName != "" ? obj.name.toLowerCase().indexOf(contentTierName) != -1 : true)
                && (product != "" ? matchProducts(obj.products, product) : true)
            ));
        }
        else
            return contentTiers;
    };

    render() {
        var filteredContentTiers = this.getFilterVal(this.props.contentTiers, this.state.filterValue);
        return (
            <div>
                <PageHeader pageName="Content Tiers" />
                <ContentTierFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <ContentTierTable RowData={filteredContentTiers} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

///<summary>
// returns  true  if any of the search value  product matches product list 
///</summary>
const matchProducts = (objProducts, product) => {
    var productNames = [];
    objProducts.map(function (item) {
        productNames.push(item.name);
    });
    var flag = false;
    productNames.map(function (name) {
        if (name.toLowerCase().indexOf(product) != -1) {
            flag = true;
        }
    });
    return flag;
};
export default ContentTiersPage;