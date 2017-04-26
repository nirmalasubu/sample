import 'babel-polyfill';
import React from 'react';
import { Provider } from 'react-redux';
import { render } from 'react-dom';
import { Router, browserHistory } from 'react-router';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
// import '../node_modules/font-awesome/cs/s/font-awesome.css';
import routes from 'Routes';
import configureStore from 'Store/ConfigureStore';
import '../wwwroot/css/site.css'
require('font-awesome/css/font-awesome.css');

const store = configureStore();
render(
    <Provider store={store}>
    <Router routes={routes} history={browserHistory} />
  </Provider>,
  document.getElementById('app')
);
