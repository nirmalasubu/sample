import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';
import { ButtonToolbar, DropdownButton, MenuItem, Tabs, Tab, Grid, Row, Col, Button, } from 'react-bootstrap';
import $ from 'jquery';
import SessionPage from 'Components/Common/SessionHandling/SessionPage';

@connect((store) => {
    return {
        user: store.user,
        config: store.config
    };
})
class Header extends React.Component {
    constructor(props) {
        super(props);
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(fetchUser());
    }

    formatDate(val) {
        if (val == null) {
            return "never"
        }
        else {

            var d = new Date(val);
            var year = d.getFullYear();
            if (year < 2000) {
                return "never";
            }
            else {

                var dateFormat = Moment(val).format('lll');
                return Moment(val).format('lll');
            }
        }
    }

    render() {
        var userFullName,apiKey,lastLogin,userName = "";
        var isAPIActive,headerLogo,version = null;
       
        if (this.props.user.firstName != undefined) {
            userFullName=  "  " + this.props.user.firstName + " " + this.props.user.lastName;
            headerLogo = <label class="headerLogo" >
                                <i class="fa fa-user"></i>{userFullName}
                            </label> 
            userName= this.props.user.userName;            
            apiKey = this.props.user.api.apiKey;
            lastLogin = this.formatDate(this.props.user.portal.lastLoginTime);
            isAPIActive = this.props.user.api.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>;
        }
                if (this.props.config.version != undefined) {
                    version=<p class="header-version">Version:{this.props.config.version}</p>
                }
        return (
            <div >
                <Grid fluid>
                    <Row>
                         <Col md={2} lg={2} >
                        <Image src="../images/ODTLogo.png" rounded />
                        </Col>
                         <Col md={10} >
                        <div class="header headerdropdown">
                            <span>{headerLogo}</span>
                            <div class="headerdropdown-content">
                                <p><strong>UserName:</strong> {userName}</p>
                                <p><strong>ApiKey: </strong>{apiKey}{isAPIActive}</p>
                                <p><strong>LastLogin:</strong> {lastLogin}</p>
                                <hr />
                                <a href="/account/logoff">Logout </a>
                            </div>
                        </div>
                         </Col>
                    </Row>
                    <Row>
                         <Col md={2} lg={2} />
                         <Col md={10} >
                            {version}
                        </Col>
                    </Row>
                </Grid >
                <hr />
            </div>
        )
    }
}

export default Header;
