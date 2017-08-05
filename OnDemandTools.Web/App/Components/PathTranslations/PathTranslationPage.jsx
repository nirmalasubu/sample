import React from 'react';
import { connect } from 'react-redux';
import PageHeader from 'Components/Common/PageHeader';
import * as PathTranslationHelper from 'Actions/PathTranslation/PathTranslationActions';
import 'react-notifications/lib/notifications.css';
import PathTranslationTable from './PathTranslationTable';

/// <summary>
/// Connect this component to global Redux data store
/// </summary>
@connect((store) => {
    return {
        pathTranslationRecords: store.pathTranslationRecords,
        applicationError: store.applicationError
    };
})



/// <summary>
//  Main grid component for Path translations
/// </summary>
class PathTranslationPage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {           
        };

    }

     /// <summary>
    /// Before the component is rendered, retrieve the list of 
    /// pathtranslations from API. This is peformed async via the appropriate
    /// action/reducer.
    /// </summary>     
    componentWillMount() {
       
    }


    /// <summary>
    /// Before the component is rendered, retrieve the list of 
    /// pathtranslations from API. This is peformed async via the appropriate
    /// action/reducer.
    /// </summary>     
    componentDidMount() {
        this.props.dispatch(PathTranslationHelper.fetchPathTranslationRecords());
        document.title = "ODT - Path Translations";
    }


    render() {    

        return (
            <div>
                <PageHeader pageName="Path Translations" />
                <PathTranslationTable pathTranslationRecords={this.props.pathTranslationRecords}/>
            </div>
        )
    }
}



export default PathTranslationPage;