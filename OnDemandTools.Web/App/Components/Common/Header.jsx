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
        serverPollTime:store.serverPollTime
    };
})
class Header extends React.Component {
    constructor(props) {
        super(props);
      
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(fetchUser());
        this.props.dispatch(fetchConfig());
        this.props.dispatch(fetchStatus());
        setInterval(this.CheckIdleTime.bind(this), 60000);  // Timer for every 1 minutes
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        console.log("serverPollTime "+nextProps.serverPollTime)
    }
    
    CheckIdleTime() {
       
        var today = new Date();
        var servertime = new Date(this.props.serverPollTime);
        var diffMs = ( today - servertime);
        var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); 
        console.log("currenttime "+ today);
        console.log("diffMins "+ diffMins);
        if (diffMins >= 2) {
            alert("Time expired!");  // Need design a modal popup instead of alert. 
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