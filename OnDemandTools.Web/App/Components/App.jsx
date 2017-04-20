import React  from 'react';
import {Link} from 'react-router';

const App = (props) => {
  return (
    <div className="container">
      <nav className="navbar navbar-default">
        <div className="container-fluid">
          <div className="navbar-header">
            <a className="navbar-brand" href="#">OnDemand Tools</a>
          </div>
          <div className="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul className="nav navbar-nav">
              <li><Link to="/">Home</Link></li>
              <li><Link to="/destinations">Destinations</Link></li>
              <li><Link to="/products">Products</Link></li>
              <li><Link to="/deliveryQueues">Delivery Queues</Link></li>
              <li><Link to="/pendingRequests">Pending Requests</Link></li>
              <li><Link to="/permissions">Permissions</Link></li>
            </ul>
          </div>
        </div>
      </nav>
      {/* Each smaller components */}
      {props.children}
    </div>
  );
};

export default App