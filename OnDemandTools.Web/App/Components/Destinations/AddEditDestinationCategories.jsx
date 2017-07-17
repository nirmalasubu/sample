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
                    var nameValidation = item.name ? null : "error"
                    return (<Row key={item.id}>
                        <Form>
                            <Col sm={3} md={5} >
                                <FormGroup controlId="Name" validationState={nameValidation}>
                                    <FormControl type="text" value={item.name} ref="Name" placeholder="Name" disabled="true" />
                                </FormGroup></Col>
                        </Form>
                        <Col sm={2} md={3} >
                            {this.propertyBrandImageConstruct(item, index)}

                        </Col>
                        <Col sm={2} >
                            {this.titleDetailConstruct(item, index)}
                        </Col>
                    </Row>)
                }.bind(this));
            }
            else {
                row = <Row><Col sm={12} ><p> No Categories available</p></Col></Row>
            }
        }

        return (
            <div>
                <div >
                    <Grid fluid={true}>
                        <Row>
                            <Col sm={3} md={5} ><label class="destination-properties-label">Name</label></Col>
                            <Col sm={4} ><label class="destination-properties-label  destination-properties-filtermargin">Filter</label></Col>
                        </Row>
                        <div class="destination-height">
                            {row}
                        </div>
                    </Grid>
                </div>
            </div>
        )
    }
}

export default AddEditDestinationCategories