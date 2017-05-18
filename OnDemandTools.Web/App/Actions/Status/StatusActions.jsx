import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';



export const fetchStatusSuccess = (statuses) => {    
    return {
        type: actionTypes.FETCH_STATUS_SUCCESS,
        statuses
    }
};

export const fetchStatus = () => {
    return (dispatch) => {
        return Axios.get('/api/status')        
          .then(response => { 
              dispatch(fetchStatusSuccess(response.data))
          })
          .catch(error => {
              throw(error);
          });
    };
};