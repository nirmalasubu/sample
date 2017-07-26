import React from 'react';
import { connect } from 'react-redux';
import PageHeader from 'Components/Common/PageHeader';
import 'react-notifications/lib/notifications.css';

/// <summary>
/// Before the component is rendered, retrieve
/// the list of path translations that is stored in Redux
/// store and set it as a property for this component
/// </summary>
@connection((store) => {
    return {
        pathTranslations: store.pathTranslations
    };
})


/// <summary>
//  Main grid component for Path translations
/// </summary>
class PathTranslationsPage extends React.Component {

    constructor(props) {
        super(props);      
       
      
    }


    /// <summary>
    /// Before the component is rendered, retrieve the list of 
    /// pathtranslations from API. This is peformed async via the appropriate
    /// action/reducer.
    /// </summary>     
    componentDidMount() {        
        this.props.dispatch(destinationActions.fetchDestinations());
        document.title = "ODT - Path Translations";
    }
    

    render() {
        return (
            <div>               
                Hello from path translations
            </div>
        )
    }
}



export default PathTranslationsPage;