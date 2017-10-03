import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';
import { ButtonToolbar, DropdownButton, MenuItem, Tabs, Tab, Grid, Row, Col, Button, OverlayTrigger, Popover } from 'react-bootstrap';
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
        this.props.dispatch(fetchConfig());
        this.props.dispatch(fetchUser());
    }

    componentWillMount() {
        this.props.dispatch(fetchConfig());
    }

    render() {

        var userFullName, apiKey, userName = "";
        var isAPIActive, userlogo, version = null;

        if (this.props.user.firstName != undefined) {
            userFullName = "  " + this.props.user.firstName + " " + this.props.user.lastName;
            userlogo = <i class="fa fa-user"></i>;
            userName = this.props.user.userName;
            apiKey = this.props.user.api.apiKey;
            isAPIActive = this.props.user.api.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>;
        }

        if (this.props.config.version != undefined) {
            version = <p class="header-version">Version: {this.props.config.version}</p>
        }

        const popoverClickRootClose = (
            <Popover id="popover-trigger-click-root-close" class="headerpopover-content">
                <label>User Name: </label> <p>{this.props.user.userName} </p>
                <label>Api Key: </label><p>{apiKey} {isAPIActive}  </p>
                <hr />
                <a href="/account/logoff">Logout </a>
            </Popover>
        );

        return (
            <div >
                <Grid fluid>
                    <Row>
                        <Col md={2} lg={2} >
                            <Image src="../images/ODTLogo.png" rounded />
                        </Col>
                        <Col md={10} >
                            <div class="header">
                                <OverlayTrigger trigger="click" rootClose placement="bottom" overlay={popoverClickRootClose}>
                                    <button class="btn-link" >
                                        {userlogo}{userFullName}
                                    </button>
                                </OverlayTrigger>
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
