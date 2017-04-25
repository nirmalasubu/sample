import React  from 'react';
import Menu from './Menu';
import Header from './Header';
import {Well} from 'react-bootstrap';
import {Grid} from 'react-bootstrap';
import {Row} from 'react-bootstrap';
import {Col} from 'react-bootstrap';
import {Panel} from 'react-bootstrap';


const Layout = (props) => {
    return (
      <div >          
          <Grid fluid="true">
            <Row className="show-grid">
                <Col><Header /></Col>
            </Row>
            <Row className="show-grid" >
              <Col xs={1} md={2}><Menu /></Col>
              <Col xs={5} md={10} ><Panel style={wellStyles} >{props.content.children}</Panel></Col>
            </Row>
          </Grid>
          
              
      </div>
  );
};

const wellStyles = { minHeight : 500 };

export default Layout