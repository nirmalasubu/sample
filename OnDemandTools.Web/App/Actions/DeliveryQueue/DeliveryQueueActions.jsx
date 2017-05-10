import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';


// Future  we get data through Axios or fetch
const  queue= [];
const  notificationHistory=[];

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


export const fetchNotificationHistorySuccess = (notificationHistory) => {
    return {
        type: actionTypes.FETCH_NOTIFICATIONHISTORY_SUCCESS,
        notificationHistory
    }
};

export const fetchNotificationHistory = (name) => {
    return (dispatch) => {
        return Axios.get('/api/deliveryqueue/notificationhistory/'+name)        
          .then(response => { 
              dispatch(fetchNotificationHistorySuccess(response.data))
          })
          .catch(error => {
              throw(error);
          });
    };
};

export const clearNotificationHistory = (name) => {
    return (dispatch) => {
        fetchNotificationHistorySuccess(notificationHistory)         
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
    return Axios.delete('/api/deliveryqueue/purge/' + name)        
          .then(response => { 
              
          })
          .catch(error => {
              throw(error);
          });
};

export const clearQueues = (name) => {
    return Axios.delete('/api/deliveryqueue/clear/' + name)        
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


