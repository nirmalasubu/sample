import React from 'react';
import {Link} from 'react-router';
import {ListGroup} from 'react-bootstrap';
import {ListGroupItem} from 'react-bootstrap';

const Menu = () => {
    return (
      <div>
        <ListGroup componentClass="div" style={MenuStyles}>
            <ListGroupItem ><Link to="/">Home</Link></ListGroupItem>
            <ListGroupItem ><Link to="/destinations">Destinations</Link></ListGroupItem>
            <ListGroupItem ><Link to="/products">Products</Link></ListGroupItem>
            <ListGroupItem ><Link to="/pendingRequests">Pending Requests</Link></ListGroupItem>
            <ListGroupItem ><Link to="/permissions">Permissions</Link></ListGroupItem>
        </ListGroup>
      </div>
  );
};

const MenuStyles = { margin: '30 auto 10px', float: 'left'};

export default Menu