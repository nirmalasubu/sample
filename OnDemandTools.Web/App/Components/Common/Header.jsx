import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';
import { ButtonToolbar, DropdownButton, MenuItem, Tabs, Checkbox, Tab, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button, OverlayTrigger, Popover, Collapse, Well } from 'react-bootstrap';


import $ from 'jquery';
import SessionPage from 'Components/Common/SessionHandling/SessionPage';

@connect((store) => {
    return {
        user: store.user
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
    //                    <ButtonToolbar>
    //  <DropdownButton bsSize="large"  class=" btn-link" title={userFullName} id="dropdown-size-large"> 
    //    <MenuItem eventKey="1">{this.props.user.userName}</MenuItem>
    //    <MenuItem eventKey="2">{apiKey}</MenuItem>
    //    <MenuItem eventKey="3">{this.props.user.userName}</MenuItem>
    //    <MenuItem divider />
    //    <MenuItem eventKey="4" ><a href="/account/logoff">Logout </a></MenuItem>
    //  </DropdownButton>
    //</ButtonToolbar>

    render() {

        var userFullName = "";
        var apiKey = "";
        var lastLogin = "";

        if (this.props.user.firstName != undefined) {
            userFullName = "  " + this.props.user.firstName + " " + this.props.user.lastName;
            apiKey = this.props.user.api.apiKey;
            lastLogin = this.props.user.portal.lastLoginTime;

        }
        const popoverClickRootClose = (
            <Popover id="popover-trigger-click-root-close" >
                <label>UserName:</label>{this.props.user.userName} <br />
                <label>ApiKey: </label> {apiKey} <br />
                <label>LastLogin: </label>{lastLogin} <br />
                <label>Version: </label>1.0 <br />
                <hr />
                <a href="/account/logoff">Logout </a>
            </Popover>
        );

        return (
            <div >
                <Image src="../images/ODTLogo.png" rounded />
                <div class="header" >
                    <OverlayTrigger trigger="click" rootClose placement="bottom" overlay={popoverClickRootClose}>
                        <button class="btn-link" >
                            <i class="fa fa-user"></i>{userFullName}
                        </button>
                    </OverlayTrigger>

                </div>
                <hr />
            </div>



        )
    }
}



export default Header;