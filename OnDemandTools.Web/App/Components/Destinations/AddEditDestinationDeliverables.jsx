import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import RemoveDeliverablesModal from 'Components/Destinations/RemoveDeliverablesModal';

//
// Sub component for all things related to destination deliverables
class AddEditDestinationDeliverables extends React.Component {

  // Define default component state information.
  constructor(props) {
    super(props);

    this.state = ({
      destinationDetails: {},
      showDeliverableDeleteModal: false,
      deliverableIndexToRemove: -1
    });
  }

  /// <summary>
  // Invoked immediately after this component is mounted
  /// <summary>
  componentDidMount() {

    // Before starting out, record parent component's data within this 
    // component's state
    this.setState({
      destinationDetails: this.props.data
    });
  }


  /// <summary>
  /// Add a new row for entering new deliverable info   
  /// </summary>
  addNewDeliverable() {
    var newDeliverable = { value: "" }

    // First get the current destination data which is funneled
    // down from the parent component
    var destination = this.state.destinationDetails;

    // Next, access deliverables (array defined in destination)
    // and add a placeholder for new deliverable
    destination.deliverables.push(newDeliverable);

    // Reflect the desination changes in global store by initiating
    // redux action
    this.setState({ destinationDetails: destination });

    // Inform parent component about validation state change
    // so that it can render its controls accordingly. In this case
    // the validation state is false b/c the user hasn't entered anything for deliverable value
    this.props.validationStates(true);
  }


  /// <summary>
  /// Handler to close deliverable delete modal window
  /// </summary>
  closeDeliverableDeleteModal = () => {
    this.setState({ showDeliverableDeleteModal: false });
  }


  /// <summary>
  /// Handler to remove deliverable at the indicated index and close deliverable delete modal window
  /// </summary>
  removeDeliverableAndCloseDeliverableDeleteModal = (index) => {

    // Remove deliverable at the indicated index and update local state
    var model = this.state.destinationDetails;
    model.deliverables.splice(index, 1);
    this.setState({ destinationDetails: model });

    // Close modal window and set states to default values
    this.setState({ showDeliverableDeleteModal: false });
    this.setState({ deliverableIndexToRemove: -1 });

    // Bubble up validation state change to parent
    this.updateValidationStates();
  }


  /// <summary>
  /// The idea here is to iterate through the full list of deliverables and if
  /// any of the deliverable value is empty/null, bubble up validation state change
  /// to parent
  /// </summary>
  updateValidationStates = () => {

    var stateChanged = this.state.destinationDetails.deliverables.some((deliverable) => {
      if (!deliverable.value) {
        return true;
      }
    });

    this.props.validationStates(stateChanged);
  }


  /// <summary>
  /// Generate tabular view to display deliverables associated with this destination  
  /// </summary>
  generateDeliverableTable = () => {


    // Open confirmation window and if confirmed, remove item from 'deliverables' list at 'index'
    const openDeleteDeliverableModal = (index) => {
      // open state
      this.setState({ showDeliverableDeleteModal: true, deliverableIndexToRemove: index });
    }


    // Handle event that's triggered from changing deliverable value. 
    // The change is reflected in local state 'destinationDetails' and
    // bubble up a validation state change event to parent component
    const handleDeliverableValueChange = (event) => {
      var model = this.state.destinationDetails;
      model.deliverables[event.target.id].value = event.target.value;
      this.setState({ destinationDetails: model });

      // Bubble up validation state change to parent. 
      this.updateValidationStates();
    }

    // Handle substitution token click
    const subsitutionTokenClick = (index, event) => {

      // Render the selected token and append it to existing value
      var model = this.state.destinationDetails;
      var oldValue = model.deliverables[index].value;
      model.deliverables[index].value = oldValue + event.target.value;
      this.setState({ destinationDetails: model });
     
      // Bubble up validation state change to parent. 
      this.updateValidationStates();

      // Hide the overlay
      this.refs.overlay.hide();
    }


    // Define the set of substitution tokens that will be available within each deliverable text box
    const popoverValueClickRootClose = (index) => {
      return (<Popover id="popover-trigger-click-root-close" title="Subsitution Tokens">
        <span><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_ID&#125;">&#123;AIRING_ID&#125;</button></span>
        <span> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_NAME&#125;">&#123;AIRING_NAME&#125;</button></span>
        <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;BRAND&#125;">&#123;BRAND&#125;</button></div>
        <div><button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;TITLE_EPISODE_NUMBER&#125;">&#123;TITLE_EPISODE_NUMBER&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" type="button" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_LONG&#125;">&#123;AIRING_STORYLINE_LONG&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_SHORT&#125;">&#123;AIRING_STORYLINE_SHORT&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;IFHD=(value)ELSE=(value)&#125;">&#123;IFHD=(value)ELSE=(value)&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-properties-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;TITLE_STORYLINE(type)&#125;">&#123;TITLE_STORYLINE(type)&#125;</button></div>
      </Popover>);
    }

    // Verfiy whether destination details (and related information like deliverables) are available before rendering
    if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object
      && this.state.destinationDetails.deliverables.length > 0) {

      // Construct individual row contents
      var deliverableRows = this.state.destinationDetails.deliverables.map(function (item, index) {

        // Validate if deliverable value is empty. If yes, set state to 'error'
        var isValueValid = item.value ? "" : "error";

        return (<Row key={index.toString()}>
          <Col sm={10} >
            <Form>
              <OverlayTrigger trigger="click" rootClose placement="left" ref="overlay" overlay={popoverValueClickRootClose(index)}>
                <FormGroup validationState={isValueValid}>
                  <FormControl type="text" id={index.toString()} value={item.value} placeholder="Value" onChange={handleDeliverableValueChange.bind(this)} />
                </FormGroup>
              </OverlayTrigger>
            </Form>
          </Col>
          <Col sm={2} >
            <button class="btn-link" title="Delete Deliverable" onClick={(event) => openDeleteDeliverableModal(index, event)}>
              <i class="fa fa-trash"></i></button>
          </Col>
        </Row>)
      });

      return (

        // Construct the full tabuler view of deliverables for rendering
        <Grid fluid={true} id="deliverable-grid">
          <Row>
            <Col sm={10} ><label class="destination-properties-label">Value</label></Col>
            <Col sm={2} ><label class="destination-properties-label destination-properties-actionmargin">Actions</label></Col>
          </Row>
          <div class="destination-height">
            {deliverableRows}
          </div>
        </Grid>
      );
    }

  }


  render() {
    return (
      <div>
        <div>
          <button class="destination-properties-addnew btn" title="Add New" onClick={(event) => this.addNewDeliverable(event)}>
            Add  New
          </button>
        </div>
        <div>
          {this.generateDeliverableTable()}
        </div>
        <RemoveDeliverablesModal data={this.state} handleClose={this.closeDeliverableDeleteModal.bind(this)} handleRemoveAndClose={this.removeDeliverableAndCloseDeliverableDeleteModal.bind(this)} />

      </div>
    )
  }
}

export default AddEditDestinationDeliverables