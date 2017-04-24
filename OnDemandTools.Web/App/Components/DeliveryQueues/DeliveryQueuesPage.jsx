import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';
import $ from 'jquery';
import '../../../node_modules/tablesorter/dist/js/jquery.tablesorter.js';
import '../../../node_modules/jquery/src/jquery.js';
class Queue extends React.Component{

    constructor(props){
        super(props);
      
    }

    //called on the page load
    componentDidMount() {
        $("#myTable").tablesorter(); 
        $("#myTable").tablesorter({ sortList: [[0,0], [1,0]] });
        this.props.fetchQueue();
       
    }

  

    render(){
        let titleInput;
        return(
          <div>
            <table id="myTable" class="table table-hover table-striped table-bordered tablesorter">
               <thead >
                 <tr>
                    <th >Queue Name</th>
                    <th>Advanced Delivery</th>
                    <th>Contact</th>
                    <th >Remote Queue</th>
                    <th>Actions</th>
                    </tr>
              </thead>
              <tbody>
       {this.props.queues.map((item, index) => {
           return (
             <tr key={index}>
                        <td>{item.friendlyName}</td>
                        <td>{item.hoursOut}</td>
                         <td><p data-toggle="tooltip" title={item.contactEmailAddress}>{item.contactEmailAddress}</p></td>
                        <td>{item.name}</td>
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
