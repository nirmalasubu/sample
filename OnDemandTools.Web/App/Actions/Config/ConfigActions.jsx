import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';

export const fetchConfigSuccess = (config) => {
    return {
        type: actionTypes.FETCH_CONFIG_SUCCESS,
        config
    }
};

export const fetchConfig = () => {
    return (dispatch) => {
        return Axios.get('/api/config')        
          .then(response => { 
              dispatch(fetchConfigSuccess(response.data))
          })
          .catch(error => {
              throw(error);
          });
    };
};