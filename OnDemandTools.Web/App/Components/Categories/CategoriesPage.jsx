import React from 'react';
import { connect } from 'react-redux';
import * as categoryActions from 'Actions/Category/CategoryActions';
import CategoryTable from 'Components/Categories/CategoryTable';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

@connect((store) => {
    return {
        categories: store.categories
    };
})

class CategoriesPage extends React.Component {

    constructor(props) {
        super(props);
       
        this.state = {
            stateQueue: [],

            filterValue: {
                code: "",
                description: "",
                content: ""
            },

            columns: [{ "label": "Name", "dataField": "name", "sort": true },
            { "label": "Destination", "dataField": "destination", "sort": false },
            { "label": "Description", "dataField": "description", "sort": false },
            { "label": "Actions", "dataField": "name", "sort": false }
            ],
            keyField: "name"
        }
    }

    //callback function to get the filter value from filter component
    handleFilterUpdate(filtersValue, type) {
        

    }

    //called on the page load
    componentDidMount() {
        this.props.dispatch(categoryActions.fetchCategories());

        document.title = "ODT - Categories";
    }

    render() {
        return (
            <div>               
                <PageHeader pageName="Categories" />
                <CategoryTable RowData={this.props.categories} ColumnData={this.state.columns} KeyField={this.state.keyField} />
            </div>
        )
    }
}


export default CategoriesPage;