import React from 'react';
import ImageCheckBox from 'Components/Common/ImageCheckBox';
import TitleSearch from 'Components/Common/TitleSearch/TitleSearch';
import { clearTitles, searchByTitleIds } from 'Actions/TitleSearch/TitleSearchActions';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { connect } from 'react-redux';
import $ from 'jquery';
import CancelWarningModal from 'Components/Common/CancelWarningModal';


// store.config gets list of brands names
@connect((store) => {
    return {
        config: store.config
    };
})

/// <summary>
///  Sub component for  related to Properties filter for destination and categories
/// <summary>
class PropertiesFilter extends React.Component {

    // Define default component state information.
    constructor(props) {
        super(props);
        this.state = {
            brandsSelection: {},
            selectedTitles: [],
            hasChange: false,
            showWarningModel: false,
            loadingTable:false
        }
    }

    /// <summary>
    ///  to open warning pop up window when user made changes and trying to close it.
    /// </summary>
    openWarningModel() {
        this.setState({ showWarningModel: true });
    }

    /// <summary>
    ///  to close warning pop up window when user made changes and  dont want to save the changes
    /// </summary>
    closeWarningModel() {
        this.setState({ showWarningModel: false });
    }

    /// <summary>
    ///  when pop up window cancel button is clicked
    /// </summary>
    handleClose() {
        // when there is no change in the close the pop up  else invoke the warning pop up 
        if (!this.state.hasChange) {
            this.props.handleClose();
        }
        else {
            this.openWarningModel(); // warning pop up window is invoked
        }
    }

    /// <summary>
    /// Loaded with data  when the pop up opens 
    /// </summary>
    loadForm() {
        
        this.props.dispatch(clearTitles());
        var brands = [];
        var titles = [];

        var titleIds = this.props.data.propertiesRow.titleIds;
        titleIds = titleIds.concat(this.props.data.propertiesRow.seriesIds);

        if (titleIds.length > 0) {
            this.setState({ loadingTable: true });
            let searchPromise = searchByTitleIds(titleIds);
            searchPromise.then(message => {
                //Title Data  is fetched asynchronously .  
                this.setState({ selectedTitles: message ,loadingTable: false});   
            })
            searchPromise.catch(error => {
                console.error(error);
                titles = [];
            });
        }
        else{
            this.setState({ loadingTable: false });
        }
       
        for (var i = 0; i < this.props.config.brands.length; i++) {
            var brandName = this.props.config.brands[i];
            var brandObject = {};
            brandObject.brandName = brandName;
            brandObject.selected = $.inArray(brandName, this.props.data.propertiesRow.brands) > -1;
            brands.push(brandObject);
        }
        

        this.setState({ brandsSelection: brands, selectedTitles: titles, hasChange: false });
      
    }

    /// <summary>
    /// when the brand  image is selected  and selected brands are saved in  a state
    /// </summary>
    handleBrandChange(brandName) {

        var brands = this.state.brandsSelection;

        for (var i = 0; i < brands.length; i++) {
            if (brands[i].brandName == brandName) {
                brands[i].selected = !brands[i].selected;
            }
        }

        this.setState({ brandsSelection: brands, hasChange: true });
    }

    /// <summary>
    /// save the changes
    /// </summary>
    onClickSave() {
        var brands = this.state.brandsSelection
        var selectedBrands = [];
        for (var i = 0; i < brands.length; i++) {
            if (brands[i].selected) {
                selectedBrands.push(brands[i].brandName);
            }
        }
       
        var titleIds = [];
        var seriesIds = [];
        for (var t = 0; t < this.state.selectedTitles.length; t++) {

            var selectedTitle = this.state.selectedTitles[t];

            if (selectedTitle.titleType.name == "Series") {
                seriesIds.push(selectedTitle.titleId);
            }
            else {
                titleIds.push(selectedTitle.titleId);
            }
        }
        var selectedFilterValues={"brands":selectedBrands,"titleIds":titleIds,"seriesIds":seriesIds};
        this.props.handleSave(selectedFilterValues);
    }

    /// <summary>
    /// remove the selected titles from the list
    /// </summary>
    onRemoveTitles(titles) {
        var titlesToUpdate = this.state.selectedTitles;
        for (var t = 0; t < titles.length; t++) {
            var titleFoundIdx = -1;
            for (var titleIdx = 0; titleIdx < titlesToUpdate.length; titleIdx++) {
                if (titlesToUpdate[titleIdx].titleId == titles[t].titleId) {
                    titleFoundIdx = titleIdx;
                }
            }
            if (titleFoundIdx > -1) {
                titlesToUpdate.splice(titleFoundIdx, 1);
            }
        }

        this.setState({ selectedTitles: titlesToUpdate, hasChange: true });
    }

    /// <summary>
    /// add the selected titles to the list
    /// </summary>
    onAddTitles(titles) {
        var titlesToUpdate = this.state.selectedTitles;

        for (var t = 0; t < titles.length; t++) {
            var titlesToAdd = null;
            for (var titleIdx = 0; titleIdx < titlesToUpdate.length; titleIdx++) {
                if (titlesToUpdate[titleIdx].titleId == titles[t].titleId) {
                    titlesToAdd = titlesToUpdate[titleIdx];
                }
            }
            if (titlesToAdd == null) {
                titlesToUpdate.unshift(titles[t]);
            }
        }

        this.setState({ selectedTitles: titlesToUpdate, hasChange: true });
    }

    render() {
        // ImageCheckBox sub component used to render list brand images
        var rows = [];
        for (var i = 0; i < this.state.brandsSelection.length; i++) {
            
            var brand = this.state.brandsSelection[i];
            rows.push(<ImageCheckBox brandName={brand.brandName} selected={brand.selected} handleBrandChange={this.handleBrandChange.bind(this)} />);
    }

    return (
      <Modal className="destinationPropertiesFilterModel" bsSize="large"
        backdrop="static"
        onEntering={this.loadForm.bind(this)}
        show={this.props.data.showAddEditPropertiesFilter}
        onHide={this.handleClose.bind(this)}>
        <Modal.Header closeButton>
          <Modal.Title>
            <div>Filters</div>
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
             <div class="panel panel-default">
             <div class="panel-body">
          <div >
            <ControlLabel> Brands</ControlLabel><br />
        {rows}
            <hr />
           
            <ControlLabel>Title/Series Associations</ControlLabel>
            <TitleSearch  
              selectedTitles={this.state.selectedTitles}
              hasTitles={this.state.loadingTable}
              onAddTitles={this.onAddTitles.bind(this)}
              onRemoveTitles={this.onRemoveTitles.bind(this)}
            />
            </div>
            </div>
          </div>
          <CancelWarningModal data={this.state} handleClose={this.closeWarningModel.bind(this)} handleAddEditClose={this.props.handleClose.bind(this)} />
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.handleClose.bind(this)}>Cancel</Button>
          <Button className="btn btn-primary btn-large" onClick={this.onClickSave.bind(this)}>Save</Button>
        </Modal.Footer>
      </Modal>
    )
          }
          }

export default PropertiesFilter