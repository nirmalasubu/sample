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

@connect((store) => {
  return {
    config: store.config
  };
})
class DestinationPropertiesFilter extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      brandsSelection: {},
      selectedTitles: [],
      hasChange: false,
      showWarningModel: false
    }
  }
  componentDidMount() {

  }

  openWarningModel() {
    this.setState({ showWarningModel: true });
  }

  closeWarningModel() {
    this.setState({ showWarningModel: false });
  }

  handleClose() {
    if (!this.state.hasChange) {
      this.props.handleClose();
    }
    else {
      this.openWarningModel();
    }
  }

  resetForm() {

    this.props.dispatch(clearTitles());
    var brands = [];
    var titles = [];

    var titleIds = this.props.data.destinationPropertiesRow.titleIds;

    if (titleIds.length > 0) {

      let searchPromise = searchByTitleIds(titleIds);
      searchPromise.then(message => {
        this.setState({ selectedTitles: message });
      })
      searchPromise.catch(error => {
        console.error(error);
        this.setState({ selectedTitles: [] });
      });
    }

    for (var i = 0; i < this.props.config.brands.length; i++) {
      var brandName = this.props.config.brands[i];
      var brandObject = {};
      brandObject.brandName = brandName;
      brandObject.selected = $.inArray(brandName, this.props.data.destinationPropertiesRow.brands) > -1;
      brands.push(brandObject);
    }

    this.setState({ brandsSelection: brands, selectedTitles: titles, hasChange: false });
  }

  handleBrandChange(brandName) {

    var brands = this.state.brandsSelection;

    for (var i = 0; i < brands.length; i++) {
      if (brands[i].brandName == brandName) {
        brands[i].selected = !brands[i].selected;
      }
    }

    this.setState({ brandsSelection: brands, hasChange: true });
  }

  onClickSave() {
    var brands = this.state.brandsSelection
    var array = [];
    for (var i = 0; i < brands.length; i++) {
      if (brands[i].selected) {
        array.push(brands[i].brandName);
      }
    }
    this.props.data.destinationPropertiesRow.brands = [];
    this.props.data.destinationPropertiesRow.brands = array;

    var titles = []
    var titleIds = [];

    for (var t = 0; t < this.state.selectedTitles.length; t++) {

      var selectedTitle = this.state.selectedTitles[t];

 
        var title = {};
        title.titleId = selectedTitle.titleId;
        title.name = selectedTitle.titleName;
        titles.push(title);
        titleIds.push(selectedTitle.titleId);
      
    }

    this.props.data.destinationPropertiesRow.titleIds = titleIds;
    this.props.data.destinationPropertiesRow.titles = titles;

    this.props.handleClose();
  }

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
        titlesToUpdate.push(titles[t]);
      }
    }

    this.setState({ selectedTitles: titlesToUpdate, hasChange: true });
  }

  render() {

    var rows = [];
    for (var i = 0; i < this.state.brandsSelection.length; i++) {
      var brand = this.state.brandsSelection[i];
      rows.push(<ImageCheckBox key={i.toString()} brandName={brand.brandName} selected={brand.selected} handleBrandChange={this.handleBrandChange.bind(this)} />);
    }

    return (
      <Modal className="destinationPropertiesFilterModel" bsSize="large"
        backdrop="static"
        onEntering={this.resetForm.bind(this)}
        show={this.props.data.showAddEditPropertiesFilter}
        onHide={this.handleClose.bind(this)}>
        <Modal.Header closeButton>
          <Modal.Title>
            <div>Filters</div>
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <div >
            <ControlLabel> Brands</ControlLabel><br />
            {rows}
            <hr />
            <ControlLabel>Title/Series Associations</ControlLabel>
            <TitleSearch
              selectedTitles={this.state.selectedTitles}
              onAddTitles={this.onAddTitles.bind(this)}
              onRemoveTitles={this.onRemoveTitles.bind(this)}
            />
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

export default DestinationPropertiesFilter