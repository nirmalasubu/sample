import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';

class AddEditDestinationCategories extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({
            destinationDetails: {},
            isPropertyNameRequired: false
        });
    }

    //receives prop changes to update state
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

    componentWillMount() {
        this.setState({
            destinationDetails: this.props.data
        });
    }

    //To show all titles.
    titleDetailConstruct(item, index) {
        var ids = [];

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

    render() {

        if (this.state.destinationDetails.id == undefined || this.state.destinationDetails.id == null) {
            return <div className="clearBoth noRecordsText">Categories can be added after the Destination has been created.</div>;
        }

        let row = null;

        if (this.state.destinationDetails.categories.length > 0) {
            row = this.state.destinationDetails.categories.map(function (item, index) {
                return (<Row componentClass="tr" key={item.id}>
                    <Col componentClass="td">
                        <p>  {item.name} </p>
                    </Col>
                    <Col componentClass="td">
                        {this.propertyBrandImageConstruct(item, index)}
                    </Col>
                    <Col componentClass="td" >
                        {this.titleDetailConstruct(item, index)}
                    </Col>
                </Row>)
            }.bind(this));
        }
        else {
            return <div className="clearBoth noRecordsText">Categories not available.</div>;            
        }

        return (
            <div className="clearBoth modalTableContainer">
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

export default AddEditDestinationCategories