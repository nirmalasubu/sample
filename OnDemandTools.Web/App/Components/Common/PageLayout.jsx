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
          <Grid fluid>
            <Row className="show-grid">
                <Col><Header /></Col>
            </Row>
            <Row>
                <Col md={10} lg={10} mdHidden={true} lgHidden={true} ><Menu stacked={false} /></Col>
            </Row>
            <Row className="show-grid" >
              <Col md={2} lg={2} xsHidden={true} smHidden={true}><Menu stacked={true} /></Col>
              <Col xs={14} md={10} lg={10} ><Well style={wellStyles} >{props.content.children}</Well></Col>
            </Row>
          </Grid>
          
              
      </div>
  );
};

const wellStyles = { minHeight : 500 };

export default Layout