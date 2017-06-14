import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button,OverlayTrigger,Popover } from 'react-bootstrap';
import DestinationPropertiesForm from 'Components/Destinations/DestinationPropertiesForm';



class AddEditDestinationProperties extends React.Component {

  constructor(props) {
      super(props);
      this.state = ({
          destinationProperties:{}
      });
  }
    componentDidMount() {
        
      this.setState({
          destinationProperties:this.props.data});
  }


              handlePropertyNameChange(event) {
                  var model = this.state.destinationProperties;
                  model.properties[event.target.id].name = event.target.value;
                  this.setState({ destinationProperties: model });
                  this.props.data=this.state.destinationProperties;
                 
              }

              handlePropertyValueChange(event) {
                  var model = this.state.destinationProperties;
                  model.properties[event.target.id].value = event.target.value;
                  this.setState({ destinationProperties: model });
                  this.props.data=this.state.destinationProperties;
                 
              }

             


              render() {
 
                  var row;
                  if (Object.keys(this.state.destinationProperties).length != 0 && this.state.destinationProperties != Object) 
                  {
                     
                      row = this.state.destinationProperties.properties.map(function (item, index) {
                          
                          return (<div class="destination-properties-RowMargin"><Row >
                         <Col sm={3} > <input type="text" id={index} defaultValue={item.name} onChange={this.handlePropertyNameChange.bind(this)} /> </Col>
                         <Col sm={3} ><input type="text"  id={index} defaultValue={item.value}  onChange={this.handlePropertyValueChange.bind(this)} /></Col>
                         <Col sm={4} >
                        {item.brands.map(function(name, index){
                            return <span>{name}</span>
                        })}
                         
                         </Col>
                         
                         <Col sm={2} > <button class="btn-link" title="Edit Properties" >
                                      <i class="fa fa-pencil-square-o"></i>
                                    </button>

                                    <button class="btn-link" title="Delete Properties" >
                                      <i class="fa fa-trash"></i></button>
                           </Col>
                         </Row></div>);
                             }.bind(this));
                             }
    return (
      <div>
            <div>
                    <button class="destination-properties-addnew" title="Add New"  onClick={(event) => this.openCreateNewQueueModel(event)}>
                        Add  New
                    </button>
               </div>
          <Grid fluid={true}>
              <Row>
                  <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
                      <Col sm={3} ><label class="destination-properties-label">Value</label></Col>
                          <Col sm={4} ><label class="destination-properties-label">Filter</label></Col>
                              <Col sm={2} ><label class="destination-properties-label">Actions</label></Col>
              </Row>
              {row}
              </Grid>

       
      </div>
    )
                              }
                    }

export default AddEditDestinationProperties