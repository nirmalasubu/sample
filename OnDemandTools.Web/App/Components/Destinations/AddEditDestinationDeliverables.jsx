import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

/// <summary>
// Sub component for all things related to destination deliverables
/// <summary>
class AddEditDestinationDeliverables extends React.Component {

  // Define default component state information.
  constructor(props) {
    super(props);

    this.state = ({
      destinationDetails: {},
      cursorPosition: 0
    });
  }

  /// <summary>
  // Invoked immediately when there is any change in props
  /// <summary>
  componentWillReceiveProps(nextProps) {
    this.setState({
      destinationDetails: nextProps.data
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
    var newDeliverable = { value: "", deleted: false }

    // First get the current destination data which is funneled
    // down from the parent component
    var destination = this.state.destinationDetails;

    // Next, access deliverables (array defined in destination)
    // and add a placeholder for new deliverable in first row
    destination.deliverables.unshift(newDeliverable);

    // Reflect the desination changes in global store by initiating
    // redux action
    this.setState({ destinationDetails: destination });

    // Inform parent component about validation state change
    // so that it can render its controls accordingly. In this case
    // the validation state is false b/c the user hasn't entered anything for deliverable value
    this.props.validationStates(true);
  }

  /// <summary>
  /// The idea here is to iterate through the full list of deliverables and if
  /// any of the deliverable value is empty/null, bubble up validation state change
  /// to parent
  /// </summary>
  updateValidationStates = () => {

    var stateChanged = this.state.destinationDetails.deliverables.some((deliverable) => {
      if (!deliverable.deleted) {
        if (!deliverable.value) {
          return true;
        }
      }
    });

    this.props.validationStates(stateChanged);
  }


  /// <summary>
  /// Generate tabular view to display deliverables associated with this destination  
  /// </summary>
  generateDeliverableTable = () => {

    const removeDeliverable = (index) => {

      // Remove deliverable at the indicated index and update local state
      var model = this.state.destinationDetails;
      model.deliverables[index].deleted = true;
      this.setState({ destinationDetails: model });

      // Close modal window and set states to default values
      this.setState({ showDeliverableDeleteModal: false });
      this.setState({ deliverableIndexToRemove: -1 });

      // Bubble up validation state change to parent
      this.updateValidationStates();
    }

    // Handle event that's triggered from changing deliverable value. 
    // The change is reflected in local state 'destinationDetails' and
    // bubble up a validation state change event to parent component
    const handleDeliverableValueChange = (event) => {
      var model = this.state.destinationDetails;
      model.deliverables[event.target.id].value = event.target.value;
      this.setState({ destinationDetails: model });

      // set the current cursor location
      this.setState({ cursorPosition: event.target.selectionStart });

      // Bubble up validation state change to parent. 
      this.updateValidationStates();
    }

    // Handle substitution token click event
    const subsitutionTokenClick = (index, event) => {

      // Render the selected token and append it to existing value at the cursorPosition
      var model = this.state.destinationDetails;
      var oldValue = model.deliverables[index].value;
      var newValue = oldValue.slice(0, this.state.cursorPosition) + event.target.value + oldValue.slice(this.state.cursorPosition);
      model.deliverables[index].value = newValue;
      this.setState({ destinationDetails: model });

      // Bubble up validation state change to parent. 
      this.updateValidationStates();

      // Hide the overlay
      this.refs.overlay.hide();
    }

    // Handle click event to record cursor position
    const handleDeliverableValueClick = (event) => {
      this.setState({ cursorPosition: event.target.selectionStart });
    }

    // Define the set of substitution tokens that will be available within each deliverable text box
    const popoverValueClickRootClose = (index) => {
      return (<Popover id="popover-trigger-click-root-close" title="Substitution Tokens">
        <span><button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_ID&#125;">&#123;AIRING_ID&#125;</button></span>
        <span> <button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_NAME&#125;">&#123;AIRING_NAME&#125;</button></span>
        <div><button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;BRAND&#125;">&#123;BRAND&#125;</button></div>
        <div><button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;TITLE_EPISODE_NUMBER&#125;">&#123;TITLE_EPISODE_NUMBER&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_LONG&#125;">&#123;AIRING_STORYLINE_LONG&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;AIRING_STORYLINE_SHORT&#125;">&#123;AIRING_STORYLINE_SHORT&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;IFHD=(value)ELSE=(value)&#125;">&#123;IFHD=(value)ELSE=(value)&#125;</button></div>
        <div> <button class="btn btn-primary btn-xs destination-deliverables-popovermargin" onClick={(event) => subsitutionTokenClick(index, event)} value="&#123;TITLE_STORYLINE(type)&#125;">&#123;TITLE_STORYLINE(type)&#125;</button></div>
      </Popover>);
    }

    // Verfiy whether destination details (and related information like deliverables) are available before rendering
    if (Object.keys(this.state.destinationDetails).length != 0 && this.state.destinationDetails != Object
      && this.state.destinationDetails.deliverables.length > 0) {

      // Construct individual row contents
      var deliverableRows = this.state.destinationDetails.deliverables.map(function (item, index) {

        // Validate if deliverable value is empty. If yes, set state to 'error'
        var isValueValid = item.value ? null : "error";

        if (isValueValid == "error" && item.deleted == true) {
          isValueValid = null;
        }

        let deliverableTextBox = null;

        if (item.deleted) {
          deliverableTextBox =
            <FormGroup>
              <FormControl type="text" disabled={true} id={index.toString()} value={item.value} ref="input" placeholder="Value"
                className="deliverablesTextBox" />
            </FormGroup>
        }
        else {
          deliverableTextBox =
            <OverlayTrigger trigger="click" rootClose placement="left" ref="overlay" overlay={popoverValueClickRootClose(index)}>
              <FormGroup validationState={isValueValid}>
                <FormControl type="text" id={index.toString()} value={item.value} ref="input" placeholder="Value" onChange={handleDeliverableValueChange.bind(this)}
                  onClick={handleDeliverableValueClick}
                  className="deliverablesTextBox" />
              </FormGroup>
            </OverlayTrigger>;
        }

        return (<Row componentClass="tr" className={item.deleted ? "strikeout" : ""} key={index.toString()}>
          <Col componentClass="td">
            <Form>
              {deliverableTextBox}
            </Form>
          </Col>
          <Col componentClass="td"  >
            <button class="btn-link" disabled={item.deleted} title="Delete Deliverable" onClick={(event) => removeDeliverable(index, event)}>
              <i class="fa fa-trash"></i></button>
          </Col>
        </Row>)
      });

      return (
        // Construct the full tabuler view of deliverables for rendering
        <Grid componentClass="table" bsClass="modalTable" id="deliverable-grid">
          <thead>
            <Row componentClass="tr">
              <Col componentClass="th" ><label>Value</label></Col>
              <Col componentClass="th" className="actionsColumn" ><label>Action</label></Col>
            </Row>
          </thead>
          <tbody>
            {deliverableRows}
          </tbody>
        </Grid>
      );
    }

  }


  render() {
    return (
      <div>
        <div>
          <button class="btn-link pull-right addMarginRight" title="Add New Destination" onClick={(event) => this.addNewDeliverable(event)}>
            <i class="fa fa-plus-square fa-2x"></i>
            <span class="addVertialAlign"> New Deliverable</span>
          </button>
        </div>
        <div className="clearBoth">
          {this.generateDeliverableTable()}
        </div>
      </div>
    )
  }
}

export default AddEditDestinationDeliverables