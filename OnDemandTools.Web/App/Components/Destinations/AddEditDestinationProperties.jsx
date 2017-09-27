import React from 'react';
import ReactDOM from 'react-dom';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';
import DestinationPropertiesFilter from 'Components/Destinations/DestinationPropertiesFilter';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';


// Sub component of destination page to  add ,edit and delete  destination properties
class AddEditDestinationProperties extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({
            destinationDetails: {},
            showAddEditPropertiesFilter: false,
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
    removeProperties(value) {
        var model = this.state.destinationDetails;
        model.properties[value].deleted = true;
        this.setState({ destinationDetails: model });
        this.props.updateDestination(model);
        this.setState({ showPropertiesDeleteModal: false });
        this.checkPropertyNameIsEmpty();
    }

    //To add a new property of destination
    addNewProperty() {
        var newProperty = { name: "", value: "", brands: [], titleIds: [], titles: [], deleted: false }
        var model = {};
        model = this.state.destinationDetails;
        model.properties.unshift(newProperty);
        this.setState({ destinationDetails: model });

        //Moves the scroll bar to top if there is too many contents
        let node = ReactDOM.findDOMNode(this.refs.destinationPropertiesContainer);
        if (node) {
            node.scrollTop = 0;
        }

        this.checkPropertyNameIsEmpty();
    }

    // when name property is edited
    handlePropertyNameChange(event) {
        var model = this.state.destinationDetails;
        model.properties[event.target.id].name = event.target.value;
        this.setState({ destinationDetails: model });
        this.props.updateDestination(model);
        this.checkPropertyNameIsEmpty();
    }

    // called when a property value is edited
    handlePropertyValueChange(event) {
        var model = this.state.destinationDetails;
        model.properties[event.target.id].value = event.target.value;
        this.setState({ destinationDetails: model });
        this.props.updateDestination(model);
    }

    // validation for the name property . To verify  name text is empty
    checkPropertyNameIsEmpty() {
        var properties = this.state.destinationDetails.properties;
        if (properties.length > 0) {
            for (var i = 0; i <= properties.length - 1; i++) {
                if (!properties[i].deleted) {
                    if (!(properties[i].name)) {
                        this.props.validationStates(true);
                        return;
                    }
                }
            }
        }
        this.props.validationStates(false);
    }


    //To show all titles.
    titledetailConstruct(item, index) {
        var ids = [];

        for (var i = 0; i < item.titleIds.length; i++) {
            ids.push(item.titleIds[i]);
        }

        if (ids.length > 0) {
            return <TitleNameOverlay disableOverlay={item.deleted} data={ids} />;
        }
    }

    //To construct brands.
    propertyBrandImageConstruct(item, index) {
        var brands = [];

        if (item.brands.length > 0)
            brands = item.brands;

        return <BrandsOverlay disableOverlay={item.deleted} data={brands} />;
    }
    // Tokens for the popover
    popoverValueClickRootClose(index) {
        return (<Popover id="popover-trigger-click-root-close" title="Subsitution Tokens">
            <span><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;AIRING_ID&#125;">&#123;AIRING_ID&#125;</button></span>
            <span> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;AIRING_NAME&#125;">&#123;AIRING_NAME&#125;</button></span>
            <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;BRAND&#125;">&#123;BRAND&#125;</button></div>
            <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;TITLE_EPISODE_NUMBER&#125;">&#123;TITLE_EPISODE_NUMBER&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_LONG&#125;">&#123;AIRING_STORYLINE_LONG&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_SHORT&#125;">&#123;AIRING_STORYLINE_SHORT&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;IFHD=(value)ELSE=(value)&#125;">&#123;IFHD=(value)ELSE=(value)&#125;</button></div>
            <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.subsitutionTokenClick(index, event)} value="&#123;TITLE_STORYLINE(type)&#125;">&#123;TITLE_STORYLINE(type)&#125;</button></div>
        </Popover>);
    }

    //To subsitute the token values  in the value text box
    subsitutionTokenClick(index, event) {
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

        if (!this.hasProperties() && this.props.permissions.disableControl) {
            return <div className="clearBoth">Properties not available</div>;
        }

        if (this.hasProperties()) {
            row = this.state.destinationDetails.properties.map(function (item, index) {
                var nameValidation = item.name ? null : "error"
                if (nameValidation == "error" && item.deleted) {
                    nameValidation = null;
                }
                var overlay = "overlay" + index

                let valueTextBox = null;

                if (item.deleted || this.props.permissions.disableControl) {
                    valueTextBox =
                        <FormGroup controlId={index.toString()} >
                            <FormControl type="text" disabled={true} value={item.value} title={item.value} ref="Value" placeholder="Value" />
                        </FormGroup>;
                }
                else {
                    valueTextBox =
                        <OverlayTrigger trigger="click" ref={overlay} rootClose placement="left" overlay={this.popoverValueClickRootClose(index)}>
                            <FormGroup controlId={index.toString()} >
                                <FormControl type="text" disabled={item.deleted} value={item.value} title={item.value} ref="Value" placeholder="Value" onChange={this.handlePropertyValueChange.bind(this)} />
                            </FormGroup>
                        </OverlayTrigger>;

                }

                var actionsCol = null;

                if (this.props.permissions.canAddOrEdit) {
                    actionsCol = <Col componentClass="td">
                        <button type="button" disabled={item.deleted} class="btn-link" title="Add/Edit Filter" onClick={(event) => this.openPropertiesFilter(item, event)} ><i class="fa fa-filter"></i></button>
                        <button type="button" disabled={item.deleted} class="btn-link" title="Delete Property" onClick={(event) => this.removeProperties(index, event)} ><i class="fa fa-trash"></i></button>
                    </Col>;
                }

                return (<Row componentClass="tr" key={index.toString()} className={item.deleted ? "strikeout" : ""}>
                    <Col componentClass="td" sm={3} >
                        <FormGroup controlId={index.toString()} validationState={nameValidation}>
                            <FormControl type="text" disabled={this.props.permissions.disableControl || item.deleted} value={item.name} title={item.name} ref="Name" placeholder="Name" onChange={this.handlePropertyNameChange.bind(this)} />
                        </FormGroup></Col>
                    <Col componentClass="td" sm={3}>
                        {valueTextBox}
                    </Col>
                    <Col componentClass="td"  >{this.propertyBrandImageConstruct(item, index)}</Col>
                    <Col componentClass="td"  >{this.titledetailConstruct(item, index)}</Col>
                    {actionsCol}
                </Row>)
            }.bind(this));
        }

        var actionHeader = null;
        var addNewPropertyControl = null;
        if (this.props.permissions.canAddOrEdit) {
            actionHeader = <Col componentClass="th" rowSpan={2} className="actionsColumn"><label>Actions</label></Col>;

            addNewPropertyControl = <div>
                <button class="btn-link pull-right addMarginRight" title="Add New Property" onClick={(event) => this.addNewProperty(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New Property</span>
                </button>;
        </div>
        }


        return (
            <div>
                {addNewPropertyControl}
                <div className="clearBoth modalTableContainer" ref="destinationPropertiesContainer">
                    <Grid componentClass="table" bsClass={this.hasProperties() ? "modalTable" : "hideModalTable"}>
                        <thead>
                            <Row componentClass="tr">
                                <Col componentClass="th" sm={3} rowSpan={2} ><label >Name</label></Col>
                                <Col componentClass="th" sm={3} rowSpan={2} ><label >Value</label></Col>
                                <Col componentClass="th" colSpan={2} className="filterColumn" ><label >Filters</label></Col>
                                {actionHeader}
                            </Row>
                            <Row componentClass="tr">
                                <Col componentClass="th" className="brandsColumn"   ><label>Brands</label></Col>
                                <Col componentClass="th" className="titlesColumn"  ><label >Title/Series</label></Col>
                            </Row>
                        </thead>
                        <tbody>
                            {row}
                        </tbody>
                    </Grid>
                </div>
                <DestinationPropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} />
            </div>)
    }
}

export default AddEditDestinationProperties