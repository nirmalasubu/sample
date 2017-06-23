import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover,Collapse,Well } from 'react-bootstrap';
import DestinationPropertiesFilter from 'Components/Destinations/DestinationPropertiesFilter';
import RemovePropertiesModal from 'Components/Destinations/RemovePropertiesModal';

// Sub component of destination page to  add ,edit and delete  destination properties
class AddEditDestinationProperties extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({
            destinationDetails: {},
            showAddEditPropertiesFilter: false,
            showPropertiesDeleteModal:false,
            isPropertyNameRequired:false,
            destinationPropertiesRow: {},
            propertyIndexToRemove:-1,
            propertyTitles:[]
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
    RemovePropertiesModel(value)
    {
        var array=[];
        array = this.state.destinationDetails;
        array.properties.splice(value, 1);
        this.setState({destinationDetails: array });
        this.props.data = this.state.destinationDetails;
        this.setState({ showPropertiesDeleteModal: false});
        this.CheckPropertyNameIsEmpty();

    }

    //To add a new property of destination
    AddNewProperty()
    {
        var newProperty={name:"",value:"", brands:[],titleIds:[],seriesIds:[],titles:[]}
        var array=[];

        array = this.state.destinationDetails;
        array.properties.reverse();
        array.properties.push(newProperty);
        array.properties.reverse();
        this.setState({destinationDetails: array });

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
        this.props.data = this.state.destinationDetails;
        this.CheckPropertyNameIsEmpty();
    }

    // called when a property value is edited
    handlePropertyValueChange(event) {
        var model = this.state.destinationDetails;
        model.properties[event.target.id].value = event.target.value;
        this.setState({ destinationDetails: model });
        this.props.data = this.state.destinationDetails;

    }

    // validation for the name property . To verify  name text is empty
    CheckPropertyNameIsEmpty()
    {
        var properties=this.state.destinationDetails.properties;
        if(properties.length>0)
        {
            for(var i=0;i<=properties.length-1;i++)
            {
                if(!(properties[i].name))
                {
                    this.props.validationStates(true);
                    return;
                }
            }
        }
        this.props.validationStates(false);
    }


    //To show all titles.
    TitledetailConstruct(item,index){

        var titleName=[];
        item.titles.map(function (title, index) {titleName.push(title.name)});
        var titletext=titleName.toString();
        var title=(<p class="destination-label-title" title="title/series">{titletext}</p>);
        return title;
    }

    //To construct brands.
    PropertyBrandImageConstruct(item,index)
    {
        if(item.brands.length>=1 && item.brands.length<3)
        {
            var image=[];
            item.brands.map(function (name, index) {
                var path = "images/brands/" + name + ".gif";
                image.push(<div class="destination-container"><img  src={path}  title={name} alt={name}/></div>)});

            var tag=(<div>{image}</div>);
            return tag;
        }

        if(item.brands.length>=3)
        {
            var image=[];
            item.brands.map(function (name, index) {
                var path = "images/brands/" + name + ".gif";
                image.push(<div class="destination-container"><img  src={path}  title={name} alt={name}/></div>)
                if(index==1)
                {
                    image.push(<div class="destination-container"><button class="btn-link destination-img-btn" type="button"> <i class="fa fa-ellipsis-h" /></button></div>)
                    }
                    });
                    var tag=(<div class="destination-img">{image}</div>);
                    return tag;
                }
      }
// Tokens for the popover
popoverValueClickRootClose(index){
     return (<Popover id="popover-trigger-click-root-close" title="Subsitution Tokens">
             <span><button class="btn btn-primary btn-xs destination-properties-popovermargin"type="button" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;AIRING_ID&#125;">&#123;AIRING_ID&#125;</button></span>
             <span> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;AIRING_NAME&#125;">&#123;AIRING_NAME&#125;</button></span>
             <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;BRAND&#125;">&#123;BRAND&#125;</button></div>
             <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;TITLE_EPISODE_NUMBER&#125;">&#123;TITLE_EPISODE_NUMBER&#125;</button></div>
             <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;AIRING_STORYLINE_LONG&#125;">&#123;AIRING_STORYLINE_LONG&#125;</button></div>
             <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;AIRING_STORYLINE_SHORT&#125;">&#123;AIRING_STORYLINE_SHORT&#125;</button></div>
             <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;IFHD=(value)ELSE=(value)&#125;">&#123;IFHD=(value)ELSE=(value)&#125;</button></div>
             <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => this.SubsitutionTokenClick(index,event)} value="&#123;TITLE_STORYLINE(type)&#125;">&#123;TITLE_STORYLINE(type)&#125;</button></div>
             </Popover>);
     }

//To subsitute the token values  in the value text box
SubsitutionTokenClick(index,event){
    var model = this.state.destinationDetails;
    var oldValue= model.properties[index].value;
    model.properties[index].value = oldValue+event.target.value;
    this.setState({ destinationDetails: model });
    this.props.data = this.state.destinationDetails;
    var overlayIndex="overlay"+index;

    //hide the overlay after selecting token
    this.refs[overlayIndex].hide();
   }

//properties construct  of a destination
  render() {
      let row = null;
      if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object) {
       if(Object.keys(this.state.destinationDetails.properties).length !== 0 && this.state.destinationDetails.properties != Object){
         row = this.state.destinationDetails.properties.map(function (item, index) {
               var nameValidation=item.name?"":"error"
               var overlay="overlay"+index
       return (<Row >
         <Form>
         <Col sm={3} >
        <FormGroup controlId="Name" validationState={nameValidation}>
        <FormControl type="text" id={index} value={item.name} title={item.name} ref="Name"  placeholder="Name"  onChange={this.handlePropertyNameChange.bind(this)}  />
        </FormGroup></Col>
        <Col sm={3}> <OverlayTrigger trigger="click" ref={overlay} rootClose placement="left" overlay={this.popoverValueClickRootClose(index)}>
        <FormGroup controlId="Value" >
        <FormControl type="text" id={index} value={item.value} title={item.value} ref="Value"  placeholder="Value"  onChange={this.handlePropertyValueChange.bind(this)}  />
        </FormGroup></OverlayTrigger></Col>
        <Col sm={2} >{this.PropertyBrandImageConstruct(item,index)}</Col>
        <Col sm={2} >{this.TitledetailConstruct(item,index)}</Col>
        <Col sm={2} >
        <button type= "button"  class="btn-link" title="Edit Filter" onClick={(event) => this.openPropertiesFilter(item, event)} ><i class="fa fa-pencil-square-o"></i></button>
        <button type= "button"  class="btn-link" title="Delete Property" onClick={(event) => this.openPropertiesDeleteModel(index, event)} ><i class="fa fa-trash"></i></button>
        </Col>
        </Form>
        </Row>)}.bind(this));
      }
      else
      {
      row =<Row><Col sm={12} ><p> No properties available</p></Col></Row>
       }
    }

 return (
 <div>
     <div>
         <button class="destination-properties-addnew btn" title="Add New" onClick={(event) => this.AddNewProperty(event)}>Add  New</button>
    </div>
     <div >
    <Grid fluid={true}>
    <Row>
     <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
     <Col sm={3} ><label class="destination-properties-label">Value</label></Col>
     <Col sm={4} ><label class="destination-properties-label  destination-properties-filtermargin">Filter</label></Col>
     <Col sm={2} ><label class="destination-properties-label destination-properties-actionmargin">Actions</label></Col>
    </Row>
    <div class="destination-height">{row}</div>
    </Grid>
    </div>
    <DestinationPropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} />
   <RemovePropertiesModal data={this.state} handleClose={this.closePropertiesDeleteModel.bind(this)}  handleRemoveAndClose={this.RemovePropertiesModel.bind(this)} />
  </div>)
  }
}

export default AddEditDestinationProperties