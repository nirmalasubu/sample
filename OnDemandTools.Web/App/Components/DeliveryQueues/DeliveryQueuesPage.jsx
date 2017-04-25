import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';
import $ from 'jquery';


class Queue extends React.Component{

    constructor(props){
        super(props);
       // this.sortColumn = this.sortColumn.bind(this);
        this.state = {
            statequeue:[]
        }
      
    }

    //called on the page load
    componentDidMount() {       
        let promise =  this.props.fetchQueue();
        promise.then(newqueue => {
            this.setState({
                statequeue: this.props.queues
            })
            });
      
    }
    
    sortColumn(Name)  {
        var queuearray=[];

        queuearray= this.props.queues.sort((a, b) => {         
            var nameA=a[Name].toLowerCase(), nameB=b[Name].toLowerCase()                     
            if (nameA < nameB) //sort string ascending
                return -1 
            if (nameA > nameB)
                return 1
            return 0
        })
        this.setState({
            statequeue: queuearray
        })
    }

    render(){      
        return(
          <div>
            <table id="myTable2" className="table table-hover table-striped table-bordered queue-table">
               <thead >
              <tr>
                    <th  className="queue-table-th-name" onClick={this.sortColumn.bind(this,'friendlyName')}>Queue Name</th>
                    <th  className="queue-table-th-advanceddevlivery">Advanced Delivery</th>
                    <th  className="queue-table-th-email" onClick={this.sortColumn.bind(this,'contactEmailAddress')}>Contact</th>
                    <th  className="queue-table-th-queue">Remote Queue</th>
                    <th  className="queue-table-th-actions">Actions</th>
                    </tr>
              </thead>
              <tbody>
            {this.state.statequeue.map((item, index) => {
           return (
             <tr key={index}>
                        <td  className="queue-table-th-name">{item.friendlyName}</td>
                        <td  className="queue-table-th-advanceddevlivery">{item.hoursOut}</td>
                         <td  className="queue-table-th-email"><p data-toggle="tooltip" title={item.contactEmailAddress}>{item.contactEmailAddress}</p></td>

                        <td  className="queue-table-th-queue">
                            <p>
                            {item.name}
                        <br />
                        <br />
                        <i>Delivery</i>: 0
                        <button class="btn-xs btn-link" title="clear pending deliveries to queue">
                            Clear
                        </button>
                           <button class="btn-xs btn-link" title="clear pending deliveries to queue">
                               Resend
                           </button>
                        <br />
                        <i>Consumption</i>: 21
                        <button class="btn-xs btn-link">
                            Purge
                        </button>
                        <span class="small">-Apr 25, 2017 2:24 PM</span>
                        </p>
                         </td>
                        <td  className="queue-table-th-actions"></td>
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
export default connect( mapStateToProps,mapDispatchToProps)(Queue);
