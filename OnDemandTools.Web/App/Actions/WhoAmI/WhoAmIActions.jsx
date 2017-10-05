import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import * as configActions from 'Actions/Config/ConfigActions'


export const fetchWhoAmISuccess = (whoami) => {
    return {
        type: actionTypes.FETCH_WHOAMI_SUCCESS,
        whoami
    }
};



export const fetchWhoAmI = () => {
    return (dispatch) => {
        return Axios.get('/whoami')        
          .then(response => { 
              dispatch(fetchWhoAmISuccess(response.data))
          })
          .catch(error => {
              dispatch(configActions.handleApplicationAPIError(error));
              throw(error);
          });
    };
};