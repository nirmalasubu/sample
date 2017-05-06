export const DeliveryQueueReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_SIGNALRQUEUES_SUCCESS':
            return action.queueCountData;
        case 'FETCH_QUEUES_SUCCESS':
            return action.queues;
        default:
            return state;
    }
};
