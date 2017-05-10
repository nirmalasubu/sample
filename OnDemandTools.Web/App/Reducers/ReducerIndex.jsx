import { combineReducers } from 'redux';

import {DeliveryQueueReducer,SignalRQueueDataReducer, NotificationHistoryQueueReducer} from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import {UserReducer} from 'Reducers/User/UserReducer';

export default combineReducers({
   
    queues:DeliveryQueueReducer,
    user:UserReducer,
    queueCountData:SignalRQueueDataReducer,
    notificationHistory:NotificationHistoryQueueReducer
    // More reducers if there are
    // can go here
});