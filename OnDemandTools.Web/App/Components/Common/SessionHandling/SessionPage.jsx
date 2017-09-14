import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import $ from 'jquery';
import SessionAlertModal from 'Components/Common/SessionHandling/SessionAlertModal';
import * as ConfigActions from 'Actions/Config/ConfigActions';
import { fetchConfig } from 'Actions/Config/ConfigActions';

@connect((store) => {
    return {
        config: store.config,
        serverPollTime: store.serverPollTime,  // gets updated when server action happens.
        applicationError:store.applicationError
    };
})

class SessionPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = ({
            showSessionModel: false,
            isSessiontimeSet: false,
            activeSessionRemainingSeconds: "",
            secondsBeforeSessionEnd: 180// No of seconds to verify before SessionEndTime
        });
        this.SessionStartTime = "";
        this.SessionEndTime = new Date();
        this.SessionExpirationTimeMinutes = "";
        this.APIError="";
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(fetchConfig());
        setInterval(this.CheckIdleTime.bind(this), 1000);  // Timer for every 1 second
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        if (this.props.config.sessionExpirationTimeMinutes != undefined && !this.state.isSessiontimeSet) {
            this.SessionExpirationTimeMinutes = this.props.config.sessionExpirationTimeMinutes;
            this.SessionStartTime = new Date();
            var minutesLater = new Date();
            minutesLater.setMinutes(minutesLater.getMinutes() + (this.SessionExpirationTimeMinutes));
            this.SessionEndTime = minutesLater;
            this.APIError=this.props.applicationError.toString();
            this.setState({ isSessiontimeSet: true });
        }

        if (this.state.isSessiontimeSet) {
          
            //console.log("nextProps.serverPollTime :"+nextProps.serverPollTime);
            this.APIError=nextProps.applicationError.toString();
            var slideexpirationSeconds = Math.round((nextProps.serverPollTime - this.SessionStartTime) / 1000);   // converting time into seconds
            if (slideexpirationSeconds > Math.round((this.SessionExpirationTimeMinutes / 2) * 60))   // calculation based on this  reference https://msdn.microsoft.com/en-us/library/system.web.configuration.formsauthenticationconfiguration.slidingexpiration(v=vs.110).aspx
            {
                this.SessionStartTime = new Date();
                var minutesLater = new Date();
                minutesLater.setMinutes(minutesLater.getMinutes() + this.SessionExpirationTimeMinutes);
                this.SessionEndTime = minutesLater;
            }
        }
    }

    //<summary>
    ///  to check when server request has happend
    ///</summary>
    CheckIdleTime() {
        if(this.APIError.includes("Network"))
            {
            console.log(" inside if APIerror "+this.APIError);
            window.location.reload();
            }
           // window.location.replace("/account/logoff");
       
        var datetimeNow = new Date();
        this.setState({ activeSessionRemainingSeconds: Math.round((this.SessionEndTime - datetimeNow) / 1000) }); // converting time into seconds
        // console.log(" activeSessionRemainingSeconds : " +this.state.activeSessionRemainingSeconds, this.SessionEndTime   )
        if (this.state.activeSessionRemainingSeconds == this.state.secondsBeforeSessionEnd) {
            this.setState({ showSessionModel: true });
        }

        if (this.state.activeSessionRemainingSeconds < 0) {
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
    handleContinueSession() {
        this.props.dispatch(ConfigActions.healthCheck())
            .then(() => {
                this.setState({ showSessionModel: false });
            }).catch(error => {
                console.log("error " + error)
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