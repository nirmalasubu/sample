import React from 'react';
import { Link } from 'react-router';
import { ListGroup } from 'react-bootstrap';
import { ListGroupItem, Nav, NavItem } from 'react-bootstrap';
import { browserHistory } from 'react-router';


class Menu extends React.Component {

  constructor(props) {
    super(props);
    var currentRoute= window.location.pathname.replace("/","");
    
    if(currentRoute.length<2)
    {
      currentRoute= "home";
    }

    this.state = { activeKey: currentRoute };
    
  }

  handleSelect(selectedKey) {
    browserHistory.push(selectedKey);
    this.setState({ activeKey: selectedKey });
  }
  render() {
      return (
        <div>
          <Nav bsStyle="pills" stacked={this.props.stacked} activeKey={this.state.activeKey} onSelect={this.handleSelect.bind(this)}>
          <NavItem eventKey={"home"}>Home</NavItem>
          <NavItem eventKey={"deliveryQueues"}>Delivery Queues</NavItem>
          <NavItem eventKey={"destinations"}>Destinations</NavItem>
          <NavItem eventKey={"categories"}>Categories</NavItem>
          <NavItem eventKey={"products"}>Products</NavItem>
          <NavItem eventKey={"contentTiers"}>Content Tiers</NavItem>                   
          <NavItem eventKey={"workflowStatuses"}>Workflow Statuses</NavItem>
          <NavItem eventKey={"permissions"}>Permissions</NavItem>
        </Nav>
      </div>
    );
  }
}

export default Menu