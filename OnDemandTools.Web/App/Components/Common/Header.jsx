import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';

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

    render() {
       
        return (
            <div >
                <Image src="../images/ODTLogo.png" rounded />
                <div style={alignLeft} >
                    <h4> Welcome {this.props.user.firstName + " " + this.props.user.lastName}! <a href="/account/logoff" >Logout</a>&nbsp;&nbsp;&nbsp;</h4>
                </div>
                 
                <hr />
            </div>
        )
    }
}

const alignLeft = { float: 'right' };

export default Header;