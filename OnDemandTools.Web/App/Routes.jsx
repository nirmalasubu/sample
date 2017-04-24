import React from 'react';
import { Route, Router, hashHistory, IndexRoute } from 'react-router';
import Home from 'Components/Common/HomePage'
import Destinations from 'Components/Destinations/DestinationsPage'
import Products from 'Components/Products/ProductsPage'
import DeliveryQueues from 'Components/DeliveryQueues/DeliveryQueuesPage';
import Permissions from 'Components/Permissions/PermissionsPage'
import PendingRequests from 'Components/PendingRequests/PendingRequests'
import App from 'Components/App'

export default (
  <Router history={hashHistory}>
    <Route path="/" component={App}>
      <IndexRoute component={Home}></IndexRoute>
      <Route path="home" component={Home}></Route>
      <Route path="destinations" component={Destinations}></Route>
      <Route path="products" component={Products}></Route>
      <Route path="deliveryQueues" component={DeliveryQueues}></Route>
      <Route path="permissions" component={Permissions}></Route>
      <Route path="pendingRequests" component={PendingRequests}></Route>
    </Route>
  </Router>
)
