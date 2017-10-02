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
        var userFullName = "";
        var apiKey = "";
        var lastLogin = "";
        var isAPIActive = null;
        if (this.props.user.firstName != undefined) {
            userFullName = "  " + this.props.user.firstName + " " + this.props.user.lastName;
            apiKey = this.props.user.api.apiKey;
            lastLogin = this.formatDate(this.props.user.portal.lastLoginTime);
            isAPIActive = this.props.user.api.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>;
        }

        return (
            <div >
                <Grid >
                    <Row>
                        <Image src="../images/ODTLogo.png" rounded />
                        <div class="header headerdropdown">
                            <span><button class="btn-link" >
                                <i class="fa fa-user"></i>{userFullName}
                            </button></span>
                            <div class="headerdropdown-content">
                                <p><strong>UserName:</strong> {this.props.user.userName}</p>
                                <p><strong>ApiKey: </strong>{apiKey}{isAPIActive}</p>
                                <p><strong>LastLogin:</strong> {lastLogin}</p>
                                <hr />
                                <a href="/account/logoff">Logout </a>
                            </div>
                        </div>
                    </Row>
                    <Row>
                        <p class="header-version">Version:{this.props.config.version}</p>
                    </Row>
                </Grid >
                <hr />
            </div>
        )
    }
}

export default Header;
