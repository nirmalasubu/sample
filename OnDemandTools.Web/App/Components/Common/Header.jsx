import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';
import { connect } from 'react-redux';
import * as userActions from 'Actions/User/UserActions';
import $ from 'jquery';

class Header extends React.Component{
    constructor(props){
        super(props);

        this.state = {
            name:"Guest"
        } 
    }
    
    //called on the page load
    componentDidMount() {       
        let promise =  this.props.fetchUser();
        promise.then(newuser => {
            this.setState({
                name: this.props.user
            })
        });
      
    }    

    render(){
        return (
        <div >
                <Image src="../images/ODTLogo.png" rounded />
                <div style={alignLeft} >
                   <h4> Welcome { this.state.name }! <a href="/account/logoff" >Logout</a>&nbsp;&nbsp;&nbsp;</h4>
                </div>
            <hr/>
        </div>
    )
    }
}

const alignLeft = { float: 'right' };

// Maps state from store to props
const mapStateToProps = (state, ownProps) => {   
    return {
        // You can now say this.props.user        
        user: state.user
    }
};

// Maps actions to props
const mapDispatchToProps = (dispatch) => {    
    return {
        fetchUser: () => dispatch(userActions.fetchUser())
    };
};

// Use connect to put them together
export default connect( mapStateToProps,mapDispatchToProps)(Header);