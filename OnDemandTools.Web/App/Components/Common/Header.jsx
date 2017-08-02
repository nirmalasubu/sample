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
        var serverPolledTime = new Date(this.props.serverPollTime);      
        var activeSessionRemainingMinutes = Math.round((( (this.SessionEndTime - today ) % 86400000) % 3600000) / 60000); 

        console.log("activeSessionRemainingMinutes"+activeSessionRemainingMinutes);
      
         var minutesBeforeSessionEnd=2; // No of minutes to verify before SessionEndTime
         var sessionTime=Math.round((((this.SessionEndTime-   this.SessionStartTime) % 86400000) % 3600000) / 60000);
       
         if (activeSessionRemainingMinutes < minutesBeforeSessionEnd) { // verify the session before "minutesBeforeSessionEnd" session end time

            var diffMs = (today -   serverPolledTime );
            var diffsMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); 
            console.log("serverPolledTime"+serverPolledTime);
            console.log("diffMins "+ diffsMins);
            if (diffsMins > Math.round(sessionTime/2)  &&  diffsMins < sessionTime)   // server is not yet polled. So show the pop up to make it active
            {
                this.setState({ showSessionModel: true });
                console.log("session about time out in 2 minute ");
            }else if(diffsMins < 0 ||  diffsMins > sessionTime)
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
               sixMinutesLater.setMinutes(sixMinutesLater.getMinutes() + 6); // Assuming expire time span is 6 minutes in startup.cs
               this.SessionEndTime =  sixMinutesLater;
           }).catch(error => {
               console.log("error "+ error)
               this.setState({ showSessionModel: false });
               window.location.reload();
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