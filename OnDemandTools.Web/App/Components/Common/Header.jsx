import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';
import { fetchStatus } from 'Actions/Status/StatusActions';
import $ from 'jquery';

@connect((store) => {
    return {
        user: store.user,
    };
})
class Header extends React.Component {
    constructor(props) {
        super(props);
        this.state=({IDLE_TIMEOUT:360,
                     _idleSecondsCounter:0});
      
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(fetchUser());
        this.props.dispatch(fetchConfig());
        this.props.dispatch(fetchStatus());
        setInterval(this.CheckIdleTime.bind(this), 1000);
    }


    CheckIdleTime() {
        //alert("Time expired!");
        var count=this.state._idleSecondsCounter + 1;
        this.setState ({_idleSecondsCounter:count});
        console.log("this.state._idleSecondsCounter "+this.state._idleSecondsCounter)
        if (this.state._idleSecondsCounter >= this.state.IDLE_TIMEOUT) {
            alert("Time expired!");
        }
    }

    render() {
        return (
            <div >
                <Image src="../images/ODTLogo.png" rounded />
                <div style={alignLeft} >
                    <h4> Welcome {this.props.user}! <a href="/account/logoff" >Logout</a>&nbsp;&nbsp;&nbsp;</h4>
                </div>
                <hr />
            </div>
        )
    }
}

const alignLeft = { float: 'right' };

export default Header;