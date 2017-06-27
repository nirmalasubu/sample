import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';


class AddEditDestinationCategories extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({
            destinationDetails: {},
            isPropertyNameRequired: false,
            titleopen: [],
            imageopen: []
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
    TitledetailConstruct(item, index) {

        var titleName = [];
        item.titles.map(function (title, index) { titleName.push(title.name) });
        var titletext = titleName.toString();
        var title = (<p class="destination-label-title" title="title/series">{titletext}</p>);
        return title;
    }

    //To construct brands. 
    CategoryBrandImageConstruct(item, index) {
        if (item.brands.length >= 1 && item.brands.length <= 4) {
            var image = [];
            item.brands.map(function (name, index) {
                var path = "images/brands/" + name + ".gif";
                image.push(<div class="destination-container"><img src={path} title={name} alt={name} /></div>)
            });

            var tag = (<div>{image}</div>);
            return tag;
        }

        if (item.brands.length >= 4) {
            var image = [];
            item.brands.map(function (name, index) {
                var path = "images/brands/" + name + ".gif";
                image.push(<div class="destination-container"><img src={path} title={name} alt={name} /></div>)
                if (index == 1) {
                    image.push(<div class="destination-container"><button class="btn-link destination-img-btn" type="button"> <i class="fa fa-ellipsis-h" /></button></div>)
                }
            });

            var tag = (<div class="destination-img">{image}</div>);
            return tag;
        }
    }

    render() {
        let row = null;

        if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object) {
            if (Object.keys(this.state.destinationDetails.categories).length !== 0 && this.state.destinationDetails.categories != Object) {
                row = this.state.destinationDetails.categories.map(function (item, index) {
                    var nameValidation = item.name ? "" : "error"
                    return (<Row >
                        <Form>
                            <Col sm={3} md={5} >
                                <FormGroup controlId="Name" validationState={nameValidation}>
                                    <FormControl type="text" id={index} value={item.name} ref="Name" placeholder="Name" disabled="true" />
                                </FormGroup></Col>
                        </Form>
                        <Col sm={2} md={3} >
                            {this.CategoryBrandImageConstruct(item, index)}

                        </Col>
                        <Col sm={2} >
                            {this.TitledetailConstruct(item, index)}

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