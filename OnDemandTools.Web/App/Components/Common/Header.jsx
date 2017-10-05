import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchWhoAmI } from 'Actions/WhoAmI/WhoAmIActions';
import { ButtonToolbar, DropdownButton, MenuItem, Tabs, Tab, Grid, Row, Col, Button, OverlayTrigger, Popover } from 'react-bootstrap';
import $ from 'jquery';
import SessionPage from 'Components/Common/SessionHandling/SessionPage';

@connect((store) => {
    return {
        user: store.user,
        whoami:store.whoami
    };
})
class Header extends React.Component {
    constructor(props) {
        super(props);
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(fetchWhoAmI());
        this.props.dispatch(fetchUser());
    }

    componentWillMount() {
        
    }

    ConstructContactForAPI() {
        var users = [];
         if (this.props.user.userContactForAPI != undefined) {
            var userContactForAPI = this.props.user.userContactForAPI;
            if (userContactForAPI.technicalContactFor.length != 0 && userContactForAPI.functionalContactFor != 0) {
                userContactForAPI.technicalContactFor.map(function (item,index) {
                    var isAPIActive=  item.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>
                        users.push(<div key={"technical "+ index.toString()}><label>{item.userName} API Key:</label> <p>{item.apiKey} {isAPIActive}</p></div>)
                })
                userContactForAPI.functionalContactFor.map(function (item,index) {
                    var isAPIActive=  item.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>
                        users.push(<div key={"functional "+index.toString()}><label>{item.userName} API Key:</label> <p>{item.apiKey} {isAPIActive}</p></div>)
                })
                return users;

            }

            if (userContactForAPI.technicalContactFor.length != 0) {
                userContactForAPI.technicalContactFor.map(function (item,index) {
                    var isAPIActive=  item.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>
                        users.push(<div key={"technical "+ index.toString()} ><label>{item.userName} API Key:</label> <p>{item.apiKey} {isAPIActive}</p></div>)
                        })
                return users;
            }

            if (userContactForAPI.functionalContactFor != 0) {
                userContactForAPI.functionalContactFor.map(function (item,index) {
                    var isAPIActive=  item.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>
                        users.push(<div key={"functional "+index.toString()}><label>{item.userName} API Key:</label> <p>{item.apiKey} {isAPIActive}</p></div>)
                    })

                return users;
            }
        }
    }

 
    render() {

        var userFullName, apiKey, userName = "";
        var isAPIActive, userlogo, version = null;
        if (this.props.user.userPermission != undefined) {
            userFullName = "  " + this.props.user.userPermission.firstName + " " + this.props.user.userPermission.lastName;
            userlogo = <i class="fa fa-user"></i>;
            userName = this.props.user.userPermission.userName;
            apiKey = this.props.user.userPermission.api.apiKey;
            isAPIActive = this.props.user.userPermission.api.isActive ? "" : <span class="header-inactiveApi"> (Inactive)</span>;
        }

        if (this.props.whoami.HostingDetails != undefined) {
          version = <p class="header-version">Version: {this.props.whoami.HostingDetails.DeployedVersion}</p>
        }

        const popoverClickRootClose = (
            <Popover id="popover-trigger-click-root-close" class="headerpopover-content">
                <label>User Name: </label> <p>{userName} </p>
                <label>API Key: </label><p>{apiKey} {isAPIActive}  </p>
                {this.ConstructContactForAPI()}
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
                        <Col md={10} lg={10}>
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
                        <Col md={10} lg={10}>
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
