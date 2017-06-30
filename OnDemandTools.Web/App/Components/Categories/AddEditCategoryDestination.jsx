import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover,Collapse,Well } from 'react-bootstrap';
import PropertiesFilter from 'Components/Common/PropertiesFilter';

// Sub component of category page to  add ,edit and delete category destinations
class AddEditCategoryDestination extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({
            categoryDetails:"",
            isCategoryNameRequired:false,
            propertiesRow: {},
            destinationIndexToRemove:-1,
            showDestinationsDeleteModal:false,
            destinationTitles:[],
            showAddEditPropertiesFilter: false,
        });
    }

    /// <summary>
    /// Handler to open properties at the indicated  row
    /// </summary>
    openPropertiesFilter(row) {
        this.setState({ showAddEditPropertiesFilter: true,propertiesRow: row.categories[0] });
    }

    /// <summary>
    /// Handler to close properties modal pop up
    /// </summary
    closePropertiesFilter() {
        this.setState({ showAddEditPropertiesFilter: false });
    }

    //To delete  a destination of category
    RemoveDestinationModel(value)
    {
        var categoryData=[];
        categoryData = this.props.data;
        categoryData.destinations.splice(value, 1);
        this.setState({categoryDetails: categoryData });
        this.props.data = this.props.data;
        this.setState({ showDestinationsDeleteModal: false});
    }

    //To add a new destination of category
    AddNewdestination()
    {
        var newDestination={name:"",brands:[],titleIds:[],seriesIds:[]}
        var categoryData=[];
        categoryData = this.state.categoryDetails;  
        categoryData.destinations.unshift(newDestination);
        this.setState({destinationDetails: categoryData });
    }
    //To open delete destination warning window
    openDestinationsDeleteModel(item) {
        this.setState({ showDestinationsDeleteModal: true, destinationIndexToRemove: item });
    }
    // To close destination delete modal window
    closePropertiesDeleteModel() {
        this.setState({ showDestinationsDeleteModal: false });
    }

    // validation for the name destination . To verify  name text is empty
    CheckDestinationNameIsEmpty()
    {
        var destinations=this.state.categoryDetails.destinations;
        if(destinations.length>0)
        {
            for(var i=0;i<=destinations.length-1;i++)
            {
                if(!(destinations[i].name))
                {
                    //this.props.validationStates(true);
                    //return;
                }
            }
        }
        //this.props.validationStates(false);
    }

    //destinations construct  of a category
    render() {
        let row = null;
        if (Object.keys(this.props.data).length != 0 && this.props.data != Object) {
            if(Object.keys(this.props.data.destinations).length !== 0 && this.props.data.destinations != Object){
                row = this.props.data.destinations.map(function (item, index) {
                    return (<Row >
                        <Form>
                            <Col sm={3} >
                                <FormGroup controlId={index} >
                                <FormControl type="text" value={item.name} title={item.name} ref="Name"  placeholder="Name" />
                                </FormGroup>
                            </Col>
                            <Col sm={3}>
                                <FormGroup controlId={index} >
                                <FormControl type="text" value={item.description} title={item.description} ref="Value"  placeholder="Description" />
                                </FormGroup>
                            </Col>
                            <Col sm={2} ></Col>
                            <Col sm={2} ></Col>
                            <Col sm={2} >
                                <button type= "button"  class="btn-link" title="Edit Filter" onClick={(event) => this.openPropertiesFilter(item, event)} ><i class="fa fa-pencil-square-o"></i></button>
                                <button type= "button"  class="btn-link" title="Delete Property" onClick={(event) => this.openPropertiesDeleteModel(index, event)} ><i class="fa fa-trash"></i></button>
                            </Col>
                        </Form>

                        </Row>)}.bind(this));
                        }
                        else
                        {
                            row =<Row><Col sm={12} ><p> No destination available</p></Col></Row>
                        }
            }

 return (
 <div>
     <div>
         <button class="destination-properties-addnew btn" title="Add New" onClick={(event) => this.AddNewDestination(event)}>New Destination</button>
    </div>
     <div >
    <Grid fluid={true}>
      <Row>
       <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
       <Col sm={3} ><label class="destination-properties-label">Description</label></Col>
       <Col sm={4} ><label class="destination-properties-label  destination-properties-filtermargin">Filter</label></Col>
       <Col sm={2} ><label class="destination-properties-label destination-properties-actionmargin">Actions</label></Col>
      </Row>
      <div class="destination-height">{row}</div>
      </Grid>
      </div>
       <PropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} />
    </div>)
      }
          }

  export default AddEditCategoryDestination;