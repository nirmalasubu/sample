export const DeliveryQueueReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_QUEUES_SUCCESS':
            return action.queues;
        default:
            return state;
    }
};

export const NotificationHistoryQueueReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_NOTIFICATIONHISTORY_SUCCESS':
            return action.notificationHistory;
        default:
            return state;
    }
};

export const SignalRQueueDataReducer = (state = {}, action) => {
    switch (action.type) {
        case 'FETCH_SIGNALRQUEUES_SUCCESS':
            return action.queueCountData;
        default:
            return state;
    }
};