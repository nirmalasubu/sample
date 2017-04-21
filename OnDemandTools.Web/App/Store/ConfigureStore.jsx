import {createStore} from 'redux';
import rootReducer from 'Reducers/ReducerIndex';

export default function configureStore(initialState) {
    return createStore(rootReducer, initialState);
}