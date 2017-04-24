﻿import React from 'react';
import { Link } from 'react-router';
import { ListGroup } from 'react-bootstrap';
import { ListGroupItem, Nav, NavItem } from 'react-bootstrap';
import { browserHistory } from 'react-router';


class Menu extends React.Component {

  constructor() {
    super();
    this.state = { activeKey: "home" };
  }

  handleSelect(selectedKey) {
    browserHistory.push(selectedKey);
    this.setState({ activeKey: selectedKey });
  }
  render() {
    return (
      <div>
        <Nav bsStyle="pills" stacked activeKey={this.state.activeKey} onSelect={this.handleSelect.bind(this)}>
          <NavItem eventKey={"home"}>Home</NavItem>
          <NavItem eventKey={"deliveryQueues"}>Delivery Queues</NavItem>
          <NavItem eventKey={"destinations"}>Destinations</NavItem>
          <NavItem eventKey={"products"}>Products</NavItem>
          <NavItem eventKey={"pendingRequests"}>Pending Requests</NavItem>
          <NavItem eventKey={"permissions"}>Permissions</NavItem>
        </Nav>
      </div>
    );
  }
}

export default Menu