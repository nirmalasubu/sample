import React from 'react';
import ReactDOM from 'react-dom';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import ImageCheckBox from 'Components/Common/ImageCheckBox';

@connect((store) => {
    return {
        config: store.config
    };
})

/// <summary>
/// Sub component of user permission  page to  provide permission to Brands in API
/// </summary>
class AddEditBrandPermissions extends React.Component {
    /// <summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    /// </summary>
    constructor(props) {
        super(props);
        this.state = ({
            brandsSelection: {},
            userPermissionModel: "",
            userPermissionunmodifiedModel: "",
            componentJustMounted: true,
        });
    }

    componentWillMount() {
        this.setState({
            userPermissionModel: this.props.data
        });
    }

    componentDidMount() {
        var model = this.state.userPermissionModel;
        this.setState({
            userPermissionunmodifiedModel: jQuery.extend(true, {}, this.props.data)
        });
        this.pushBrandsPropstoState(this.state.userPermissionModel.api.brands);
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        this.setState({
            userPermissionModel: nextProps.data
        });
       this.pushBrandsPropstoState(nextProps.data.api.brands);
    }

    /// <summary>
    /// To push property brands to local state brands
    /// </summary>
    pushBrandsPropstoState(propsBrands)
    {
        var brands = [];
        for (var i = 0; i < this.props.config.brands.length; i++) {
            var brandName = this.props.config.brands[i];
            var brandObject = {};
            brandObject.brandName = brandName;
            brandObject.selected = $.inArray(brandName, propsBrands) > -1;
            brands.push(brandObject);
        }
        this.setState({ brandsSelection: brands });
    }

    /// <summary>
    /// event for the permit all checkbox
    /// </summary>
    permitBrandChange() {
        var model = this.state.userPermissionModel;
        model.api.brandPermitAll = !this.state.userPermissionModel.api.brandPermitAll;

        if (model.api.brandPermitAll) {
            model.api.brands = [];
            for (var i = 0; i < this.props.config.brands.length; i++) {
                model.api.brands.push(this.props.config.brands[i]);
            }
        }
        else {
            model.api.brands = [];
            for (var i = 0; i < this.state.userPermissionunmodifiedModel.length; i++) {
                model.api.brands.push(this.state.userPermissionunmodifiedModel[i]);
            }
        }

        this.setState({ userPermissionModel: model });
        this.props.updatePermission(model);
    }

    /// <summary>
    /// click event for selecting individual brands
    /// </summary>
    handleBrandChange(brandName) {
        var model = this.state.userPermissionModel;
        var brands = this.state.brandsSelection;
        if (model.portal.isAdmin ||model.api.brandPermitAll) {
            return false;
        }

        for (var i = 0; i < brands.length; i++) {
            if (brands[i].brandName == brandName) {
               if (!brands[i].selected) {
                    if ($.inArray(brandName, model.api.brands) == -1)
                        model.api.brands.push(brandName);
                }
               else {
                    model.api.brands.splice($.inArray(brandName, model.api.brands), 1);
                }
            }
        }
        this.props.updatePermission(model);
    }

    render() {
        var rows = [];
        for (var i = 0; i < this.state.brandsSelection.length; i++) {
            var brand = this.state.brandsSelection[i];
            rows.push(<span key={i.toString()} ><ImageCheckBox brandName={brand.brandName} brandkey={i} selected={brand.selected} handleBrandChange={this.handleBrandChange.bind(this)} /></span>);
        }

    return <div>  <br/><Form inline>
            <FormGroup controlId="permit" >
                <label for="permit" class="control-label" style={{ float: "left", paddingRight: 10 }}>Permit All</label>
                <div style={{ float: "left" }}>
                    <label class="switch">
                        <input type="checkbox" disabled={this.state.userPermissionModel.portal.isAdmin?"disabled":""} checked={this.state.userPermissionModel.api.brandPermitAll} onChange={(event) => this.permitBrandChange()} />
                        <span class="slider round"></span>
                    </label>
                </div>
            </FormGroup>
            <div> {rows} </div>
        </Form></div>
    }
}

export default AddEditBrandPermissions