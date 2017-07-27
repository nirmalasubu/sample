import React from 'react';
import $ from 'jquery';
import PropTypes from 'prop-types';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import ListBox from 'Components/Common/DualListBox/ListBox';

class DualListBox extends React.Component {
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

    static defaultProps = {
        available: undefined,
        selected: [],
        canFilter:false,
        filterType:"available",
        moduleName:""
    };

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

    filterCallBack(option, filterInput){
        if (filterInput === '') {
            return true;
        }

        return (new RegExp(filterInput, 'i')).test(option.label);
    };

    onFilterChange(event) {
        var filter = this.state.filter;
        filter[event.target.dataset.key] = event.target.value;
        this.setState({
            filter: filter
        });
    }

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

    filterAvailable(options) {
        const { filterType } = this.props;
        // Show all un-selected options
        return this.filterOptions(
            options,
            option => this.props.selected.indexOf(option.value) < 0,
            this.state.filter.available
        );
    }

    filterSelected(options) {
        const { filterType } = this.props;
        // Order the selections by the default order
        return this.filterOptions(
            options,
            option => this.props.selected.indexOf(option.value) >= 0,
            this.state.filter.selected
        );
    }

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

    onDoubleClick(event) {
        const value = event.currentTarget.value;
        const selected = this.toggleSelected([value]);

        this.props.onChange(selected);
    }

    onClick(direction) {
        const { options, onChange } = this.props;
        const select = direction === 'Right' ? this.available : this.selected;

        console.log(select);

        let selected = [];
        
        selected = this.toggleSelected(
            this.getSelectedOptions(select),
        );

        onChange(selected);
    }

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

    getSelectedOptions(element) {
        return this.arrayFrom(element.options)
            .filter(option => option.selected)
            .map(option => option.value);
    }

    arrayFrom(iterable) {
        const arr = [];

        for (let i = 0; i < iterable.length; i += 1) {
            arr.push(iterable[i]);
        }

        return arr;
    }

    onLeftSelectChange(event){
        var array = [];
        array = this.getSelectedOptions(event.target);
        if(array.length>0)
            this.setState({isAvailableSelected:true});
        else
            this.setState({isAvailableSelected:false});
    }

    onRightSelectChange(event){
        var array = [];
        array = this.getSelectedOptions(event.target);
        if(array.length>0)
            this.setState({isCurrentSelected:true});
        else
            this.setState({isCurrentSelected:false});
    }

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
