import React from 'react';
import { connect } from 'react-redux';
import $ from 'jquery';
import { Modal } from 'react-bootstrap';
import { ModalDialogue } from 'react-bootstrap';
import { ModalBody } from 'react-bootstrap';
import { ModalFooter } from 'react-bootstrap';
import { ModalHeader } from 'react-bootstrap';
import { ModalTitle } from 'react-bootstrap';
import { Button } from 'react-bootstrap';


class SessionAlertModal extends React.Component
{
    constructor(props) {
        super(props);

        this.state = ({
            message: "",
            time: {}, 
            seconds:120 
        });
        
        this.timer = 0;
        this.startTimer = this.startTimer.bind(this);
        this.countDown = this.countDown.bind(this);
    }


    //<summary>
    ///  converts seconds to time
    ///</summary>
    secondsToTime(secs){
      
        let hours = Math.floor(secs / (60 * 60));

        let divisor_for_minutes = secs % (60 * 60);
        let minutes = Math.floor(divisor_for_minutes / 60);

        let divisor_for_seconds = divisor_for_minutes % 60;
        let seconds = Math.ceil(divisor_for_seconds);

        let obj = {
            "h": hours,
            "m": minutes,
            "s": seconds
        };
        return obj;
    }

    componentDidMount() {
      
        let timeLeftVar = this.secondsToTime(this.state.seconds);
        this.setState({ time: timeLeftVar });
    }

    startTimer() {
        if (this.timer == 0) {
            this.timer = setInterval(this.countDown.bind(this), 1000);  //calls every second
        }
    }

    countDown() {
        // Remove one second, set state so a re-render happens.
      
        let seconds = this.state.seconds - 1;
        this.setState({
            time: this.secondsToTime(seconds),
            seconds: seconds,
        });
    
        // Check if we're at zero.
        if (seconds == 0) { 
            clearInterval(this.timer);
        }
    }

    //<summary>
    ///  when the pop up is bubbled up
    ///</summary>
    onOpenModal(){ 
        this.timer=0;
        this.setState({
            message:"Session about to end. you will be logged out, if no server request is made. Would you like to continue?",
            time: {}, 
            seconds:120 
        });
        let timeLeftVar = this.secondsToTime(this.state.seconds);
        this.setState({ time: timeLeftVar });
        this.startTimer();
       
    }

    //<summary>
    ///  when users yes to continue the session
    ///</summary>
    onContinue(){
        clearInterval(this.timer);
        this.props.handleContinueSession();
    }

    onCancel(){
        clearInterval(this.timer);
        this.props.handleClose();
    }

    render(){
       
        return (
           <Modal show={this.props.data.showSessionModel} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}> 
                <Modal.Header closeButton>
                    <Modal.Title>
           Warning !
                    </Modal.Title>
                </Modal.Header>        
                <Modal.Body>
           {
                        <p>In {this.state.time.m} minutes: {this.state.time.s} seconds {this.state.message}</p>
           }
                </Modal.Body>
                <Modal.Footer>
                    <Button  onClick={(event) => this.onCancel(event)}>NO</Button>  
                    <Button bsStyle="primary" onClick={(event) => this.onContinue(event)}>YES</Button>
                </Modal.Footer>
           </Modal>
        )
                    }
                    }

export default SessionAlertModal;