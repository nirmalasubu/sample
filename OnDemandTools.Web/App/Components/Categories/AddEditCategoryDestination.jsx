import React from 'react';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover,Collapse,Well } from 'react-bootstrap';
import PropertiesFilter from 'Components/Common/PropertiesFilter';
import * as categoryActions from 'Actions/Category/CategoryActions';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';
import Select from 'react-select';
import RemoveDestinationModal from 'Components/Categories/RemoveDestinationModal';

//Get all destinations from store
@connect((store) => {
    return {
        destinations: store.destinations
    };
})

// Sub component of category page to  add ,edit and delete category destinations
class AddEditCategoryDestination extends React.Component {
    
    // Define default component state information. This will
    // get modified further based on how the user interacts with it
    constructor(props) {
        super(props);
        this.state = ({
            categoryDetails:{},
            isCategoryNameRequired:false,
            propertiesRowRow: {},
            destinationIndexToRemove:-1,
            showDestinationsDeleteModal:false,
            destinationTitles:[],
            titleText : "",            
            showAddEditPropertiesFilter: false,
            options: [],
            destinationValue: "",
            propertiesRowIndex:-1
            
        });
    }

    componentDidMount() {
        this.setState({
            categoryDetails: this.props.data
        });        
    }

    //To Bind dropdown with all destinations name and description
    getOptions(categoryDetails) {
        var options = [];
        for (var x = 0; x < this.props.destinations.length; x++) {

            var detailIndex  = -1;

            if(categoryDetails.destinations.length>0)
                detailIndex= categoryDetails.destinations.findIndex((obj => obj.name == this.props.destinations[x].name));

            if(detailIndex < 0)
            {
                var optionValue = { value: this.props.destinations[x].name, label: this.props.destinations[x].name + "-" + this.props.destinations[x].description};
                options.push(optionValue);
            }
        }
        return options;
    }
    
    /// <summary>
    /// Handler to open properties at the indicated  row
    /// </summary>
    openPropertiesFilter(row,index) {
        this.setState({ showAddEditPropertiesFilter: true,propertiesRow: row.categories[0] ,propertiesRowIndex:index });
    }

    /// <summary>
    /// Handler to close properties modal pop up
    /// </summary
    closePropertiesFilter() {
        this.setState({ showAddEditPropertiesFilter: false });
    }

    //To delete  a destination of category
    removeDestinationModel(index, value)
    {
        var categoryData=[];
        categoryData = this.state.categoryDetails;
        if(categoryData.destinations[index].categories[0].id==undefined)
        {
            categoryData.destinations[index].name="";
            categoryData.destinations[index].description="";
        }
        else
        {
            categoryData.destinations.splice(value, 1);
            this.setState({ showDestinationsDeleteModal: false});
        }
        this.setState({categoryDetails: categoryData });        
    }

    //To add a new destination of category
    addNewDestination()
    {
        var optionValues = this.getOptions(this.state.categoryDetails);
        var newDestination= {name:"", description:"", categories:[]};
        var category = {
            name: this.state.categoryDetails.name,
            brands: [],
            titleIds: [],
            seriesIds: []
        };
        newDestination.categories.push(category);
        var categoryData=[];
        categoryData = this.state.categoryDetails;  
        categoryData.destinations.unshift(newDestination);
        this.setState({destinationDetails: categoryData, options: optionValues});
        
        this.CheckDestinationNameIsEmpty();
    }

    //To open delete destination warning window
    openDestinationsDeleteModel(item) {
        this.setState({ showDestinationsDeleteModal: true, destinationIndexToRemove: item });
    }

    // To close destination delete modal window
    closeDestinationDeleteModel() {
        this.setState({ showDestinationsDeleteModal: false });
    }

    SavePropertiesFilterData(selectedFilterValues)
    {
       
        var model= this.state.categoryDetails;
        model.destinations[this.state.propertiesRowIndex].categories[0].brands=selectedFilterValues.brands;
        model.destinations[this.state.propertiesRowIndex].categories[0].titleIds=selectedFilterValues.titleIds;
        model.destinations[this.state.propertiesRowIndex].categories[0].seriesIds=selectedFilterValues.seriesIds;
        this.setState({ categoryDetails: model , showAddEditPropertiesFilter: false});
       
    }

