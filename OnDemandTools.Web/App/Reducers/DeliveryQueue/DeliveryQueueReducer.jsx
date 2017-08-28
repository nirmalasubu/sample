export const DeliveryQueueReducer = (state = [], action) => {
    switch (action.type) {
        case 'FETCH_QUEUES_SUCCESS':
            return action.queues;
        case 'SAVE_QUEUE_SUCCESS':
            var queueIndex = state.findIndex((obj => obj.id == action.queue.id));
            if (queueIndex < 0) {
                state.push(action.queue);
            }
            else {
                state[queueIndex] = action.queue;
            }
            return state;
        case 'DELETE_QUEUE_SUCCESS':
            var dIndex = state.findIndex((obj => obj.id == action.objectId));
            if (dIndex >= 0)
                state.splice(dIndex,1);
            return state;
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