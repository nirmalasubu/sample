import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import rootReducer from 'Reducers/ReducerIndex';

export default function configureStore(initialState) {
    const customMiddleWare = store => next => action => {
        next(action);
        action.type= "TIMER_SUCCESS";  
        next(action); // required whenever server action actions trigger Timer success also.
    }
    return createStore(rootReducer, initialState,
  applyMiddleware(thunk,customMiddleWare));
}