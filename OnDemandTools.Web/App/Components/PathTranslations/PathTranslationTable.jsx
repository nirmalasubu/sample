
import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';


/// <summary>
//  Main table that displays all of the Path translations
/// </summary>
class PathTranslationTable extends React.Component {


    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = {
            newPathTranslationModel: {},
            showModal: false,
            showAddEditModel: false,
            showDeleteModal: false
        }
    }


    /// <summary>
    /// Before the component is rendered, retrieve the list of 
    /// pathtranslations from API. This is peformed async via the appropriate
    /// action/reducer.
    /// </summary>  
    componentDidMount() {

    }


    /// <summary>
    /// Open modal window to add path translations  
    /// </summary>
    openCreateNewPathTranslationModel() {
       console.log('ohhh');
    }


    render() {
        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="New Path Translation" onClick={(event) => this.openCreateNewPathTranslationModel(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New Path Translation</span>
                    </button>
                </div>
            </div>
        )
    }

}

export default PathTranslationTable;