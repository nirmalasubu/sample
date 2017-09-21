import React from 'react';
import { Link } from 'react-router';
import { ListGroup } from 'react-bootstrap';
import { ListGroupItem, Nav, NavItem, NavDropdown, MenuItem } from 'react-bootstrap';
import { browserHistory } from 'react-router';
import { connect } from 'react-redux';

@connect((store) => {
  return {
    user: store.user
  };
})
class Menu extends React.Component {

  constructor(props) {
    super(props);
    var currentRoute = window.location.pathname.replace("/", "");

    if (currentRoute.length < 2) {
      currentRoute = "home";
    }

    this.state = { activeKey: currentRoute, modulePermissions: {} };

  }

  componentDidMount() {
    if (this.props.user.portal) {
      this.setState({ modulePermissions: this.props.user.portal.modulePermissions });
    }
  }

  //receives prop changes to update state
  componentWillReceiveProps(nextProps) {
    if (nextProps.user.portal) {
      this.setState({ modulePermissions: nextProps.user.portal.modulePermissions });
    }
  }

  handleSelect(selectedKey) {
    browserHistory.push(selectedKey);
    this.setState({ activeKey: selectedKey });
  }

  onToggle() {

  }

  render() {
    var categoryMenuItem = null;
    var deliveryQueuesMenuItem = null;
    var destinationMenuItem = null;
    var productsMenuItem = null;
    var contentTierMenuItem = null;
    var workflowMenuItem = null;
    var airingIdMenuItem = null;
    var pathTranslationsMenuItem = null;
    var userManagementMenuItem = null;

    if (this.state.modulePermissions.Categories) {
      var module = this.state.modulePermissions;

      if (module.Categories.canRead) {
        categoryMenuItem = <NavItem eventKey={"categories"}>Categories</NavItem>;
      }

      if (module.DeliveryQueues.canRead) {
        deliveryQueuesMenuItem = <NavItem eventKey={"deliveryQueues"}>Delivery Queues</NavItem>;
      }

      if (module.Destinations.canRead) {
        destinationMenuItem = <NavItem eventKey={"destinations"}>Destinations</NavItem>;
      }

      if (module.Products.canRead) {
        productsMenuItem = <NavItem eventKey={"products"}>Products</NavItem>;
      }

      if (module.ContentTiers.canRead) {
        contentTierMenuItem = <NavItem eventKey={"contentTiers"}>Content Tiers</NavItem>;
      }

      if (module.WorkflowStatuses.canRead) {
        workflowMenuItem = <NavItem eventKey={"workflowStatuses"}>Workflow Statuses</NavItem>;
      }

      if (module.IdDistribution.canRead) {
        airingIdMenuItem = <NavItem eventKey={"airingIds"}>ID Distribution</NavItem>;
      }

      if (module.PathTranslations.canRead) {
        pathTranslationsMenuItem = <NavItem eventKey={"pathTranslations"}>Path Translations</NavItem>;
      }

      if (module.UserManagement.canRead) {
        userManagementMenuItem = <NavDropdown class="dropdownAccess" open={true} onToggle={this.onToggle.bind(this)} noCaret
          title="Access Management" id="nav-dropdown">
          <MenuItem eventKey={"userManagement"}>User</MenuItem>
          <MenuItem eventKey={"systemManagement"}>System</MenuItem>
        </NavDropdown>;
      }

    }

    return (
      <div>
        <Nav bsStyle="pills" stacked={this.props.stacked} activeKey={this.state.activeKey} onSelect={this.handleSelect.bind(this)}>
          <NavItem eventKey={"home"}>Home</NavItem>
          {deliveryQueuesMenuItem}
          {destinationMenuItem}
          {categoryMenuItem}
          {productsMenuItem}
          {contentTierMenuItem}
          {workflowMenuItem}
          {airingIdMenuItem}
          {pathTranslationsMenuItem}
          {userManagementMenuItem}
        </Nav>
      </div>
    );
  }
}

export default Menu