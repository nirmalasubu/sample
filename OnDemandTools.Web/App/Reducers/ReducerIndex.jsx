import { combineReducers } from 'redux';

import { DeliveryQueueReducer, SignalRQueueDataReducer, NotificationHistoryQueueReducer } from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import { DestinationReducer } from 'Reducers/Destination/DestinationReducer';
import { CategoryReducer } from 'Reducers/Category/CategoryReducer';
import { UserReducer } from 'Reducers/User/UserReducer';
import { ConfigReducer, ApplicationErrorReducer } from 'Reducers/Config/ConfigReducer';
import { StatusReducer } from 'Reducers/Status/StatusReducer';
import { ProductReducer } from 'Reducers/Product/ProductReducer';
import { TitleSearchReducer } from 'Reducers/TitleSearch/TitleSearchReducer';
import { ContentTierReducer } from 'Reducers/ContentTier/ContentTierReducer';
import { CurrentAiringIdReducer } from 'Reducers/CurrentAiringId/CurrentAiringIdReducer';
import { PathTranslationReducer } from 'Reducers/PathTranslation/PathTranslationReducer';
import { PermissionReducer } from 'Reducers/Permission/PermissionReducer';

export default combineReducers({
    queues: DeliveryQueueReducer,
    destinations: DestinationReducer,
    user: UserReducer,
    queueCountData: SignalRQueueDataReducer,
    notificationHistory: NotificationHistoryQueueReducer,
    config: ConfigReducer,
    statuses: StatusReducer,
    products: ProductReducer,
    titleSearch: TitleSearchReducer,
    categories: CategoryReducer,
    contentTiers: ContentTierReducer,
    currentAiringIds: CurrentAiringIdReducer, 
    pathTranslations: PathTranslationReducer,  
    pathTranslationModel: PathTranslationReducer,
    pathTranslationRecords: PathTranslationReducer,
    permissions: PermissionReducer,
    applicationError: ApplicationErrorReducer,

    // More reducers if there are
    // can go here
});