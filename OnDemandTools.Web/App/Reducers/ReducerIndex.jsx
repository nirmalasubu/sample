import { combineReducers } from 'redux';

import {DeliveryQueueReducer} from 'Reducers/DeliveryQueue/DeliveryQueueReducer'

export default combineReducers({
   
    queues:DeliveryQueueReducer
    // More reducers if there are
    // can go here
});