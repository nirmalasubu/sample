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
            isSessionExtended:false,
            isSessiontimeSet:false,
            activeSessionRemainingSeconds:""
        });
        this.SessionStartTime ="";
        this.SessionEndTime =new Date();
        this.SessionExpirationTime="";
        this.SecondsBeforeSessionEnd=45; // No of minutes to verify before SessionEndTime
      //  this.activeSessionRemainingSeconds="",
       this.counter=0;
    }

   
    //called on the page load
    componentDidMount() {
       
       this.props.dispatch(fetchConfig());
      setInterval(this.CheckIdleTime.bind(this), 1000);  // Timer for every 1 second
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
   
        if(this.props.config.sessionExpirationTime!=undefined && !this.state.isSessiontimeSet)
        {
           
            this.SessionExpirationTime=this.props.config.sessionExpirationTime;
            this.SessionStartTime =  new Date();
            var minutesLater =new Date();
            minutesLater.setMinutes(minutesLater.getMinutes() +  (this.SessionExpirationTime));
            this.SessionEndTime =  minutesLater;  
            this.setState({isSessiontimeSet:true});
        }

        if(this.state.isSessiontimeSet)
        {   
            console.log(" this.props.serverPollTime :"+  JSON.stringify(nextProps.serverPollTime));
            console.log(" this.SessionStartTime :"+  this.SessionStartTime);
            var slideexpirationSeconds = Math.round( (nextProps.serverPollTime- this.SessionStartTime)/1000);
            console.log(" slideexpirationSeconds :"+  slideexpirationSeconds);
            console.log(" (Math.round(this.SessionExpirationTime)/2) :"+  Math.round(this.SessionExpirationTime/2*60));
           
            if(slideexpirationSeconds > Math.round((this.SessionExpirationTime/2)*60) )   // calculation based on this  reference https://msdn.microsoft.com/en-us/library/system.web.configuration.formsauthenticationconfiguration.slidingexpiration(v=vs.110).aspx
            {
                this.SessionStartTime =  new Date();
                var minutesLater = new Date();
                minutesLater.setMinutes(minutesLater.getMinutes() +  this.SessionExpirationTime);
                this.SessionEndTime =  minutesLater;
                console.log(" inside if this.SessionEndTime  2 :"+this.SessionEndTime);
            }
        }
       
      
    }

    //<summary>
    ///  to check when server request has happend
    ///</summary>
    CheckIdleTime() {
       
        var datetimeNow = new Date();

        this.setState({ activeSessionRemainingSeconds: Math.round((this.SessionEndTime-datetimeNow)/1000) });
       
       
       // this.activeSessionRemainingSeconds =Math.round((this.SessionEndTime-datetimeNow)/1000); 

        //console.log(" activeSessionRemainingSeconds : " +this.activeSessionRemainingSeconds, this.SessionEndTime   )
        console.log(" activeSessionRemainingSeconds : " +this.state.activeSessionRemainingSeconds, this.SessionEndTime   )
        if (this.state.activeSessionRemainingSeconds == this.SecondsBeforeSessionEnd) {
           
            this.setState({ showSessionModel: true });
        }
      
        if (this.state.activeSessionRemainingSeconds <-2) {   // need to check for zero < 0
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
               this.setState({ showSessionModel: false});
           }).catch(error => {
               console.log("error "+ error)
               this.setState({ showSessionModel: false});
               //window.location.reload();
           });
        
        
    }

    render() {
        return (
            <div >
                 <SessionAlertModal data={this.state}  handleContinueSession={this.handleContinueSession.bind(this)} handleClose={this.closeSessionModel.bind(this)} />
            </div>
        )
    }
}



export default SessionPage;