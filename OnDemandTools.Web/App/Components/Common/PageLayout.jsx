import React  from 'react';
import Menu from './Menu';
import Header from './Header';
import {Well} from 'react-bootstrap';


const Layout = (props) => {
    return (
      <div className="container">
        <Header />
        <Menu />
        {/* Each smaller components */}
        <Well  style={wellStyles} >{props.content.children}</Well>      
      </div>
  );
};

const wellStyles = { minWidth: 980, minHeight: 500, margin: '0 auto 10px', float: 'left'};

export default Layout