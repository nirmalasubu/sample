﻿import { combineReducers } from 'redux';

import {DeliveryQueueReducer,SignalRQueueDataReducer, NotificationHistoryQueueReducer,FilterQueueDataReducer} from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import {DestinationReducer, FilterDestinationDataReducer} from 'Reducers/Destination/DestinationReducer';
import {UserReducer} from 'Reducers/User/UserReducer';
import {ConfigReducer} from 'Reducers/Config/ConfigReducer';
import {StatusReducer} from 'Reducers/Status/StatusReducer';

export default combineReducers({   
    queues:DeliveryQueueReducer,
    filterValue:FilterQueueDataReducer,
    destinations:DestinationReducer,
    filterDestination:FilterDestinationDataReducer,
    user:UserReducer,
    queueCountData:SignalRQueueDataReducer,
    notificationHistory:NotificationHistoryQueueReducer,
    config: ConfigReducer,
    statuses: StatusReducer
    // More reducers if there are
    // can go here
});