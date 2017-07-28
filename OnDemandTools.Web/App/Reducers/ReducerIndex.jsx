﻿import { combineReducers } from 'redux';

import { DeliveryQueueReducer, SignalRQueueDataReducer, NotificationHistoryQueueReducer, FilterQueueDataReducer } from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import { DestinationReducer, FilterDestinationDataReducer } from 'Reducers/Destination/DestinationReducer';
import { CategoryReducer ,FilterCategoryDataReducer } from 'Reducers/Category/CategoryReducer';
import { UserReducer } from 'Reducers/User/UserReducer';
import { ConfigReducer,TimerReducer } from 'Reducers/Config/ConfigReducer';
import { StatusReducer,FilterStatusDataReducer } from 'Reducers/Status/StatusReducer';
import { ProductReducer, FilterProductDataReducer } from 'Reducers/Product/ProductReducer';
import { TitleSearchReducer } from 'Reducers/TitleSearch/TitleSearchReducer';

export default combineReducers({
    queues: DeliveryQueueReducer,
    filterValue: FilterQueueDataReducer,
    destinations: DestinationReducer,
    filterDestination: FilterDestinationDataReducer,
    user: UserReducer,
    queueCountData: SignalRQueueDataReducer,
    notificationHistory: NotificationHistoryQueueReducer,
    config: ConfigReducer,
    statuses: StatusReducer,
    products: ProductReducer,
    titleSearch: TitleSearchReducer,
    categories: CategoryReducer,
    filterCategory: FilterCategoryDataReducer,
    filterProduct: FilterProductDataReducer,
    filterStatus:FilterStatusDataReducer,
    serverPollTime:TimerReducer

    // More reducers if there are
    // can go here
});