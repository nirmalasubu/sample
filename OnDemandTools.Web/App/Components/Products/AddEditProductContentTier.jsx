import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';

class AddEditProductContentTier extends React.Component {
    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = ({
            productDetails: {}
        });
    }

    /// <summary>
    /// receives prop changes to update state
    /// </summary>
    componentWillReceiveProps(nextProps) {
        this.setState({
            productDetails: nextProps.data
        });
    }

    /// <summary>
    /// This method sets the state with the product details before the component gets mounted
    /// </summary> 
    componentWillMount() {
        this.setState({
            productDetails: this.props.data
        });
    }

    ///<summary>
    /// To show all titles.
    ///</summary>
    titleDetailConstruct(item, index) {
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

    ///<summary>
    /// To construct brands.
    ///</summary>
    brandImageConstruct(item, index) {
        var brands = [];

        if (item.brands.length > 0)
            brands = item.brands;

        return <BrandsOverlay disableOverlay={false} data={brands} />;
    }

    render() {

        if (this.state.productDetails.id == undefined || this.state.productDetails.id == null) {
            return <div className="clearBoth"> <table><tbody><tr><td><p>Content-Tiers can be added after the Product has been created.</p></td></tr></tbody></table></div>;
        }

        let row = null;

        if (this.state.productDetails.contentTiers.length > 0) {
            row = this.state.productDetails.contentTiers.map(function (item, index) {
                return (<Row componentClass="tr" key={item.id}>
                    <Col componentClass="td">
                        <p>  {item.name} </p>
                    </Col>
                    <Col componentClass="td">
                        {this.brandImageConstruct(item, index)}
                    </Col>
                    <Col componentClass="td" >
                        {this.titleDetailConstruct(item, index)}
                    </Col>
                </Row>)
            }.bind(this));
        }
        else {
            return <div className="clearBoth"> <table><tbody><tr><td><p>No Content-Tiers available</p></td></tr></tbody></table></div>;            
        }

        return (
            <div className="clearBoth">
                <Grid componentClass="table" bsClass="modalTable">
                    <thead>
                        <Row componentClass="tr">
                            <Col componentClass="th" rowSpan={2}><label >Name</label></Col>
                            <Col componentClass="th" colSpan={2} className="filterColumn" ><label >Filters</label></Col>
                        </Row>
                        <Row componentClass="tr">
                            <Col componentClass="th" className="brandsColumn"  ><label>Brands</label></Col>
                            <Col componentClass="th"  ><label >Title/Series</label></Col>
                        </Row>
                    </thead>
                    <tbody>
                        {row}
                    </tbody>
                </Grid>
            </div>
        )
    }
}

export default AddEditProductContentTier