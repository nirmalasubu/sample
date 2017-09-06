import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import $ from 'jquery';
import SessionAlertModal from 'Components/Common/SessionHandling/SessionAlertModal';
import  * as ConfigActions from 'Actions/Config/ConfigActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';

@connect((store) => {
    return {
        config: store.config,
        serverPollTime:store.serverPollTime  // gets updated when server action happens.
    };
})
class SessionPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = ({
            showSessionModel: false});
        this.SessionStartTime =  new Date();
        this.SessionEndTime =  new Date();
        this.SessionExpirationTime=0;
    }

    //called on the page load
    componentDidMount() {
        this.SessionExpirationTime=this.props.data;
        this.SessionStartTime =  new Date();
        var minutesLater = new Date();
        minutesLater.setMinutes(minutesLater.getMinutes() +  this.SessionExpirationTime);
        this.SessionEndTime =  minutesLater;      
        setInterval(this.CheckIdleTime.bind(this), 60000);  // Timer for every 1 minutes
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        var slideexpirationMinutes = Math.round((( (nextProps.serverPollTime - this.SessionStartTime ) % 86400000) % 3600000) / 60000); 
        if(slideexpirationMinutes>=( Math.round(this.SessionExpirationTime)/2))
        {
            this.SessionStartTime =  new Date();
            var minutesLater = new Date();
            minutesLater.setMinutes(minutesLater.getMinutes() +  this.SessionExpirationTime);
            this.SessionEndTime =  minutesLater;
        }
      
    }

    //<summary>
    ///  to check when server request has happend
    ///</summary>
    CheckIdleTime() {
     
        var today = new Date();
        var serverPolledTime = new Date(this.props.serverPollTime);  
      
        var activeSessionRemainingMinutes = Math.round((( (this.SessionEndTime - today ) % 86400000) % 3600000) / 60000); 

        var minutesBeforeSessionEnd=2; // No of minutes to verify before SessionEndTime
        var sessionTime=Math.round((((this.SessionEndTime - this.SessionStartTime) % 86400000) % 3600000) / 60000);
    
        if (activeSessionRemainingMinutes == minutesBeforeSessionEnd) {
            this.setState({ showSessionModel: true });
        }
      
        if (activeSessionRemainingMinutes <= 0) {          
            window.location.reload();
        }
    }


    //<summary>
    ///  session would be ended and take back to login page 
    ///</summary>
    closeSessionModel() {
        this.setState({ showSessionModel: false }); 
    
    }

    //<summary>
    ///  Re Intiate the session .when the user clicks yes
    ///</summary>
    handleContinueSession()
    {
       
        this.props.dispatch(ConfigActions.healthCheck())
           .then(() => {
               this.setState({ showSessionModel: false });
               this.SessionStartTime =  new Date();  // reset the value once user wishes to continue
               var minutesLater = new Date();
               minutesLater.setMinutes(minutesLater.getMinutes() + 6); // Assuming expire time span is 6 minutes in startup.cs
               this.SessionEndTime =  minutesLater;
           }).catch(error => {
               console.log("error "+ error)
               this.setState({ showSessionModel: false });
               window.location.reload();
           });
        
         
    }

    render() {
        return (
            <div >
                 <SessionAlertModal data={this.state} handleContinueSession={this.handleContinueSession.bind(this)} handleClose={this.closeSessionModel.bind(this)} />
            </div>
        )
    }
}



export default SessionPage;