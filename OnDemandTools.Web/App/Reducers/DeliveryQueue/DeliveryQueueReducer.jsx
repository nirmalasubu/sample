export const DeliveryQueueReducer = (state = [], action) => {
    switch (action.type) {
        case 'CREATE_QUEUE_SUCCESS':
            return [
              ...state,
              Object.assign({}, action.book)
            ];
        case 'FETCH_QUEUES_SUCCESS':
            return action.queues;
        default:
            return state;
    }
};