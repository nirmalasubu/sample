import { combineReducers } from 'redux';

import { DeliveryQueueReducer, SignalRQueueDataReducer, NotificationHistoryQueueReducer, FilterQueueDataReducer } from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import { DestinationReducer, FilterDestinationDataReducer } from 'Reducers/Destination/DestinationReducer';
import { CategoryReducer } from 'Reducers/Category/CategoryReducer';
import { UserReducer } from 'Reducers/User/UserReducer';
import { ConfigReducer } from 'Reducers/Config/ConfigReducer';
import { StatusReducer } from 'Reducers/Status/StatusReducer';
import { ProductReducer } from 'Reducers/Product/ProductReducer';
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
    categories: CategoryReducer
    // More reducers if there are
    // can go here
});