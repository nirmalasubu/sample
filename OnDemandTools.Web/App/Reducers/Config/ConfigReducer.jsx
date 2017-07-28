export const ConfigReducer = (state = {}, action) => {
    switch (action.type) {
        case 'FETCH_CONFIG_SUCCESS':
            return action.config;
        default:
            return state;
    }
};


export const TimerReducer = (state = "", action) => {
    switch (action.type) {
        case 'TIMER_SUCCESS':
            console.log("store updated");
            var currentdate = new Date(); 
            //var datetime =  currentdate.getDate() + "/"
            //                + (currentdate.getMonth()+1)  + "/" 
            //                + currentdate.getFullYear() + " "  
            //                + currentdate.getHours() + ":"  
            //                + currentdate.getMinutes() 
            const newState = currentdate 
            return newState
        default:
            return state;
    }
};