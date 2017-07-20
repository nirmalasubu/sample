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

export const filterStatusSuccess= (filterStatus) => {
    return {
        type: actionTypes.FILTER_STATUS_SUCCESS,
        filterStatus
    }
};


export const deleteStatusSuccess = (objectId) => {
    return {
        type: actionTypes.DELETE_STATUS_SUCCESS,
        objectId
    }
};

export const deleteStatus = (id) => {
    return (dispatch) => {        
        return Axios.delete('/api/status/'+ id)
            .then(response => {
                dispatch(deleteStatusSuccess(id))
            })
            .catch(error => {
                throw (error);
            });
    };
}

export const saveStatusSuccess = (status) => {
    return {
        type: actionTypes.SAVE_STATUS_SUCCESS,
        status
    }
}

export const saveStatus = (model) => {
    return (dispatch) => {        
        return Axios.post('/api/status', model)
            .then(response => {
                dispatch(saveStatusSuccess(response.data))
            })
            .catch(error => {
                throw (error);
            });
    };
};

export const getNewStatus = () => {
    return Axios.get('/api/status/newstatus')
        .then(response => {
            return (response.data);
        })
        .catch(error => {
            throw (error);
        });
};