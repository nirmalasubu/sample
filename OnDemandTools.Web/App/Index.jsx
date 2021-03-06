import 'babel-polyfill';
import React from 'react';
import { Provider } from 'react-redux';
import { render } from 'react-dom';
import { Router, browserHistory } from 'react-router';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import routes from 'Routes';
import configureStore from 'Store/ConfigureStore';
import '../wwwroot/css/site.css';
require('font-awesome/css/font-awesome.css');

// Configure the store that will be used through out the application
const store = configureStore();
window.store = store;


render(
    <Provider store={store}>
    <Router routes={routes} history={browserHistory} />
  </Provider>,
  document.getElementById('app')
);
