import { combineReducers } from 'redux';

import { DeliveryQueueReducer, SignalRQueueDataReducer, NotificationHistoryQueueReducer, FilterQueueDataReducer } from 'Reducers/DeliveryQueue/DeliveryQueueReducer';
import { DestinationReducer, FilterDestinationDataReducer } from 'Reducers/Destination/DestinationReducer';
import { CategoryReducer, FilterCategoryDataReducer } from 'Reducers/Category/CategoryReducer';
import { UserReducer } from 'Reducers/User/UserReducer';
import { ConfigReducer, ApplicationErrorReducer } from 'Reducers/Config/ConfigReducer';
import { StatusReducer,FilterStatusDataReducer } from 'Reducers/Status/StatusReducer';
import { ProductReducer, FilterProductDataReducer } from 'Reducers/Product/ProductReducer';
import { TitleSearchReducer } from 'Reducers/TitleSearch/TitleSearchReducer';
import { ContentTierReducer, FilterContentTierDataReducer } from 'Reducers/ContentTier/ContentTierReducer';
import { CurrentAiringIdReducer,FilterAiringIdDataReducer } from 'Reducers/CurrentAiringId/CurrentAiringIdReducer';
import { PathTranslationReducer } from 'Reducers/PathTranslation/PathTranslationReducer'

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
    contentTiers: ContentTierReducer,
    filterContentTier: FilterContentTierDataReducer,
    filterCategory: FilterCategoryDataReducer,
    filterProduct: FilterProductDataReducer,
    filterStatus: FilterStatusDataReducer,
    currentAiringIds: CurrentAiringIdReducer,
    filterDistribution: FilterAiringIdDataReducer,   
    pathTranslations: PathTranslationReducer,  
    pathTranslationModel: PathTranslationReducer,
    applicationError: ApplicationErrorReducer
    // More reducers if there are
    // can go here
});