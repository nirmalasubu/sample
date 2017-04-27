import React from 'react';
import { connect } from 'react-redux';
import * as queueActions from 'Actions/DeliveryQueue/DeliveryQueueActions';
import DeliveryQueueFilter from 'Components/DeliveryQueues/DeliveryQueueFilter';
import $ from 'jquery';


class Queue extends React.Component {

    constructor(props) {
        super(props);
        // this.sortColumn = this.sortColumn.bind(this);
        this.state = {
            stateQueue: [],
            sortClass: [{ "friendlyName": "fa fa-sort" },
            { "hoursOut": "fa fa-sort" },
            { "contactEmailAddress": "fa fa-sort" }],
            filterValue : {
                queueName : "",
                contactName : "",
                queueId : ""
            }
        }
    }

    handleFilterUpdate(filtersValue, type) {
            var valuess = this.state.filterValue;
            if(type=="QN")
                valuess.queueName = filtersValue;

            if(type=="CN")
                valuess.contactName = filtersValue;

            if(type=="QI")
                valuess.queueId = filtersValue;

            if(type=="CL")
            {
                this.state.filterValue.queueName="";
                this.state.filterValue.contactName="";
                this.state.filterValue.queueId="";
            }

            this.setState({
                filterValue: valuess
            });

            if(this.state.filterValue.queueName!="" || this.state.filterValue.contactName!="" || this.state.filterValue.queueId!="")
            {
                var friendlyName = this.state.filterValue.queueName.toLowerCase();
                var contactName = this.state.filterValue.contactName.toLowerCase();
                var queueId = this.state.filterValue.queueId.toLowerCase();
                
                var queueArray = $.grep(this.props.queues, function(v) {
                    return ((friendlyName !=""?v.friendlyName.toLowerCase().indexOf(friendlyName) != -1:true) 
                        && (contactName!=""?v.contactEmailAddress.toLowerCase().indexOf(contactName) != -1:true)
                        && (queueId!=""?v.name.toLowerCase().indexOf(queueId) != -1:true));                    
                });
                this.setState({
                    stateQueue: queueArray
                });
            }
            else
                this.setState({
                    stateQueue: this.props.queues
                });
        
    }

    //called on the page load
    componentDidMount() {
        let promise = this.props.fetchQueue();
        promise.then(newqueue => {
            this.setState({
                stateQueue: this.props.queues
            })
        });
        document.title = "ODT - Delivery Queues";
    }

    sortString(Name) {
        var queueArray = [];
        var sortDesc = false;
        this.state.sortClass.map(function (css) {
            for (var key in css) {
                if(key==Name)
                {
                    if(css[key]=="fa fa-sort-asc" )
                    { 
                        css[key]="fa fa-sort-desc"
                        sortDesc=true;
                    }
                    else {
                        css[key] = "fa fa-sort-asc"
                    }
                } else {
                    css[key] = "fa fa-sort"
                }

            }
        })

        if (sortDesc) {
            queueArray = this.props.queues.sort((a, b) => {
                var nameA = a[Name].toLowerCase(), nameB = b[Name].toLowerCase()
                if (nameA > nameB) //sort string descending
                    return -1
                if (nameA < nameB)
                    return 1
                return 0
            })
        }
        else {
            queueArray = this.props.queues.sort((a, b) => {
                var nameA = a[Name].toLowerCase(), nameB = b[Name].toLowerCase()
                if (nameA < nameB) //sort string ascending
                    return -1
                if (nameA > nameB)
                    return 1
                return 0
            })
        }
        this.setState({
            stateQueue: queueArray

        })
    }


    sortNumber(Value) {
        var queueArray = [];
        var sortDesc = false;
        this.state.sortClass.map(function (css) {
            for (var key in css) {
                if (key == Value) {
                    if (css[key] == "fa fa-sort-asc") {
                        css[key] = "fa fa-sort-desc"
                        sortDesc = true;
                    }
                    else {
                        css[key] = "fa fa-sort-asc"
                    }
                } else {
                    css[key] = "fa fa-sort"
                }

            }
        })

        if (sortDesc) {
            queueArray = this.props.queues.sort((a, b) => {
                return a[Value] - b[Value]
            })
        }
        else {
            queueArray = this.props.queues.sort((a, b) => {
                return b[Value] - a[Value]
            })
        }
        this.setState({
            stateQueue: queueArray

        })
    }

    render() {
        return (

            <div>
                  <label className="queue-head">Delivery Queue Page</label>
                  <hr/>
                <DeliveryQueueFilter updateFilter={this.handleFilterUpdate.bind(this)} />

                <div className="row">                   
                    <div className="col-xs-12">
                        <table id="queueTable" className="table table-hover table-striped table-bordered table-responsive">
                            <thead>
                                <tr>
                                    <th onClick={this.sortString.bind(this, 'friendlyName')}>Queue Name <i class={this.state.sortClass[0].friendlyName} /></th>
                                    <th onClick={this.sortNumber.bind(this, 'hoursOut')} className="queue-table-th-advanceddevlivery" >Advanced Delivery  <i class={this.state.sortClass[1].hoursOut} /></th>
                                    <th onClick={this.sortString.bind(this, 'contactEmailAddress')}>Contact  <i class={this.state.sortClass[2].contactEmailAddress} /></th>
                                    <th  >Remote Queue</th>
                                    <th >Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {this.state.stateQueue.map((item, index) => {
                                    return (
                                        <tr key={index}>
                                            <td  >{item.friendlyName}</td>
                                            <td className="queue-table-th-advanceddevlivery">{item.hoursOut}</td>
                                            <td ><p data-toggle="tooltip" title={item.contactEmailAddress}>{item.contactEmailAddress}</p></td>

                                            <td  >
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
                                            <td><i class="fa fa-search" aria-hidden="true"></i> <i class="fa fa-calendar" aria-hidden="true"></i></td>
                                        </tr>
                                    );
                                })}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

        )
    }
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
