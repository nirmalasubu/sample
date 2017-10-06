import * as actionTypes from 'Actions/ActionTypes';
import Axios from 'axios';
import * as configActions from 'Actions/Config/ConfigActions'


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
              dispatch(configActions.handleApplicationAPIError(error));
              throw(error);
          });
    };
};


/// <summary>
/// Asynchronously retrieve user contact from API. If successful
/// dispatch the appropriate action for further processing
/// </summary>
export const fetchContactForApiRecords = (id) => {
    return Axios.get('/api/user/getcontactforapidetailsbyuserid/' + id)
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            configActions.handleApplicationAPIError(error);
        });


};