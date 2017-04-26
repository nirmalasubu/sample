import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';



export const fetchUserSuccess = (user) => {
    return {
        type: actionTypes.FETCH_USERDETAILS_SUCCESS,
        user
    }
};



export const fetchUser = () => {
    return (dispatch) => {
        return Axios.get('/api/user')        
          .then(response => { 
              dispatch(fetchUserSuccess(response.data))
          })
          .catch(error => {
              throw(error);
          });
    };
};