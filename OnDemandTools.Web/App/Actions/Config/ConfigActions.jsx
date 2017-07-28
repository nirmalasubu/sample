import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';


/****************** Actions ************************/
export const fetchConfigSuccess = (config) => {
    return {
        type: actionTypes.FETCH_CONFIG_SUCCESS,
        config
    }
};

/// <summary>
/// Asynchronously retrieve path translations from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const handleApplicationAPIError = (error) => {    
      return {
        type: actionTypes.APPLICATION_API_ERROR,
        error
    }
};



/******************* Helper methods *******************/
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



