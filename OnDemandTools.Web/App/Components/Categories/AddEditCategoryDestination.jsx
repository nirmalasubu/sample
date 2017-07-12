import React from 'react';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover,Collapse,Well } from 'react-bootstrap';
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
        this.props.dispatch(destinationActions.fetchDestinations());
        this.props.data.destinations.sort(this.sortDestinationsByName);
        var optionValues = this.getOptions(this.props.data);
        this.setState({
            categoryDetails: this.props.data,
            options: optionValues
        });   
        
        this.CheckDestinationNameIsEmpty(this.props.data.destinations);
    }

    /// <summary>
    /// To Bind dropdown with all destinations name and description
    /// </summary>
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

        if(options.length>0)
            options.sort(this.sortOptionsByName);

        return options;
    }
    /// <summary>
    //This will sort your options array
    /// </summary>
    sortOptionsByName(a, b){
        var aName = a.value.toLowerCase();
        var bName = b.value.toLowerCase(); 
        return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
    }    

    /// <summary>
    //This will sort your destinations array
    /// </summary>
    sortDestinationsByName(a, b){
        var aName = a.name.toLowerCase();
        var bName = b.name.toLowerCase(); 
        return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
    }
    
    /// <summary>
    /// Handler to open properties at the indicated  row
    /// </summary>
    openPropertiesFilter(row,index) {
        this.setState({ showAddEditPropertiesFilter: true,propertiesRow: row.categories[0] ,propertiesRowIndex:index });
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
    removeDestinationModel(index, value)
    {
        var categoryData=[];
        categoryData = this.state.categoryDetails;
        if(categoryData.destinations[index].categories[0].id==undefined)
        {
            categoryData.destinations.splice(index, 1);
        }
        else
        {
            var category = {
                id: categoryData.destinations[index].categories[0].id,
                name: "",
                brands:categoryData.destinations[index].categories[0].brands,
                titleIds:categoryData.destinations[index].categories[0].titleIds,
                seriesIds:categoryData.destinations[index].categories[0].seriesIds,
                removed: true
            };
            categoryData.destinations[index].categories.length = 0;
            categoryData.destinations[index].categories.push(category);
            this.setState({ showDestinationsDeleteModal: false});
        }
        var optionValues = this.getOptions(categoryData);        
        this.setState({options: optionValues });

        this.CheckDestinationNameIsEmpty(categoryData.destinations);
    }

    /// <summary>
    //To add a new destination of category
    /// </summary>
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
        
        this.setState({options:optionValues});
        
        this.CheckDestinationNameIsEmpty(categoryData.destinations);
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

    SavePropertiesFilterData(selectedFilterValues)
    {
       
        var model= this.state.categoryDetails;
        model.destinations[this.state.propertiesRowIndex].categories[0].brands=selectedFilterValues.brands;
        model.destinations[this.state.propertiesRowIndex].categories[0].titleIds=selectedFilterValues.titleIds;
        model.destinations[this.state.propertiesRowIndex].categories[0].seriesIds=selectedFilterValues.seriesIds;
        this.setState({ categoryDetails: model , showAddEditPropertiesFilter: false});
       
    }

    /// <summary>
    //this method to show all category titles.
    /// </summary>
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

    /// <summary>
    //this method constructs the category brands images.
    /// </summary>
    categoryBrandImageConstruct(item,index)
    {
        //this.CheckDestinationNameIsEmpty();

        var brands = [];

        if(item.categories.length>0)
            brands = item.categories[0].brands;
        
        return <BrandsOverlay data={brands} /> ;
    }

    /// <summary>
    //this method handle the change of the dropdown value.
    /// </summary>
    handleChange(index, value) {
        var model = this.state.categoryDetails;
        model.destinations[index].name= value;
        var detailIndex = this.props.destinations.findIndex((obj => obj.name == value));
        model.destinations[index].description= this.props.destinations[detailIndex].description;
        var optionValues = this.getOptions(model);
        this.setState({ categoryDetails: model, options: optionValues });  
        
        this.CheckDestinationNameIsEmpty(model.destinations);
    }

    /// <summary>
    // validation for the name destination . To verify  name text is empty
    /// </summary>
    CheckDestinationNameIsEmpty(destinations)
    {
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

            this.props.validationStates(false);
        }
        else{
            this.props.validationStates(true);
        }        
    }

    /// <summary>
    //destinations construct  of a category
    /// </summary>
    render() {
        let row = null;
        if (Object.keys(this.state.categoryDetails).length != 0 && this.state.categoryDetails != Object) {            
            if(Object.keys(this.state.categoryDetails.destinations).length !== 0 && this.state.categoryDetails.destinations != Object){                     
                row = this.state.categoryDetails.destinations.map(function (item, index) {
                    if(true)
                    {
                        var nameValidation=item.name!=""?null:"error";
                        let col = null, colDesc = null;
                        if(item.name==""){
                            col = (<Col sm={6}  >
                                        <FormGroup controlId={index} validationState="error">
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
                else{
                        col = (
                                <Col sm={3}  >
                                    <FormControl controlId={index} type="text" disabled={true} value={item.name} title={item.name} ref="Name"  placeholder="Destination" />
                                    
                                </Col>
                            );

                colDesc = (
                    <Col sm={3} >
                        <FormControl controlId={index} type="text" disabled={true} value={item.description} title={item.description} ref="Value"  placeholder="Description" />
                       
                    </Col>
                            );
        }
        return (<Row bsClass={item.categories[0].removed==undefined?"row row-margin":"row row-margin strikeout"}>
    {col}
    {colDesc}
    <Col sm={2} bsClass="col-height col">{this.categoryBrandImageConstruct(item,index)}</Col>
    <Col sm={2} bsClass="col-height col">{this.titleDetailConstruct(item,index)}</Col>
    <Col sm={2} bsClass="col-height col">
        <button type= "button"  class="btn-link img-height" title="Add/Edit Filter" onClick={(event) => this.openPropertiesFilter(item,index, event)} ><i class="fa fa-filter"></i></button>
        <button type= "button"  class="btn-link img-height" title="Delete Destination" onClick={(event) => this.removeDestinationModel(index)} ><i class="fa fa-trash"></i></button>
    </Col>

</Row>)}}.bind(this));
    }
    else
    {
        row =<Row><Col sm={12}><p> No destination available</p></Col></Row>
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
                    <Grid bsClass="category-table" >
                        <Row >
                            <Col sm={3} ><label class="destination-properties-label">Destination</label></Col>
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