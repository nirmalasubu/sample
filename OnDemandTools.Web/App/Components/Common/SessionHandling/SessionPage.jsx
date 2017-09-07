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
            showSessionModel: false,
        isSessiontimeSet:false});
        this.SessionStartTime ="";
        this.SessionEndTime ="";
        this.SessionExpirationTime="";
    }

   
    //called on the page load
    componentDidMount() {
        console.log(" componentDidMount" );
       this.props.dispatch(fetchConfig());
      setInterval(this.CheckIdleTime.bind(this), 60000);  // Timer for every 1 minutes
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
       
        if(this.props.config.sessionExpirationTime!=undefined && !this.state.isSessiontimeSet)
        {
           
            this.SessionExpirationTime=this.props.config.sessionExpirationTime;
            this.SessionStartTime =  new Date();
            var minutesLater =new Date();
            minutesLater.setMinutes(minutesLater.getMinutes() +  this.SessionExpirationTime);
            this.SessionEndTime =  minutesLater;    
            console.log( " this.SessionStartTime  :"+this.SessionStartTime );
            console.log( " this.SessionEndTime  :"+this.SessionEndTime );
            console.log( " this.SessionExpirationTime  :"+this.SessionExpirationTime );
            this.setState({isSessiontimeSet:true});
        }

        if(this.state.isSessiontimeSet)
        {
            var slideexpirationMinutes = Math.round((( (nextProps.serverPollTime - this.SessionStartTime ) % 86400000) % 3600000) / 60000); 
            console.log(" slideexpirationMinutes :"+  slideexpirationMinutes);
            if(slideexpirationMinutes>(Math.round(this.SessionExpirationTime)/2))
            {
                console.log("inside if")
                this.SessionStartTime =  new Date();
                var minutesLater = new Date();
                minutesLater.setMinutes(minutesLater.getMinutes() +  this.SessionExpirationTime);
                this.SessionEndTime =  minutesLater;
            }
        }
       
      
    }

    //<summary>
    ///  to check when server request has happend
    ///</summary>
    CheckIdleTime() {
        console.log(" CheckIdleTime this.SessionEndTime :"+this.SessionEndTime );
     
        var today = new Date();
        var serverPolledTime = new Date(this.props.serverPollTime);  
      
        var activeSessionRemainingMinutes = Math.round((( (this.SessionEndTime - today ) % 86400000) % 3600000) / 60000); 

        var minutesBeforeSessionEnd=2; // No of minutes to verify before SessionEndTime
        console.log(" CheckIdleTime activeSessionRemainingMinutes :"+activeSessionRemainingMinutes );
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
               console.log("in method"+ this.SessionExpirationTime);
               minutesLater.setMinutes(minutesLater.getMinutes() + this.SessionExpirationTime); // Assuming expire time span is 6 minutes in startup.cs
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