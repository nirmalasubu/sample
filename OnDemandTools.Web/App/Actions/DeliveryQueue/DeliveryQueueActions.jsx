import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';


// Future  we get data through Axios or fetch
const  queue= [];

export const fetchQueuesSuccess = (queues) => {
    return {
        type: actionTypes.FETCH_QUEUES_SUCCESS,
        queues
    }
};

export const fetchQueues = () => {
    return (dispatch) => {
        return Axios.get('/api/deliveryqueue')        
          .then(response => { 
              dispatch(fetchQueuesSuccess(response.data))
          })
          .catch(error => {
              throw(error);
          });
    };
};

export const resetQueues = (name) => {
    return Axios.post('/api/deliveryqueue/reset/' + name)        
          .then(response => { 
              
          })
          .catch(error => {
              throw(error);
          });
};

export const purgeQueues = (name) => {
    return Axios.post('/api/deliveryqueue/purge/' + name)        
          .then(response => { 
              
          })
          .catch(error => {
              throw(error);
          });
};

export const clearQueues = (name) => {
    return Axios.post('/api/deliveryqueue/clear/' + name)        
          .then(response => { 
              
          })
          .catch(error => {
              throw(error);
          });
};

export const signalRStart=(signalRdata)=> {
    return {
            type: actionTypes.FETCH_SIGNALRQUEUES_SUCCESS,
            queueCountData:signalRdata};
  
};


