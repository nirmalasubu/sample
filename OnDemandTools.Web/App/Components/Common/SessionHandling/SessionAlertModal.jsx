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
            seconds:"" 
        });
    }

    componentWillReceiveProps(nextProps) {
        this.setState({seconds:nextProps.data.activeSessionRemainingSeconds});
    }

    //<summary>
    ///  when the pop up is bubbled up
    ///</summary>
    onOpenModal(){ 
        this.setState({
            seconds:this.props.data.activeSessionRemainingSeconds
        });
    }

    //<summary>
    ///  when users yes to continue the session
    ///</summary>
    onContinue(){
        clearInterval(this.timer);
        this.props.handleContinueSession();
    }

    ///<summary>
    ///  when users clicks No
    ///</summary>
    onCancel(){
        clearInterval(this.timer);
        this.props.handleClose();
    }

    render(){
        let minutes="";
        minutes=this.props.data.secondsBeforeSessionEnd>0?Math.floor(this.props.data.secondsBeforeSessionEnd/60):0;
        return (
           <Modal backdrop="static" show={this.props.data.showSessionModel} onEntering={this.onOpenModal.bind(this)} onHide={this.props.handleClose}> 
                <Modal.Header closeButton>
                    <Modal.Title>
           Warning !
                    </Modal.Title>
                </Modal.Header>        
                <Modal.Body>
           {
                        <p>Your session is about to expire in {minutes} minutes. Do you want to extend the session?</p>
           }
                </Modal.Body>
                <Modal.Footer>
                    <Button  onClick={(event) => this.onCancel(event)}>No</Button>  
                    <Button bsStyle="primary" onClick={(event) => this.onContinue(event)}>Extend</Button>
                </Modal.Footer>
           </Modal>
        )
                    
    }
}

export default SessionAlertModal;