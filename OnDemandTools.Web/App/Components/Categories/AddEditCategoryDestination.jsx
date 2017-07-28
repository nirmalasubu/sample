import React from 'react';
import ReactDOM from 'react-dom';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well, Panel } from 'react-bootstrap';
import PropertiesFilter from 'Components/Common/PropertiesFilter';
import * as categoryActions from 'Actions/Category/CategoryActions';
import * as destinationActions from 'Actions/Destination/DestinationActions';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';
import Select from 'react-select';
import RemoveDestinationModal from 'Components/Categories/RemoveDestinationModal';
/// <summary>
//Get all destinations from store
/// </summary>
@connect((store) => {
    return {
        destinations: store.destinations
    };
})

/// <summary>
// Sub component of category page to  add ,edit and delete category destinations
/// </summary>
class AddEditCategoryDestination extends React.Component {

    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);
        this.state = ({
            categoryDetails: {},
            isCategoryNameRequired: false,
            propertiesRowRow: {},
            destinationIndexToRemove: -1,
            showDestinationsDeleteModal: false,
            destinationTitles: [],
            titleText: "",
            showAddEditPropertiesFilter: false,
            options: [],
            destinationValue: "",
            propertiesRowIndex: -1,
            destinations: []

        });
    }

    componentDidMount() {
        if (this.props.data.destinations.length > 0)
            this.props.data.destinations.sort(this.sortDestinationsByName);

        var destinationArray = destinationActions.getDestinations();
        destinationArray.then(message => {
            this.setState({ destinations: message, categoryDetails: this.props.data });

            if (this.props.data.destinations.length == 0) {
                this.addNewDestination();
            }
        })
        destinationArray.catch(error => {
            console.error(error);
            this.setState({ destinations: [], categoryDetails: this.props.data });
        });
    }

    /// <summary>
    /// To Bind dropdown with all destinations name and description
    /// </summary>
    getOptions(categoryDetails) {
        var options = [];
        for (var x = 0; x < this.state.destinations.length; x++) {

            var detailIndex = -1;

            if (categoryDetails.destinations.length > 0)
                detailIndex = categoryDetails.destinations.findIndex((obj => obj.name == this.state.destinations[x].name));

            if (detailIndex < 0) {
                var optionValue = { value: this.state.destinations[x].name, label: this.state.destinations[x].name + "-" + this.state.destinations[x].description };
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
    //This will sort your destinations array
    /// </summary>
    sortDestinationsByName(a, b) {
        var aName = a.name.toLowerCase();
        var bName = b.name.toLowerCase();
        return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
    }

    /// <summary>
    /// Handler to open properties at the indicated  row
    /// </summary>
    openPropertiesFilter(row, index) {
        this.setState({ showAddEditPropertiesFilter: true, propertiesRow: row.categories[0], propertiesRowIndex: index });
    }

    /// <summary>
    /// Handler to close properties modal pop up
    /// </summary>
    closePropertiesFilter() {
        this.setState({ showAddEditPropertiesFilter: false });
    }

    /// <summary>
    /// To delete  a destination of category
    /// </summary>
    removeDestinationModel(index, value) {
        var categoryData = [];
        categoryData = this.state.categoryDetails;
        if (categoryData.destinations[index].name == "") {
            categoryData.destinations.splice(index, 1);
        }
        else {
            var category = {
                id: categoryData.destinations[index].categories[0].id,
                name: "",
                brands: categoryData.destinations[index].categories[0].brands,
                titleIds: categoryData.destinations[index].categories[0].titleIds,
                seriesIds: categoryData.destinations[index].categories[0].seriesIds,
                removed: true
            };
            categoryData.destinations[index].categories.length = 0;
            categoryData.destinations[index].categories.push(category);
            this.setState({ showDestinationsDeleteModal: false });
        }
        var optionValues = this.getOptions(categoryData);
        this.setState({ options: optionValues });
    }

    /// <summary>
    //To add a new destination of category
    /// </summary>
    addNewDestination() {
        var optionValues = this.getOptions(this.state.categoryDetails);
        var newDestination = { name: "", description: "", categories: [] };
        var category = {
            name: this.state.categoryDetails.name,
            brands: [],
            titleIds: [],
            seriesIds: []
        };
        newDestination.categories.push(category);
        var categoryData = [];
        categoryData = this.state.categoryDetails;
        categoryData.destinations.unshift(newDestination);

        this.setState({ options: optionValues });

        //set the scroll to top
        let node = ReactDOM.findDOMNode(this.refs.categoryScroll);
        if (node) {
            panel.scrollTop = 0;
        }
    }

    /// <summary>
    //To open delete destination warning window
    /// </summary>
    openDestinationsDeleteModel(item) {
        this.setState({ showDestinationsDeleteModal: true, destinationIndexToRemove: item });
    }

    /// <summary>
    // To close destination delete modal window
    /// </summary>
    closeDestinationDeleteModel() {
        this.setState({ showDestinationsDeleteModal: false });
    }

    SavePropertiesFilterData(selectedFilterValues) {

        var model = this.state.categoryDetails;
        model.destinations[this.state.propertiesRowIndex].categories[0].brands = selectedFilterValues.brands;
        model.destinations[this.state.propertiesRowIndex].categories[0].titleIds = selectedFilterValues.titleIds;
        model.destinations[this.state.propertiesRowIndex].categories[0].seriesIds = selectedFilterValues.seriesIds;
        this.setState({ categoryDetails: model, showAddEditPropertiesFilter: false });

    }

    /// <summary>
    //this method to show all category titles.
    /// </summary>
    titleDetailConstruct(item, index) {

        var ids = [];
        if (item.categories.length > 0) {
            for (var i = 0; i < item.categories[0].seriesIds.length; i++) {
                ids.push(item.categories[0].seriesIds[i]);
            }

            for (var i = 0; i < item.categories[0].titleIds.length; i++) {
                ids.push(item.categories[0].titleIds[i]);
            }

            if (ids.length > 0) {
                return <TitleNameOverlay disableOverlay={item.categories[0].removed != undefined} data={ids} />;
            }
        }
    }

    /// <summary>
    //this method constructs the category brands images.
    /// </summary>
    categoryBrandImageConstruct(item, index) {
        var brands = [];

        if (item.categories.length > 0)
            brands = item.categories[0].brands;

        return <BrandsOverlay disableOverlay={item.categories[0].removed != undefined} data={brands} />;
    }

    /// <summary>
    //this method handle the change of the dropdown value.
    /// </summary>
    handleChange(index, value) {
        var model = this.state.categoryDetails;
        model.destinations[index].name = value;
        var detailIndex = this.state.destinations.findIndex((obj => obj.name == value));
        model.destinations[index].description = this.state.destinations[detailIndex].description;
        var optionValues = this.getOptions(model);
        this.setState({ categoryDetails: model, options: optionValues });
    }

    /// <summary>
    //destinations construct  of a category
    /// </summary>
    render() {
        let row = null;
        if (Object.keys(this.state.categoryDetails).length != 0 && this.state.categoryDetails != Object) {
            if (Object.keys(this.state.categoryDetails.destinations).length !== 0 && this.state.categoryDetails.destinations != Object) {
                row = this.state.categoryDetails.destinations.map(function (item, index) {
                    if (true) {
                        let col = null, colDesc = null;
                        if (item.name == "") {
                            col = (<Col componentClass="td" colSpan={2} sm={6}  >
                                <FormGroup controlId={index.toString()}>
                                    <Select
                                        searchable={true}
                                        simpleValue className="category-select-control"
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
                        return (<Row componentClass="tr" key={index} bsClass={item.categories[0].removed == undefined ? "row row-margin" : "row row-margin strikeout"}>
                            {col}
                            {colDesc}
                            <Col componentClass="td">{this.categoryBrandImageConstruct(item, index)}</Col>
                            <Col componentClass="td">{this.titleDetailConstruct(item, index)}</Col>
                            <Col componentClass="td">
                                <button disabled={item.categories[0].removed == undefined ? false : true} type="button" class="btn-link img-height" title="Add/Edit Filter" onClick={(event) => this.openPropertiesFilter(item, index, event)} ><i class="fa fa-filter"></i></button>
                                <button disabled={item.categories[0].removed == undefined ? false : true} type="button" class="btn-link img-height" title="Delete Destination" onClick={(event) => this.removeDestinationModel(index)} ><i class="fa fa-trash"></i></button>
                            </Col>

                        </Row>)
                    }
                }.bind(this));
            }
            else {
                row = <Row><Col sm={12}><p> No destination available</p></Col></Row>
            }
        }

        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="Add New Destination" onClick={(event) => this.addNewDestination(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New Destination</span>
                    </button>
                </div>
                <div className="clearBoth modalTableContainer" ref="categoryScroll">
                    <Grid componentClass="table" bsClass="modalTable">
                        <thead>
                            <Row componentClass="tr" >
                                <Col componentClass="th" rowSpan={2} sm={3} ><label>Destination</label></Col>
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
                <RemoveDestinationModal data={this.state} handleClose={this.closeDestinationDeleteModel.bind(this)} handleRemoveAndClose={this.removeDestinationModel.bind(this)} />
            </div>

        )
    }
}

export default AddEditCategoryDestination;