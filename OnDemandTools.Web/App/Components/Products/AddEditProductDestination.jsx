import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import validator from 'validator';
import InfoOverlay from 'Components/Common/InfoOverlay';
import * as destinationAction from 'Actions/Destination/DestinationActions';
import DualListBox from 'Components/Common/DualListBox/DualListBox';

@connect((store) => {
    return {
        destinations : store.destinations
    };
})
/// <summary>
/// Sub component of product page to  add ,edit product destination details
/// </summary>
class AddEditProductDestination extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);

        this.state = ({
            productModel: {},
            options:[],
            selected:[]
        });
    }

    /// <summary>
    /// This method sets the state before the component gets mounted
    /// </summary> 
    componentWillMount() {
        this.setState({ productModel: this.props.data });
    }

    /// <summary>
    /// This method sets the state after the component gets mounted
    /// </summary> 
    componentDidMount() {
        var optionsArray = this.state.options;
        var selectedArray = this.state.selected;
        var destinationNames = destinationAction.getDestinations();
        destinationNames.then(names => {
            var namesArray = names.map(val => val.name);
            namesArray.sort();
            for(var i=0; i < namesArray.length; i++)
            {
                var option = {label:namesArray[i], value:namesArray[i]};
                optionsArray.push(option);
            }

            for(var i=0; i < this.state.productModel.destinations.length; i++)
            {
                selectedArray.push(this.state.productModel.destinations[i]);
            }

            this.setState({options:optionsArray, selected:selectedArray});
        })
        destinationNames.catch(error => {
            console.error(error);
            this.setState({options:[], selected:[]});
        });   
    }

    /// <summary>
    /// This method sets the seleted values to the state
    /// </summary> 
    onChange(selectedValues)
    {
        var selectedArray = this.state.selected;
        var product = this.state.productModel;        

        selectedArray = [];        
        selectedArray = selectedValues;

        product.destinations=[];
        product.destinations = selectedValues;

        this.setState({productModel:product, selected:selectedArray});
    }

    render() {

        return (
            <div>
                <DualListBox canFilter moduleName="Destinations" options={this.state.options} onChange={this.onChange.bind(this)} selected={this.state.selected} />
            </div>
        )
    }
}

export default AddEditProductDestination