import React from 'react';
import ReactDOM from 'react-dom';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well, Panel } from 'react-bootstrap';
import PropertiesFilter from 'Components/Common/PropertiesFilter';
import * as contentTierActions from 'Actions/ContentTier/ContentTierActions';
import * as productActions from 'Actions/Product/ProductActions';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';
import Select from 'react-select';
import RemoveProductModal from 'Components/ContentTiers/RemoveProductModal';
/// <summary>
//Get all products from store
/// </summary>
@connect((store) => {
    return {
        products: store.products
    };
})

/// <summary>
// Sub component of contentTier page to  add ,edit and delete contentTier products
/// </summary>
class AddEditContentTierProduct extends React.Component {

    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);
        this.state = ({
            contentTierDetails: {},
            isContentTierNameRequired: false,
            propertiesRowRow: {},
            productIndexToRemove: -1,
            showProductsDeleteModal: false,
            productTitles: [],
            titleText: "",
            showAddEditPropertiesFilter: false,
            options: [],
            productValue: "",
            propertiesRowIndex: -1,
            products: []
        });
    }

    componentDidMount() {
        if (this.props.data.products.length > 0)
            this.props.data.products.sort(this.sortProductsByName);

        this.setState({ contentTierDetails: this.props.data }, function () {
            if (this.props.data.products.length == 0) {
                this.addNewProduct();
            }
        });
    }

    /// <summary>
    /// To Bind dropdown with all products name and description
    /// </summary>
    getOptions(contentTierDetails) {
        var options = [];
        for (var x = 0; x < this.props.products.length; x++) {

            var detailIndex = -1;

            if (contentTierDetails.products.length > 0)
                detailIndex = contentTierDetails.products.findIndex((obj => obj.name == this.props.products[x].name));

            if (detailIndex < 0) {
                var optionValue = { value: this.props.products[x].name, label: this.props.products[x].name + "-" + this.props.products[x].description };
                options.push(optionValue);
            }
        }

        if (options.length > 0)
            options.sort(this.sortOptionsByName);

        return options;
    }
    /// <summary>
    //This will sort your options array
    /// </summary>
    sortOptionsByName(a, b) {
        var aName = a.value.toLowerCase();
        var bName = b.value.toLowerCase();
        return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
    }

    /// <summary>
    //This will sort your products array
    /// </summary>
    sortProductsByName(a, b) {
        var aName = a.name.toLowerCase();
        var bName = b.name.toLowerCase();
        return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
    }

    /// <summary>
    /// Handler to open properties at the indicated  row
    /// </summary>
    openPropertiesFilter(row, index) {
        this.setState({ showAddEditPropertiesFilter: true, propertiesRow: row.contentTiers[0], propertiesRowIndex: index });
    }

    /// <summary>
    /// Handler to close properties modal pop up
    /// </summary>
    closePropertiesFilter() {
        this.setState({ showAddEditPropertiesFilter: false });
    }

    /// <summary>
    /// To delete  a product of contentTier
    /// </summary>
    removeProductModel(index, value) {
        var contentTierData = [];
        contentTierData = this.state.contentTierDetails;
        if (contentTierData.products[index].name == "") {
            contentTierData.products.splice(index, 1);
        }
        else {
            var contentTier = {
                id: contentTierData.products[index].contentTiers[0].id,
                name: "",
                brands: contentTierData.products[index].contentTiers[0].brands,
                titleIds: contentTierData.products[index].contentTiers[0].titleIds,
                seriesIds: contentTierData.products[index].contentTiers[0].seriesIds,
                removed: true
            };
            contentTierData.products[index].contentTiers.length = 0;
            contentTierData.products[index].contentTiers.push(contentTier);
            this.setState({ showProductsDeleteModal: false });
        }
        var optionValues = this.getOptions(contentTierData);
        this.setState({ options: optionValues });
    }

    /// <summary>
    //To add a new product of contentTier
    /// </summary>
    addNewProduct() {
        var optionValues = this.getOptions(this.state.contentTierDetails);
        var newProduct = { name: "", description: "", contentTiers: [] };
        var contentTier = {
            name: this.state.contentTierDetails.name,
            brands: [],
            titleIds: [],
            seriesIds: []
        };
        newProduct.contentTiers.push(contentTier);
        var contentTierData = [];
        contentTierData = this.state.contentTierDetails;
        contentTierData.products.unshift(newProduct);

        this.setState({ options: optionValues });

        //set the scroll to top
        let node = ReactDOM.findDOMNode(this.refs.contentTierScroll);
        if (node) {
            node.scrollTop = 0;
        }
    }

    /// <summary>
    //To open delete product warning window
    /// </summary>
    openProductsDeleteModel(item) {
        this.setState({ showProductsDeleteModal: true, productIndexToRemove: item });
    }

    /// <summary>
    // To close product delete modal window
    /// </summary>
    closeProductDeleteModel() {
        this.setState({ showProductsDeleteModal: false });
    }

    SavePropertiesFilterData(selectedFilterValues) {

        var model = this.state.contentTierDetails;
        model.products[this.state.propertiesRowIndex].contentTiers[0].brands = selectedFilterValues.brands;
        model.products[this.state.propertiesRowIndex].contentTiers[0].titleIds = selectedFilterValues.titleIds;
        model.products[this.state.propertiesRowIndex].contentTiers[0].seriesIds = selectedFilterValues.seriesIds;
        this.setState({ contentTierDetails: model, showAddEditPropertiesFilter: false });

    }

    /// <summary>
    //this method to show all contentTier titles.
    /// </summary>
    titleDetailConstruct(item, index) {

        var ids = [];
        if (item.contentTiers.length > 0) {
            for (var i = 0; i < item.contentTiers[0].seriesIds.length; i++) {
                ids.push(item.contentTiers[0].seriesIds[i]);
            }

            for (var i = 0; i < item.contentTiers[0].titleIds.length; i++) {
                ids.push(item.contentTiers[0].titleIds[i]);
            }

            if (ids.length > 0) {
                return <TitleNameOverlay disableOverlay={item.contentTiers[0].removed != undefined} data={ids} />;
            }
        }
    }

    /// <summary>
    //this method constructs the contentTier brands images.
    /// </summary>
    contentTierBrandImageConstruct(item, index) {
        var brands = [];

        if (item.contentTiers.length > 0)
            brands = item.contentTiers[0].brands;

        return <BrandsOverlay disableOverlay={item.contentTiers[0].removed != undefined} data={brands} />;
    }

    /// <summary>
    //this method handle the change of the dropdown value.
    /// </summary>
    handleChange(index, value) {
        var model = this.state.contentTierDetails;
        model.products[index].name = value;
        var detailIndex = this.props.products.findIndex((obj => obj.name == value));
        model.products[index].description = this.props.products[detailIndex].description;
        var optionValues = this.getOptions(model);
        this.setState({ contentTierDetails: model, options: optionValues });
    }

    /// <summary>
    //products construct  of a contentTier
    /// </summary>
    render() {
        let row = null;
        if (Object.keys(this.state.contentTierDetails).length != 0 && this.state.contentTierDetails != Object) {
            if (Object.keys(this.state.contentTierDetails.products).length !== 0 && this.state.contentTierDetails.products != Object) {
                row = this.state.contentTierDetails.products.map(function (item, index) {
                    if (true) {
                        let col = null, colDesc = null;
                        if (item.name == "") {
                            col = (<Col componentClass="td" colSpan={2} sm={6}  >
                                <FormGroup controlId={index.toString()}>
                                    <Select
                                        searchable={true}
                                        simpleValue className="contentTier-select-control"
                                        options={this.state.options}
                                        onChange={(event) => this.handleChange(index, event)}
                                        value={item.name} />
                                </FormGroup>
                            </Col>
                            );
                        }
                        else {
                            col = (
                                <Col componentClass="td" bsClass="col-height col" sm={3}>
                                    <p>  {item.name} </p>
                                </Col>
                            );
                            colDesc = (
                                <Col componentClass="td" bsClass="col-height col" sm={3} >
                                    <p>  {item.description} </p>
                                </Col>
                            );
                        }
                        return (<Row componentClass="tr" key={index} bsClass={item.contentTiers[0].removed == undefined ? "row row-margin" : "row row-margin strikeout"}>
                            {col}
                            {colDesc}
                            <Col componentClass="td">{this.contentTierBrandImageConstruct(item, index)}</Col>
                            <Col componentClass="td">{this.titleDetailConstruct(item, index)}</Col>
                            <Col componentClass="td">
                                <button disabled={item.contentTiers[0].removed == undefined ? false : true} type="button" class="btn-link img-height" title="Add/Edit Filter" onClick={(event) => this.openPropertiesFilter(item, index, event)} ><i class="fa fa-filter"></i></button>
                                <button disabled={item.contentTiers[0].removed == undefined ? false : true} type="button" class="btn-link img-height" title="Delete Product" onClick={(event) => this.removeProductModel(index)} ><i class="fa fa-trash"></i></button>
                            </Col>

                        </Row>)
                    }
                }.bind(this));
            }
            else {
                row = <Row><Col sm={12}><p> No product available</p></Col></Row>
            }
        }

        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="Add New Product" onClick={(event) => this.addNewProduct(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New Product</span>
                    </button>
                </div>
                <div className="clearBoth modalTableContainer" ref="contentTierScroll">
                    <Grid componentClass="table" bsClass="modalTable">
                        <thead>
                            <Row componentClass="tr" >
                                <Col componentClass="th" rowSpan={2} sm={3} ><label>Product</label></Col>
                                <Col componentClass="th" rowSpan={2} sm={3} ><label>Description</label></Col>
                                <Col componentClass="th" colSpan={2} className="filterColumn"  ><label>Filters</label></Col>
                                <Col componentClass="th" rowSpan={2} className="actionsColumn" ><label>Actions</label></Col>
                            </Row>
                            <Row componentClass="tr">
                                <Col componentClass="th" className="brandsColumn" ><label>Brands</label></Col>
                                <Col componentClass="th"  ><label>Title/Series</label></Col>
                            </Row>
                        </thead>
                        <tbody>
                            {row}
                        </tbody>
                    </Grid>
                </div>
                <PropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} handleSave={this.SavePropertiesFilterData.bind(this)} />
                <RemoveProductModal data={this.state} handleClose={this.closeProductDeleteModel.bind(this)} handleRemoveAndClose={this.removeProductModel.bind(this)} />
            </div>

        )
    }
}

export default AddEditContentTierProduct;