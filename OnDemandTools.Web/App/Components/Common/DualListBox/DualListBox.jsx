import React from 'react';
import $ from 'jquery';
import PropTypes from 'prop-types';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import ListBox from 'Components/Common/DualListBox/ListBox';

class DualListBox extends React.Component {
    /// <summary>
    /// set the prop types
    /// </summary>
    static propTypes = {
        options: PropTypes.arrayOf(
            PropTypes.oneOfType([
                optionShape,
                PropTypes.shape({
                    value: PropTypes.any,
                    options: PropTypes.arrayOf(optionShape),
                }),
                ]),
        ).isRequired,
        onChange: PropTypes.func.isRequired,
        canFilter: PropTypes.bool,
        available: PropTypes.arrayOf(PropTypes.string),
        selected: PropTypes.arrayOf(PropTypes.string),
        filterType: PropTypes.string,
        moduleName: PropTypes.string,
    };

    /// <summary>
    /// set the default values to the prop types
    /// </summary>
    static defaultProps = {
        available: undefined,
        selected: [],
        canFilter:false,
        filterType:"available",
        moduleName:""
    };

    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);

        this.state = ({
            filter: {
                available: '',
                selected: '',
            },

            isAvailableSelected:false,
            isCurrentSelected:false
        });

        this.onDoubleClick = this.onDoubleClick.bind(this);
        this.onKeyUp = this.onKeyUp.bind(this);
        this.onFilterChange = this.onFilterChange.bind(this);
        this.onLeftSelectChange = this.onLeftSelectChange.bind(this);
        this.onRightSelectChange = this.onRightSelectChange.bind(this);
    }

    ///<summary>
    /// This is a callback method which filters the options based upon the filter input 
    ///</summary>
    filterCallBack(option, filterInput){
        if (filterInput === '') {
            return true;
        }

        return (new RegExp(filterInput, 'i')).test(option.label);
    };

    ///<summary>
    /// This is a callback method which is called at the time of search text change. 
    ///</summary>
    onFilterChange(event) {
        var filter = this.state.filter;
        filter[event.target.dataset.key] = event.target.value;
        this.setState({
            filter: filter
        });
    }

    ///<summary>
    /// Sets the available and selected options based on the filterer 
    ///</summary>
    filterOptions(options, filterer, filterInput) {
        const {canFilter, filterCallback } = this.props;
        const filtered = [];

        options.forEach((option) => {
            if (filterer(option)) {
                // Test option against filter input
                if (canFilter && !this.filterCallBack(option, filterInput)) {
                    return;
                }

                filtered.push(option);
            }
        });

        return filtered;
    }

    ///<summary>
    /// Sets the available options 
    ///</summary>
    filterAvailable(options) {
        const { filterType } = this.props;
        // Show all un-selected options
        return this.filterOptions(
            options,
            option => this.props.selected.indexOf(option.value) < 0,
            this.state.filter.available
        );
    }

    ///<summary>
    /// Sets the selected options 
    ///</summary>
    filterSelected(options) {
        const { filterType } = this.props;
        // Order the selections by the default order
        return this.filterOptions(
            options,
            option => this.props.selected.indexOf(option.value) >= 0,
            this.state.filter.selected
        );
    }

    ///<summary>
    /// this method renders the options tag inside select 
    ///</summary>
    renderOptions(options) {
        return options.map((option) => {
            const key = `${option.value}-${option.label}`;

            return (
                <option key={key} value={option.value}>
                    {option.label}
                </option>
            );
        });
    }

    ///<summary>
    /// this methos add/remove the values from selected array 
    ///</summary>
    toggleSelected(selected) {
        const oldSelected = this.props.selected.slice(0);

        selected.forEach((value) => {
            const index = oldSelected.indexOf(value);

            if (index >= 0) {
                oldSelected.splice(index, 1);
            } else {
                oldSelected.push(value);
            }
        });

        return oldSelected;
    }
    
    ///<summary>
    /// this method moves thevalue to the opposite box on double click 
    ///</summary>
    onDoubleClick(event) {
        const value = event.currentTarget.value;
        const selected = this.toggleSelected([value]);

        this.setState({
            isAvailableSelected:false,
            isCurrentSelected:false
        });

        this.props.onChange(selected);
    }

    ///<summary>
    /// this method adds/removes the values from available and selected box 
    /// on click of add and remove button
    ///</summary>
    onClick(direction) {
        const { options, onChange } = this.props;
        const select = direction === 'Right' ? this.available : this.selected;

        console.log(select);

        let selected = [];
        
        selected = this.toggleSelected(
            this.getSelectedOptions(select),
        );

        this.setState({
            isAvailableSelected:false,
            isCurrentSelected:false
        });

        onChange(selected);
    }

    ///<summary>
    /// this method selects the values from available and selected box on key up
    ///</summary>
    onKeyUp(event) {
        const { currentTarget, key } = event;

        if (key === 'Enter') {
            const selected = this.toggleSelected(
                arrayFrom(currentTarget.options)
                    .filter(option => option.selected)
                    .map(option => option.value),
            );

            this.props.onChange(selected);
        }
    }

    ///<summary>
    /// this method gets the select values from the list box
    ///</summary>
    getSelectedOptions(element) {
        return this.arrayFrom(element.options)
            .filter(option => option.selected)
            .map(option => option.value);
    }

    ///<summary>
    /// this method gets the array of values from the options shape
    ///</summary>
    arrayFrom(iterable) {
        const arr = [];

        for (let i = 0; i < iterable.length; i += 1) {
            arr.push(iterable[i]);
        }

        return arr;
    }

    ///<summary>
    /// this method gets called at the time of selecting 
    /// the values from available list box
    ///</summary>
    onLeftSelectChange(event){
        var array = [];
        array = this.getSelectedOptions(event.target);
        if(array.length>0)
            this.setState({isAvailableSelected:true});
        else
            this.setState({isAvailableSelected:false});
    }

    ///<summary>
    /// this method gets called at the time of selecting 
    /// the selected list box
    ///</summary>
    onRightSelectChange(event){
        var array = [];
        array = this.getSelectedOptions(event.target);
        if(array.length>0)
            this.setState({isCurrentSelected:true});
        else
            this.setState({isCurrentSelected:false});
    }

    ///<summary>
    /// this method renders filter text box component
    ///</summary>
    renderFilter(controlKey, displayName) {
        const {
            canFilter,
            filterType
            } = this.props;
        
        if(filterType!=controlKey)
        {                
            return null;
        }

        return (
            <div >
                <input
                    data-key={controlKey}
                    id={'filter-'+{controlKey}}
                    type="text"
                    value={this.state.filter[controlKey]}
                    onChange={this.onFilterChange}
                />
            </div>
        );
    }

    ///<summary>
    /// this method renders select box component
    ///</summary>
    renderListBox(controlKey, displayName, options, actions) {
        const {
            canFilter,
            filterType
            } = this.props;

            return (
                <ListBox
                    controlKey={controlKey}
                    canFilter={canFilter}
                    displayName={displayName}
                    inputRef={(c) => {
                        this[controlKey] = c;
                    }}
                    filterValue={this.state.filter[controlKey]}
                    onFilterChange={this.onFilterChange}
                    onDoubleClick={this.onDoubleClick}
                    onKeyUp={this.onKeyUp}
                    onSelectChange={actions=="Right"?this.onLeftSelectChange:this.onRightSelectChange}
                    filterType={filterType}
                >
                    {options}
                </ListBox>
            );
                    }

    render() {
            const {
                options,
                selected,
                moduleName
                } = this.props;
                const availableOptions = this.renderOptions(this.filterAvailable(options));
                const selectedOptions = this.renderOptions(this.filterSelected(options));

        return (
                <div >
                    <Grid fluid={true} >
                        <Row>
                            <Col md={3}>
                                <label >
                                    Search
                                </label><br/>
                                {this.renderFilter('available', 'Available')}
                            </Col>
                            <Col md={3} >
                                {this.renderListBox('available', 'Available '+ moduleName, availableOptions, "Right")}
                            </Col>
                            <Col md={2} >
                                <div class="listBoxButtons" >
                                    <Button bsStyle="primary" className="listBoxButton btnList" disabled={!this.state.isAvailableSelected} onClick={this.onClick.bind(this,"Right")}>
                                            Add >
                                    </Button>
                                        <br /><br />
                                    <Button bsStyle="primary" className="listBoxButton btnList" disabled={!this.state.isCurrentSelected} onClick={this.onClick.bind(this,"Left")} >
                                        {"< Remove"}
                                    </Button>
                                </div>
                            </Col>
                            <Col md={3} >
                                {this.renderListBox('selected', 'Current '+ moduleName, selectedOptions, "Left")}
                            </Col>
                        </Row>
                    </Grid>
                </div>
            )
    }
}

const optionShape = PropTypes.shape({
    value: PropTypes.any.isRequired,
    label: PropTypes.string.isRequired,
});

export default DualListBox;
