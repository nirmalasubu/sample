import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover,Collapse,Well } from 'react-bootstrap';
import DestinationPropertiesFilter from 'Components/Destinations/DestinationPropertiesFilter';
import RemovePropertiesModal from 'Components/Destinations/RemovePropertiesModal';


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
      titleopen:false,
      imageopen:false
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

  openPropertiesFilter(row) {
    this.setState({ showAddEditPropertiesFilter: true, destinationPropertiesRow: row });
  }

  closePropertiesFilter() {
    this.setState({ showAddEditPropertiesFilter: false });
  }

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
  AddNewProperty()
  {
      var newProperty={name:"",value:"", brands:[],titleIds:[],seriesIds:[],titles:[]}
      var array=[];
    
      array = this.state.destinationDetails;
      array.properties.reverse();
      array.properties.push(newProperty);
      array.properties.reverse();
      this.setState({destinationDetails: array });
      //this.props.data = this.state.destinationDetails;
      
      this.CheckPropertyNameIsEmpty();
  }

  openPropertiesDeleteModel(item) {
    this.setState({ showPropertiesDeleteModal: true, propertyIndexToRemove: item });
  }

  closePropertiesDeleteModel() {
    this.setState({ showPropertiesDeleteModal: false });
  }

  handlePropertyNameChange(event) {
    var model = this.state.destinationDetails;
    model.properties[event.target.id].name = event.target.value;
    this.setState({ destinationDetails: model });
    this.props.data = this.state.destinationDetails;
    this.CheckPropertyNameIsEmpty();
  }

  handlePropertyValueChange(event) {
    var model = this.state.destinationDetails;
    model.properties[event.target.id].value = event.target.value;
    this.setState({ destinationDetails: model });
    this.props.data = this.state.destinationDetails;

  }

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


  render() {
      let row = null;
      
      if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object) {
          if(Object.keys(this.state.destinationDetails.properties).length !== 0 && this.state.destinationDetails.properties != Object)
          {
              row = this.state.destinationDetails.properties.map(function (item, index) {
                  var nameValidation=item.name?"":"error"
                  var titleName=[]
                  item.titles.map(function (title, index) {titleName.push(title.name)})
                  var  firstTitle=titleName[0]
                  var titlestring=titleName.toString()
                 
                 

                  return (<Row >
                 <Form>
                 <Col sm={3} >
                      <FormGroup controlId="Name" validationState={nameValidation}>
                     <FormControl type="text" id={index} value={item.name} ref="Name"  placeholder="Name"  onChange={this.handlePropertyNameChange.bind(this)}  />
                     </FormGroup></Col>
                 <Col sm={3}> 
                     <FormGroup controlId="Value" >
                     <FormControl type="text" id={index} value={item.value} ref="Value"  placeholder="Value"  onChange={this.handlePropertyValueChange.bind(this)}  />
                     </FormGroup></Col>
                  </Form>
                 <Col sm={2} >
                    
                {item.brands.map(function (name, index) {
                    var path = "images/brands/" + name + ".gif"
                    return (<img src={path} />);
                })}
               
                   
                </Col>
                <Col sm={2} >
          <div onClick={ ()=> this.setState({ titleopen: !this.state.titleopen })}>
                {firstTitle} 
              </div>
              <Collapse in={this.state.titleopen}>
                <div>
                {titlestring}
                 </div> 
              </Collapse>
                          </Col>
                          <Col sm={2} >
                            <button class="btn-link" title="Edit Filter" onClick={(event) => this.openPropertiesFilter(item, event)} >  
                              <i class="fa fa-pencil-square-o"></i>
                            </button>
                            <button class="btn-link" title="Delete Property" onClick={(event) => this.openPropertiesDeleteModel(index, event)} >
                              <i class="fa fa-trash"></i></button>
                          </Col>
                        </Row>)
                              }.bind(this));
                              }
                              else
                              {
                              row =<Row><Col sm={12}><p> No properties available</p></Col></Row>
                              }
               
                              }
         
         

                  return (
                    <div>
                      <div>
                        <button class="destination-properties-addnew btn" title="Add New" onClick={(event) => this.AddNewProperty(event)}>
                                Add  New
                                            </button>
                            </div>
                            <div >
                            <Grid fluid={true}>
                      <Row>
                        <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
                        <Col sm={3} ><label class="destination-properties-label">Value</label></Col>
                        <Col sm={4} ><label class="destination-properties-label  destination-properties-filtermargin">Filter</label></Col>
                        <Col sm={2} ><label class="destination-properties-label destination-properties-actionmargin">Actions</label></Col>
                      </Row>
                      <div class="destination-height">
                        {row}
                          </div>
                    </Grid>
                        </div>
                   <DestinationPropertiesFilter data={this.state} handleClose={this.closePropertiesFilter.bind(this)} />
                    <RemovePropertiesModal data={this.state} handleClose={this.closePropertiesDeleteModel.bind(this)}  handleRemoveAndClose={this.RemovePropertiesModel.bind(this)} />
                  </div>
                )
                  }
                            }

            export default AddEditDestinationProperties