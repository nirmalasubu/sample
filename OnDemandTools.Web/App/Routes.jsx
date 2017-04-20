import React  from 'react';
import {Route, IndexRoute} from 'react-router';
import Home from './components/common/HomePage'
import Destinations from './components/Destinations/DestinationsPage'
import Products from './components/Products/ProductsPage'
import DeliveryQueues from './components/DeliveryQueues/DeliveryQueuesPage';
import Permissions from './components/Permissions/PermissionsPage'
import PendingRequests from './components/PendingRequests/PendingRequests'
import App from './components/App'

export default (
  <Route path="/" component={App}>
    <IndexRoute component={Home}></IndexRoute>
    <Route path="/destinations" component={Destinations}></Route>
    <Route path="/products" component={Products}></Route>
    <Route path="/deliveryQueues" component={DeliveryQueues}></Route>
    <Route path="/permissions" component={Permissions}></Route>
    <Route path="/pendingRequests" component={PendingRequests}></Route>
  </Route>
)