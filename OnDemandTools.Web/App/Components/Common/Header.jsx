import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import { fetchUser } from 'Actions/User/UserActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';
import { fetchStatus } from 'Actions/Status/StatusActions';
import $ from 'jquery';
import SessionAlertModal from 'Components/Common/SessionAlertModal';
import  * as categoryActions from 'Actions/Category/CategoryActions';

@connect((store) => {
    return {
        user: store.user,
        serverPollTime:store.serverPollTime  // gets updated when server action happens.
    };
})
class Header extends React.Component {
    constructor(props) {
        super(props);
        this.state = ({
            showSessionModel: false});
        this.SessionStartTime =  new Date();
        this.SessionEndTime =  new Date();
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(fetchUser());
        this.props.dispatch(fetchConfig());
        this.props.dispatch(fetchStatus());
        this.SessionStartTime =  new Date();
        var sixMinutesLater = new Date();
        sixMinutesLater.setMinutes(sixMinutesLater.getMinutes() + 6);
        this.SessionEndTime =  sixMinutesLater;
        console.log("SessionStartTime" +this.SessionStartTime);
        console.log("SessionEndTime"+ this.SessionEndTime);
        setInterval(this.CheckIdleTime.bind(this), 60000);  // Timer for every 1 minutes
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        console.log("serverPollTime "+nextProps.serverPollTime)
    }
    

    //<summary>
    ///  to check when server request has happend
    ///</summary>
    CheckIdleTime() {
      
        var today = new Date();
        var serverActiveTime = new Date(this.props.serverPollTime);
        var diffMs = ( today -  this.SessionStartTime );
        var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); 
        //var diffMs = (  this.SessionStartTime - serverActiveTime);
        //var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); 

        console.log("currenttime "+ today);
        console.log("diffMins "+ diffMins);
        if (diffMins > 3) { // session is idle more than 2 minutes show a pop up warning  that means it cross half the slide expiration time of the server

            var diffsMs = (this.SessionEndTime -   serverActiveTime );
            var diffsMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); 
            if (diffsMins > 3  &&  diffsMins <= 5)
            {
                this.setState({ showSessionModel: true });
                console.log("session about time out in 2 minute ");
            }else if(diffsMins < 0 ||  diffsMins > 5)
            {
                console.log("session timed out");
            }
          
        }
    }


    //<summary>
    ///  session would be ended and take back to login page 
    ///</summary>
    closeSessionModel() {
        this.setState({ showSessionModel: false }); 
        window.location.reload();
    }

    //<summary>
    ///  Re Intiate the session .when the user clicks yes
    ///</summary>
    handleContinueSession()
    {
        this.props.dispatch(categoryActions.healthCheck())
           .then(() => {
               this.setState({ showSessionModel: false });
               this.SessionStartTime =  new Date();  // reset the value once user wishes to continue
               var sixMinutesLater = new Date();
               sixMinutesLater.setMinutes(sixMinutesLater.getMinutes() + 6);
               this.SessionEndTime =  sixMinutesLater;
           }).catch(error => {
               console.log("error "+ error)
               this.setState({ showSessionModel: false });
               //window.location.reload();
           });
        
         
    }

    render() {
        return (
            <div >
                <Image src="../images/ODTLogo.png" rounded />
                <div style={alignLeft} >
                    <h4> Welcome {this.props.user}! <a href="/account/logoff" >Logout</a>&nbsp;&nbsp;&nbsp;</h4>
                </div>
                <hr />
                 <SessionAlertModal data={this.state} handleContinueSession={this.handleContinueSession.bind(this)} handleClose={this.closeSessionModel.bind(this)} />
            </div>
        )
    }
}

const alignLeft = { float: 'right' };

export default Header;