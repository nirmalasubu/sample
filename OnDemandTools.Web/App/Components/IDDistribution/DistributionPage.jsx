import React from 'react';
import { connect } from 'react-redux';
import * as currentAiringIdActions from 'Actions/AiringIdDistribution/AiringIdDistributionActions';
import DistributionTable from 'Components/IDDistribution/DistributionTable';
import DistributionFilter from 'Components/IDDistribution/DistributionFilter';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    ///<summary>
    /// Retrieve filtered list of current airing Id's based on filterValue like code, airing id
    /// which are defined in Redux store
    ///</summary>
    var filteredDistributionValues = getFilterVal(store.currentAiringIds, store.filterDistribution);
    return {
        currentAiringIds: store.currentAiringIds,
        filteredDistributions: (filteredDistributionValues!=undefined?filteredDistributionValues:store.currentAiringIds)
    };
})
class DistributionPage extends React.Component {
    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
       
        this.state = {
            stateQueue: [],

            filterValue: {
                code: "",
                airingId: ""
            },

            columns: [{ "label": "Code", "dataField": "prefix", "sort": true },
            { "label": "Airing ID", "dataField": "airingId", "sort": false },
            { "label": "Billing Number", "dataField": "billingNumberCurrent", "sort": false },
            { "label": "Actions", "dataField": "prefix", "sort": false }
            ],
            keyField: "prefix"
        }
    }

    ///<summary>
    // Function to handle filtering of current airing id data. This handler
    // is used within the airing id filter sub component. Filtering is currently
    // supported for Code, amd airing id.
    /// </summary>
    handleFilterUpdate(filtersValue, type) {
        var stateFilterValue = this.state.filterValue;
        if (type == "CD")
            stateFilterValue.code = filtersValue;

        if (type == "AI")
            stateFilterValue.airingId = filtersValue;

        if (type == "CL") {
            stateFilterValue.code = "";
            stateFilterValue.airingId = "";
        }
        this.setState({
            filterValue: stateFilterValue
        });
        this.props.dispatch(currentAiringIdActions.filterAiringIdSuccess(this.state.filterValue));  
    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(currentAiringIdActions.filterAiringIdSuccess(this.state.filterValue));
        this.props.dispatch(currentAiringIdActions.fetchCurrentAiringId());

        document.title = "ODT - ID Distribution";
    }

    render() {
        return (
            <div>               
                <PageHeader pageName="ID Distribution" />
                <DistributionFilter updateFilter={this.handleFilterUpdate.bind(this)} />
                <DistributionTable RowData={this.props.filteredDistributions} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}

///<summary>
// The goal of this function is to filter 'current airing ids' (which is stored in Redux store)
// based on user provided filter criteria and return the refined 'current airings ids' list.
// If no filter criteria is provided then return the full 'current airing ids' list
///</summary>
const getFilterVal = (currentAiringIds, filterVal) => {
    if(filterVal.code!=undefined)
    {
        var code = filterVal.code.toLowerCase();
        var airingId = filterVal.airingId.toLowerCase();
        return (currentAiringIds.filter(obj=> (code != "" ? obj.prefix.toLowerCase().indexOf(code) != -1 : true)
              &&(airingId != "" ? obj.airingId.toLowerCase().indexOf(airingId) != -1 : true)
            ));
    }
    else
        return currentAiringIds;
};

export default DistributionPage;