import React from 'react';
import { Route, Router, hashHistory, IndexRoute } from 'react-router';
import Home from 'Components/Home/HomePage';
import DestinationPage from 'Components/Destinations/DestinationsPage';
import Products from 'Components/Products/ProductsPage';
import DeliveryQueues from 'Components/DeliveryQueues/DeliveryQueuesPage';
import UserPermissions from 'Components/Permissions/User/UserPermissionsPage';
import SystemPermissions from 'Components/Permissions/System/SystemPermissionsPage';
import PendingRequests from 'Components/PendingRequests/PendingRequestsPage';
import ContentTiers from 'Components/ContentTiers/ContentTiersPage';
import WorkflowStatuses from 'Components/WorkflowStatus/WorkflowStatusesPage';
import CategoriesPage from 'Components/Categories/CategoriesPage';
import airingIds from 'Components/IDDistribution/DistributionPage';
import PathTranslations from 'Components/PathTranslations/PathTranslationPage';
import App from 'Components/App';


export default (
  <Router history={hashHistory}>
    <Route path="/" component={App}>
      <IndexRoute component={Home}></IndexRoute>
      <Route path="home" component={Home}></Route>
      <Route path="destinations" component={DestinationPage}></Route>
      <Route path="categories" component={CategoriesPage}></Route>
      <Route path="products" component={Products}></Route>
      <Route path="deliveryQueues" component={DeliveryQueues}></Route>
      <Route path="pendingRequests" component={PendingRequests}></Route>
      <Route path="contentTiers" component={ContentTiers}></Route>
      <Route path="workflowStatuses" component={WorkflowStatuses}></Route>
      <Route path="airingIds" component={airingIds}></Route>
      <Route path="pathTranslations" component={PathTranslations}></Route>
      <Route path="userManagement" component={UserPermissions}></Route>
      <Route path="systemManagement" component={SystemPermissions}></Route>
    </Route>
  </Router>
)
