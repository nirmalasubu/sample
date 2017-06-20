import React from 'react';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover,Collapse,Well } from 'react-bootstrap';


class AddEditDestinationCategories extends React.Component {

    constructor(props) {
        super(props);
        this.state = ({
            destinationDetails: {},
            isPropertyNameRequired:false,
            titleopen:[],
            imageopen:[]
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

    handlePropertyNameChange(event) {
        var model = this.state.destinationDetails;
        model.properties[event.target.id].name = event.target.value;
        this.setState({ destinationDetails: model });
        this.props.data = this.state.destinationDetails;
        this.CheckPropertyNameIsEmpty();
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
   

    TitleDetailClick(index)
    { 
        var array=this.state.titleopen;
        array[index]=!this.state.titleopen[index];
        this.setState({titleopen:array});
        
    }

    TitledetailConstruct(item,index){
        if(this.state.titleopen[index]){
            var titleName=[];
            item.titles.map(function (title, index) {titleName.push(title.name)});
            var titletext=titleName.toString();
            return(<div>{titletext}</div>);}
    }

    CategoryTitleConstruct(item,index)
    { 
        if(item.titles.length==1)
        {
            return(<div>{item.titles[0].name}</div>);
        }
              
        if(item.titles.length>1)
        {
            if(this.state.titleopen[index]==undefined)
            { 
                var array=this.state.titleopen;
                array[index]=false; 
                this.setState({titleopen:array});
                this.props.titleopen=this.state.titleopen;
            }
                        
            var titleName=[];
            item.titles.map(function (title, index) {titleName.push(title.name)});
            var titletext=titleName.toString();
            var firstTitle=titleName[0]+"...";
            var title=(<div onMouseOver={(event) => this.TitleDetailClick(index, event)}>{firstTitle}</div>);
       
            return title;
    }
}

ImageDetailClick(index){
   
    var array=this.state.imageopen;
    array[index]=!this.state.imageopen[index];
    this.setState({imageopen:array});
   
}


CategoryBrandImageConstruct(item,index)
{ 
    if(item.brands.length==1)
    {
        var path = "images/brands/" + name + ".gif";
       return (<img src={path} />);
    }
              
if(item.brands.length>1)
{
    if(this.state.imageopen[index]==undefined)
    { 
        var array=this.state.imageopen;
        array[index]=false; 
        this.setState({imageopen:array});
        this.props.imageopen=this.state.imageopen;
    }
    var path = "images/brands/" + item.brands[0] + ".gif";
    var image=(<img onMouseOver={(event) => this.ImageDetailClick(index, event)} src={path} />);
       
        return image;
}
}




    render() {
        let row = null;

        if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object) {
            if(Object.keys(this.state.destinationDetails.categories).length !== 0 && this.state.destinationDetails.categories != Object)
            {
                row = this.state.destinationDetails.categories.map(function (item, index) {
                    var nameValidation=item.name?"":"error"
                    return (<Row >
                   <Form>
                   <Col sm={3} >
                        <FormGroup controlId="Name" validationState={nameValidation}>
                       <FormControl type="text" id={index} value={item.name} ref="Name"  placeholder="Name" disabled="true" onChange={this.handlePropertyNameChange.bind(this)}  />
                       </FormGroup></Col>
                    </Form>
                   <Col sm={2} >
     {this.CategoryBrandImageConstruct(item,index)}
            {this.state.imageopen[index]? item.brands.map(function (name, index)
            {
                var path = "images/brands/" + name + ".gif";
                return (<img src={path} />);
            
            }):null}

                  </Col>

                  <Col sm={2} >
                  {this.CategoryTitleConstruct(item,index)}
                  {this.state.titleopen[index]?this.TitledetailConstruct(item,index):null}
                                  </Col>
                          </Row>)
      }.bind(this));
      }
      else
      {
                                row =<Row><Col sm={12} ><p> No Categories available</p></Col></Row>
      }

      }

                    return (
                      <div>
                              <div >
                              <Grid fluid={true}>
                        <Row>
                          <Col sm={3} ><label class="destination-properties-label">Name</label></Col>
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