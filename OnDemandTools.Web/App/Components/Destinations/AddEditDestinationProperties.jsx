import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';
import DestinationPropertiesFilter from 'Components/Destinations/DestinationPropertiesFilter';
import RemovePropertiesModal from 'Components/Destinations/RemovePropertiesModal';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';


// Sub component of destination page to  add ,edit and delete  destination properties
class AddEditDestinationProperties extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({
            destinationDetails: {},
            showAddEditPropertiesFilter: false,
            showPropertiesDeleteModal: false,
            isPropertyNameRequired: false,
            destinationPropertiesRow: {},
            propertyIndexToRemove: -1,
            propertyTitles: []
        });
    }

    /// <summary>
    // Invoked immediately when there is any change in props
    /// <summary>
    componentWillReceiveProps(nextProps) {
        this.setState({
            destinationDetails: nextProps.data
        });
    }

    componentDidMount() {
        this.setState({
            destinationDetails: this.props.data
        });
    }

    // to open properties filter window
    openPropertiesFilter(row) {
        this.setState({ showAddEditPropertiesFilter: true, destinationPropertiesRow: row });
    }

    //to close properties filter window
    closePropertiesFilter() {
        this.setState({ showAddEditPropertiesFilter: false });
    }

    //To delete  a property of destination
    RemovePropertiesModel(value) {
        var model = {};
        model = this.state.destinationDetails;
        model.properties.splice(value, 1);
        this.setState({ destinationDetails: model });
        this.props.updateDestination(model);
        this.setState({ showPropertiesDeleteModal: false });
        this.CheckPropertyNameIsEmpty();

    }

    //To add a new property of destination
    AddNewProperty() {
        var newProperty = { name: "", value: "", brands: [], titleIds: [], seriesIds: [], titles: [] }
        var model = {};
        model = this.state.destinationDetails;
        model.properties.unshift(newProperty);
        this.setState({ destinationDetails: model });

        this.CheckPropertyNameIsEmpty();
    }
    //To open delete property warning window
    openPropertiesDeleteModel(item) {
        this.setState({ showPropertiesDeleteModal: true, propertyIndexToRemove: item });
    }
    // To close property delete modal window
    closePropertiesDeleteModel() {
        this.setState({ showPropertiesDeleteModal: false });
    }

    // when name property is edited
    handlePropertyNameChange(event) {
        var model = this.state.destinationDetails;
        model.properties[event.target.id].name = event.target.value;
        this.setState({ destinationDetails: model });
        this.props.updateDestination(model);
        this.CheckPropertyNameIsEmpty();
    }

    // called when a property value is edited
    handlePropertyValueChange(event) {
        var model = this.state.destinationDetails;
        model.properties[event.target.id].value = event.target.value;
        this.setState({ destinationDetails: model });
        this.props.updateDestination(model);
    }

    // validation for the name property . To verify  name text is empty
    CheckPropertyNameIsEmpty() {
        var properties = this.state.destinationDetails.properties;
        if (properties.length > 0) {
            for (var i = 0; i <= properties.length - 1; i++) {
                if (!(properties[i].name)) {
                    this.props.validationStates(true);
                    return;
                }
            }
        }
        this.props.validationStates(false);
    }


    //To show all titles.
    titledetailConstruct(item, index) {
        var ids = [];

        for (var i = 0; i < item.seriesIds.length; i++) {
            ids.push(item.seriesIds[i]);
        }

        for (var i = 0; i < item.titleIds.length; i++) {
            ids.push(item.titleIds[i]);
        }

        if (ids.length > 0) {
            return <TitleNameOverlay disableOverlay={false} data={ids} />;
        }
    }

    //To construct brands.
    propertyBrandImageConstruct(item, index) {
        var brands = [];

        if (item.brands.length > 0)
            brands = item.brands;

        return <BrandsOverlay disableOverlay={false} data={brands} />;
    }
    // Tokens for the popover
    popoverValueClickRootClose(index) {
        return (<Popover id="popover-trigger-click-root-close" title="Subsitution Tokens">
            <span><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;AIRING_ID&#125;">&#123;AIRING_ID&#125;</button></span>
            <span> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;AIRING_NAME&#125;">&#123;AIRING_NAME&#125;</button></span>
            <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;BRAND&#125;">&#123;BRAND&#125;</button></div>
            <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;TITLE_EPISODE_NUMBER&#125;">&#123;TITLE_EPISODE_NUMBER&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_LONG&#125;">&#123;AIRING_STORYLINE_LONG&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_SHORT&#125;">&#123;AIRING_STORYLINE_SHORT&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;IFHD=(value)ELSE=(value)&#125;">&#123;IFHD=(value)ELSE=(value)&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.SubsitutionTokenClick(index, event)} value="&#123;TITLE_STORYLINE(type)&#125;">&#123;TITLE_STORYLINE(type)&#125;</button></div>
        </Popover>);
    }

    //To subsitute the token values  in the value text box
    SubsitutionTokenClick(index, event) {
        var model = this.state.destinationDetails;
        var oldValue = model.properties[index].value;
        model.properties[index].value = oldValue + event.target.value;
        this.setState({ destinationDetails: model });
        this.props.updateDestination(this.state.destinationDetails);
        var overlayIndex = "overlay" + index;

        //hide the overlay after selecting token
        this.refs[overlayIndex].hide();
    }

    hasProperties() {
        if (this.state.destinationDetails.properties == undefined) return false;

        return (this.state.destinationDetails.properties.length > 0);
    }

    //properties construct  of a destination
    render() {
        let row = null;
        if (this.hasProperties()) {
            row = this.state.destinationDetails.properties.map(function (item, index) {
                var nameValidation = item.name ? null : "error"
                var overlay = "overlay" + index
                return (<Row componentClass="tr" key={index.toString()}>
                    <Col componentClass="td" sm={3} >
                        <FormGroup controlId={index.toString()} validationState={nameValidation}>
                            <FormControl type="text" value={item.name} title={item.name} ref="Name" placeholder="Name" onChange={this.handlePropertyNameChange.bind(this)} />
                        </FormGroup></Col>
                    <Col componentClass="td" sm={3}> <OverlayTrigger trigger="click" ref={overlay} rootClose placement="left" overlay={this.popoverValueClickRootClose(index)}>
                        <FormGroup controlId={index.toString()} >
                            <FormControl type="text" value={item.value} title={item.value} ref="Value" placeholder="Value" onChange={this.handlePropertyValueChange.bind(this)} />
                        </FormGroup></OverlayTrigger></Col>
                    <Col componentClass="td"  >{this.propertyBrandImageConstruct(item, index)}</Col>
                    <Col componentClass="td"  >{this.titledetailConstruct(item, index)}</Col>
                    <Col componentClass="td">
                        <button type="button" class="btn-link" title="Add/Edit Filter" onClick={(event) => this.openPropertiesFilter(item, event)} ><i class="fa fa-filter"></i></button>
                        <button type="button" class="btn-link" title="Delete Property" onClick={(event) => this.openPropertiesDeleteModel(index, event)} ><i class="fa fa-trash"></i></button>
                    </Col>
                </Row>)
            }.bind(this));
        }

        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="Add New Destination" onClick={(event) => this.AddNewProperty(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New Property</span>
                    </button>
                </div>
                <div className="clearBoth">
                    <Grid componentClass="table" bsClass={this.hasProperties() ? "modalTable" : "hideModalTable"}>
                        <Row componentClass="tr">
                            <Col componentClass="th" row={0} sm={3} rowSpan={2} ><label >Name</label></Col>
                            <Col componentClass="th" row={0} sm={3} rowSpan={2} ><label >Value</label></Col>
                            <Col componentClass="th" row={0} colSpan={2} className="filterColumn" ><label >Filters</label></Col>
                            <Col componentClass="th" row={0} rowSpan={2} className="actionsColumn"><label>Actions</label></Col>
                        </Row>
                        <Row componentClass="tr">
                            <Col componentClass="th" className="brandsColumn"   ><label>Brands</label></Col>
                            <Col componentClass="th" className="titlesColumn"  ><label >Title/Series</label></Col>
                        </Row>
                        <tbody>
                            {row}
                        </tbody>
                    </Grid>
                </div>
                <DestinationPropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} />
                <RemovePropertiesModal data={this.state} handleClose={this.closePropertiesDeleteModel.bind(this)} handleRemoveAndClose={this.RemovePropertiesModel.bind(this)} />
            </div>)
    }
}

export default AddEditDestinationProperties