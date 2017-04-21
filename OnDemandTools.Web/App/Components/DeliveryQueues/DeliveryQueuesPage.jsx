import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from '../../actions/deliveryQueue/deliveryQueueActions';


class Queue extends React.Component{

    constructor(props){
        super(props);
    }

    //called on the page load
    componentDidMount() {
        this.props.fetchQueue();
    }


    render(){
        let titleInput;
        return(
          <div>
            <table class="table table-hover table-striped table-bordered">
               <thead >
                 <tr>
                    <th>Queue Name</th>
                    <th>Advanced Delivery</th>
                    <th>Contact</th>
                    <th>Remote Queue</th>
                    <th>Actions</th>
                    </tr>
              </thead>
              <tbody>
       {this.props.queues.map((item, index) => {
           return (
             <tr key={index}>
                        <td>{item.friendlyQueueName}</td>
                        <td>{item.advancedDeliery}</td>
                         <td>{item.email}</td>
                        <td>{item.queueName}</td>
                        <td></td>
                      </tr>
                    );
    })}
                </tbody>
            </table>
      </div>

                     )}
                }               


 // Maps state from store to props
const mapStateToProps = (state, ownProps) => {
    return {
// You can now say this.props.queues
        queues: state.queues
    }
};

// Maps actions to props
  const mapDispatchToProps = (dispatch) => {
  return {
    fetchQueue: () => dispatch(queueActions.fetchQueues())
 };
 };

 // Use connect to put them together
export default connect(mapStateToProps, mapDispatchToProps)(Queue);
