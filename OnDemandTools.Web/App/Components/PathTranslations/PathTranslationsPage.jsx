import React from 'react';
import { connect } from 'react-redux';
import PageHeader from 'Components/Common/PageHeader';
import * as PathTranslationHelper from 'Actions/PathTranslation/PathTranslationActions';
import 'react-notifications/lib/notifications.css';
import PathTranslationModel from './PathTranslationModel'

/// <summary>
/// Connect this component to global Redux data store
/// </summary>
@connect((store) => {
    return {
        pathTranslationData: store.pathTranslationModel,
        applicationError:store.applicationError
    };
})


 
/// <summary>
//  Main grid component for Path translations
/// </summary>
class PathTranslationsPage extends React.Component {

    constructor(props) {
        super(props);      
       
        this.state = {
             pathTranslationData: new PathTranslationModel()
        };
      
    }


    /// <summary>
    /// Before the component is rendered, retrieve the list of 
    /// pathtranslations from API. This is peformed async via the appropriate
    /// action/reducer.
    /// </summary>     
    componentDidMount() {        
        this.props.dispatch(PathTranslationHelper.fetchPathTranslation());
        document.title = "ODT - Path Translations";
    }
    

    render() {
        let myComponent;
        if(this.props.pathTranslationData.pathTranslations){
            myComponent = this.props.pathTranslationData.pathTranslations.length;
        }
        else{
            myComponent = 0;
        }

        let error;
        if(this.props.applicationError){
            error = this.props.applicationError.stack;
        }
        else{
            error = '';
        }
       
       

        return (
            <div>               
                {myComponent}
                {error}
            </div>
            
        )
    }
}



export default PathTranslationsPage;