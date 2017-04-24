import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';

// Future  we get data through Axios or fetch
const  queue= [
                {
                    queueName: 1,
                    advancedDeliery: 'List item 1',
                    email: 'List item 1',
                    friendlyQueueName:'TBS',
                    firstName:'TND',
                    lastName:'dfds'
                },
                {
                    queueName: 2,
                    advancedDeliery: 'List item 2',
                    email: 'List item 2',
                    friendlyQueueName:'MEDIUM', 
                    firstName:'TND',
                    lastName:'dfds'
                },
                {
                    queueName: 3,
                    advancedDeliery: 'List item 3',
                    email: 'List item 3',
                    friendlyQueueName:'CONOE',
                    firstName:'TND',
                    lastName:'dfds'
                },
                {
                    queueName: 4,
                    advancedDeliery: 'List item 4',
                    email: 'List item 4',
                    friendlyQueueName:'ENCODING',
                    firstName:'TND',
                    lastName:'dfds'
                }
];

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
              console.log(response.data);
              dispatch(fetchQueuesSuccess(response.data))
          })
          .catch(error => {
              throw(error);
          });
    };
};