    //this method to show all category titles.
    titleDetailConstruct(item,index){
       
        var ids = [];
        if(item.categories.length>0)
        {            
            for (var i = 0; i < item.categories[0].seriesIds.length; i++) {
                ids.push(item.categories[0].seriesIds[i]);
            }

            for (var i = 0; i < item.categories[0].titleIds.length; i++) {
              
                ids.push(item.categories[0].titleIds[i]);
            }
          
            if(ids.length>0)
            {
                return <TitleNameOverlay data={ids} />;
            }
        }        
    }

    //this method constructs the category brands images.
    categoryBrandImageConstruct(item,index)
    {
        var brands = [];

        if(item.categories.length>0)
            brands = item.categories[0].brands;
        
        return <BrandsOverlay data={brands} /> ;
    }

    handleChange(index, value) {
        var model = this.state.categoryDetails;
        model.destinations[index].name= value;
        var detailIndex = this.props.destinations.findIndex((obj => obj.name == value));
        model.destinations[index].description= this.props.destinations[detailIndex].description;
        var optionValues = this.getOptions(model);
        this.setState({ categoryDetails: model, options: optionValues });  
        
        this.CheckDestinationNameIsEmpty();
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
                    this.props.validationStates(true);
                    return;
                }
            }
        }
        this.props.validationStates(false);
    }

    //destinations construct  of a category
    render() {
        let row = null;
        if (Object.keys(this.state.categoryDetails).length != 0 && this.state.categoryDetails != Object) {
            if(Object.keys(this.state.categoryDetails.destinations).length !== 0 && this.state.categoryDetails.destinations != Object){
                row = this.state.categoryDetails.destinations.map(function (item, index) {                    
                    var nameValidation=item.name!=""?null:"error";
                    let col = null, colDesc = null;
                    if(item.name==""){
                        col = (<Col sm={6   }>
                                    <FormGroup controlId={index} validationState="error">
                                          <Select 
                                      searchable={false} 
                                      simpleValue className="category-select-control" 
                                      options={this.state.options} 
                                      onChange={(event) => this.handleChange(index, event)}
                                      value={item.name} />
                                    </FormGroup>
                                </Col>
                            );
                    }
                    else{
                        col = (
                                <Col sm={3} >
                                    <FormGroup controlId={index} >
                                    <FormControl type="text" disabled={true} value={item.name} title={item.name} ref="Name"  placeholder="Destination" />
                                    </FormGroup>
                                </Col>
                            );

                            colDesc = (
                                <Col sm={3}>
                                    <FormGroup controlId={index} >
                                    <FormControl type="text" disabled={true} value={item.description} title={item.description} ref="Value"  placeholder="Description" />
                                    </FormGroup>
                                </Col>
                            );
                    }
                    return (<Row >
                        <Form>
                            {col}
                            {colDesc}
                            <Col sm={2} >{this.categoryBrandImageConstruct(item,index)}</Col>
                            <Col sm={2} >{this.titleDetailConstruct(item,index)}</Col>
                            <Col sm={2} >
                                <button type= "button"  class="btn-link" title="Edit Filter" onClick={(event) => this.openPropertiesFilter(item,index, event)} ><i class="fa fa-filter"></i></button>
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
                <button class="btn-link pull-right addMarginRight" title="Add New Destination" onClick={(event) => this.addNewDestination(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New Destination</span>
                </button>
            </div><br/><br/>
             <div>     
                 <div >
                    <Grid fluid={true}>
                        <Row>
                        <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
                        <Col sm={3} ><label class="destination-properties-label">Description</label></Col>
                        <Col sm={4} ><label class="destination-properties-label  destination-properties-filtermargin">Filters</label></Col>
                        <Col sm={2} ><label class="destination-properties-label destination-properties-actionmargin">Actions</label></Col>
                        </Row>
                        <div class="category-height">{row}</div>
                    </Grid>
                </div>
                   <PropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} handleSave={this.SavePropertiesFilterData.bind(this)} />
                  
                   <RemoveDestinationModal data={this.state} handleClose={this.closeDestinationDeleteModel.bind(this)}  handleRemoveAndClose={this.removeDestinationModel.bind(this)} />
            </div>
        </div>
     )
      }
          }

  export default AddEditCategoryDestination;