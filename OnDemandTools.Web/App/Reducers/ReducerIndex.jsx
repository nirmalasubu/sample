import { combineReducers } from 'redux';

import {DeliveryQueueReducer} from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import {UserReducer} from 'Reducers/User/UserReducer';

export default combineReducers({
   
    queues:DeliveryQueueReducer,
    user:UserReducer
    // More reducers if there are
    // can go here
});