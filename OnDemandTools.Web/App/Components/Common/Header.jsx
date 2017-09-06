import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';
import { fetchStatus } from 'Actions/Status/StatusActions';
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
        this.props.dispatch(fetchStatus());
    }

    render() {
        let session="";
        if(this.props.config.sessionExpirationTime!=undefined)
        {
            session= <SessionPage data={this.props.config.sessionExpirationTime}/>
        }
        return (
            <div >
                <Image src="../images/ODTLogo.png" rounded />
                <div style={alignLeft} >
                    <h4> Welcome {this.props.user}! <a href="/account/logoff" >Logout</a>&nbsp;&nbsp;&nbsp;</h4>
                </div>
                    {session}
                <hr />
            </div>
        )
    }
}

const alignLeft = { float: 'right' };

export default Header;