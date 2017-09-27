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
        applicationError: store.applicationError,
        user: store.user
    };
})

/// <summary>
//  Main grid component for Path translations
/// </summary>
class PathTranslationPage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            permissions: { canAdd: false, canRead: false, canEdit: false, canAddOrEdit: false, disableControl: true },
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

        if (this.props.user && this.props.user.portal) {
            this.setState({ permissions: this.props.user.portal.modulePermissions.PathTranslations })
        }
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        if (nextProps.user && nextProps.user.portal) {
            this.setState({ permissions: nextProps.user.portal.modulePermissions.PathTranslations });
        }
    }


    render() {

        if (this.props.user.portal == undefined) {
            return <div>Loading...</div>;
        }
        else if (!this.state.permissions.canRead) {
            return <h3>Unauthorized to view this page</h3>;
        }

        return (
            <div>
                <PageHeader pageName="Path Translations" />
                <PathTranslationTable permissions={this.state.permissions} pathTranslationRecords={this.props.pathTranslationRecords} />
            </div>
        )
    }
}



export default PathTranslationPage;