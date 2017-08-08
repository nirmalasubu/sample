
import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import RemovePathTranslationModal from 'Components/PathTranslations/RemovePathTranslationModal';
import AddEditPathTranslationModal from 'Components/PathTranslations/AddEditPathTranslationModal';
import PathTranslationModel from './PathTranslationModel'

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
            pathTranslationDetails: PathTranslationModel,
            showModal: false,
            showAddEditModal: false,
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
        this.setState({ showAddEditModal: true, pathTranslationDetails: PathTranslationModel });
    }


    /// <summary>
    /// Create the presentation for the source column
    /// </summary>
    sourceFormatter(cell, row) {
        return (
            <div>
                <div><span class='standout'>Base URL:</span> {cell.baseUrl}</div>
                {
                    cell.brand ? <div><span class='standout'>Brand:</span> {cell.brand}</div> : ''
                }
            </div>
        );
    }

    /// <summary>
    /// Create the presentation for the target column
    /// </summary>
    targetFormatter(cell, row) {
        return (
            <div>
                <div><span class='standout'>Base URL:</span> {cell.baseUrl}</div>
                <div><span class='standout'>Protection Type:</span> {cell.protectionType}</div>
                <div><span class='standout'>URL Type:</span> {cell.urlType}</div>
            </div>
        );
    }


    ///<summary>
    // Close delete confirmation modal
    ///</summary>
    closeDeleteModel(pathTranslationToDelete) {
        this.setState({ showDeleteModal: false });
    }

    ///<summary>
    // Close add/edit confirmation modal
    ///</summary>
    closeAddEditModel(pathTranslationToDelete) {
        this.setState({ showAddEditModal: false });
    }




    /// <summary>
    /// Create the presentation for the actions column
    /// </summary>
    actionsFormatter = (cell, row) => {

        // handle delete button click
        const onOpenDeleteModal = (record) => {
            this.setState({ showDeleteModal: true, pathTranslationDetails: record });           
        }

        // handle edit button click
        const onOpenEditModal = (record) => {
            // Before opening modal window, do some sanity check
            const pathTranModel = record;
            pathTranModel.id = pathTranModel.id ? pathTranModel.id : "";
            pathTranModel.source.baseUrl = pathTranModel.source.baseUrl ? pathTranModel.source.baseUrl : "";
            pathTranModel.source.brand = pathTranModel.source.brand ? pathTranModel.source.brand : "";
            pathTranModel.target.baseUrl = pathTranModel.target.baseUrl ? pathTranModel.target.baseUrl : "";
            pathTranModel.target.protectionType = pathTranModel.target.protectionType ? pathTranModel.target.protectionType : "";
            pathTranModel.target.urlType = pathTranModel.target.urlType ? pathTranModel.target.urlType : "";

            this.setState({ showAddEditModal: true, pathTranslationDetails: pathTranModel });         
        }


        return (
            <div>
                <button class="btn-link" title="Edit Path Translation" onClick={() => { onOpenEditModal(row) }} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Delete Path Translation" onClick={() => { onOpenDeleteModal(row) }} >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
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
                <BootstrapTable
                    data={this.props.pathTranslationRecords}
                    striped={true}
                    hover={true}
                    pagination={true}>
                    <TableHeaderColumn dataField="source" width="45%" isKey={true} dataAlign="left" dataFormat={this.sourceFormatter}>Source Path</TableHeaderColumn>
                    <TableHeaderColumn dataField="target" width="45%" dataAlign="left" dataFormat={this.targetFormatter}>Target Path</TableHeaderColumn>
                    <TableHeaderColumn dataFormat={this.actionsFormatter}>Actions</TableHeaderColumn>
                </BootstrapTable >
                <RemovePathTranslationModal data={this.state} handleClose={this.closeDeleteModel.bind(this)} />
                <AddEditPathTranslationModal data={this.state} handleClose={this.closeAddEditModel.bind(this)} />
            </div >
        )
    }

}

export default PathTranslationTable;