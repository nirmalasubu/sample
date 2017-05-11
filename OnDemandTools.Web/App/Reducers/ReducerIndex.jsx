import { combineReducers } from 'redux';

import {DeliveryQueueReducer,SignalRQueueDataReducer, NotificationHistoryQueueReducer} from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import {UserReducer} from 'Reducers/User/UserReducer';
import {ConfigReducer} from 'Reducers/Config/ConfigReducer';

export default combineReducers({   
    queues:DeliveryQueueReducer,
    user:UserReducer,
    queueCountData:SignalRQueueDataReducer,
    notificationHistory:NotificationHistoryQueueReducer,
    config: ConfigReducer
    // More reducers if there are
    // can go here
});