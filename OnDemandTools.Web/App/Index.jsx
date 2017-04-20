import 'babel-polyfill';
import React from 'react';
import { Provider } from 'react-redux';
import { render } from 'react-dom';
import { Router, browserHistory } from 'react-router';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import routes from './Routes';

render(
  <Provider>
    <Router routes={routes} history={browserHistory} />
  </Provider>,
  document.getElementById('app')
);
