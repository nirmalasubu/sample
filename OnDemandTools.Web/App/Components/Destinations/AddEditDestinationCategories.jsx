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

    render() {
        let row = null;

        if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object) {
            if (Object.keys(this.state.destinationDetails.categories).length !== 0 && this.state.destinationDetails.categories != Object) {
                row = this.state.destinationDetails.categories.map(function (item, index) {
                    return (<Row componentClass="tr" key={item.id}>
                        <Col componentClass="td" sm={3} md={5} >
                            <p>  {item.name} </p>
                        </Col>
                        <Col componentClass="td" sm={2} md={3} >
                            {this.propertyBrandImageConstruct(item, index)}
                        </Col>
                        <Col componentClass="td" sm={2} >
                            {this.titleDetailConstruct(item, index)}
                        </Col>
                    </Row>)
                }.bind(this));
            }
            else {
                row = <Row componentClass="tr"><Col componentClass="td" colSpan={3} sm={12} ><p> No Categories available</p></Col></Row>
            }
        }

        return (
            <div className="clearBoth">
                <Grid componentClass="table" bsClass="modalTable">
                    <Row componentClass="tr">
                        <Col componentClass="th" row={0} sm={3} rowSpan={2} ><label >Name</label></Col>
                        <Col componentClass="th" row={0} colSpan={2} sm={4} ><label >Filters</label></Col>
                    </Row>
                    <Row componentClass="tr">
                        <Col componentClass="th" sm={4} ><label>Brands</label></Col>
                        <Col componentClass="th" sm={4} ><label >Title/Series</label></Col>
                    </Row>
                    {row}
                </Grid>
            </div>
        )
    }
}

export default AddEditDestinationCategories