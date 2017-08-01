import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { connect } from 'react-redux';
import $ from 'jquery';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import RemoveCurrentAiringIdModal from 'Components/IDDistribution/RemoveCurrentAiringIdModal';
import TextOverlay from 'Components/Common/TextOverlay';
import { getNewAiringId } from 'Actions/AiringIdDistribution/AiringIdDistributionActions';
import * as currentAiringIdActions  from 'Actions/AiringIdDistribution/AiringIdDistributionActions';

@connect((store) => {
    return {
        emptyResult:null
    };
})

class DistributionTable extends React.Component {
    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = {
            newAiringIdModel: {},
            showModal: false,
            showAddEditModel: false,
            showDeleteModal: false,
            airingIdDetails: "",
            options: {
                defaultSortName: 'prefix',
                defaultSortOrder: 'asc',  
                sizePerPageList: [{
                    text: '10 ', value: 10
                }, {
                    text: '25 ', value: 25
                }, {
                    text: '50 ', value: 50
                },
                {
                    text: 'All ', value: 10000000
                }]
            }
        }
    }

    ///<summary>
    ///Called on component load
    ///</summary>
    componentDidMount() {
        let promise = getNewAiringId();
        promise.then(message => {
            this.setState({
                newAiringIdModel: message
            });
        }).catch(error => {
            this.setState({
                newAiringIdModel: {}
            });
        });
    }

    ///<summary>
    ///This is to open a modal popup to add a new airing id.
    ///</summary>
    openCreateNewAiringIdModel() {
        this.setState({ showAddEditModel: true, airingIdDetails: jQuery.extend(true, {}, this.state.newAiringIdModel) });
    }

    ///<summary>
    // when delete airing id button event handled
    ///</summary>
    openDeleteModel(val) {
        this.setState({ showDeleteModal: true, airingIdDetails: val });
    }

    ///<summary>
    // React modal pop up control delete airing id is closed
    ///</summary>
    closeDeleteModel() {
        this.setState({ showDeleteModal: false });
    }

    ///<summary>
    ///This is to generate a new airing id using prefix.
    ///</summary>
    generateAiringId(val) {
        this.props.dispatch(currentAiringIdActions.generateAiringId(val.prefix));
    }

    ///<summary>
    ///This is to open a modal popup to edit a airing id.
    ///</summary>
    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, airingIdDetails: val });
    }

    ///<summary>
    ///This is to close a modal popup to edit a airing id.
    ///</summary>
    closeAddEditModel() {
        this.setState({ showAddEditModel: false, airingIdDetails: this.state.newAiringIdModel });
    }

    ///<summary>
    ///This method returns a prefix of airing id to display in the grid.
    ///</summary>
    prefixFormat(val, rowData) {
        return (
            <p data-toggle="tooltip"> {val}</p>
        );
    }

    ///<summary>
    ///This method returns a product description to display in the grid.
    ///</summary>
    airingIdFormat(val) {        
        return <p data-toggle="tooltip"> {val}</p>;
    }

    ///<summary>
    ///This method returns the current billing number that mapped to a airing id to display in the grid.
    ///</summary>
    billingNumberFormat(val) {
        return <p data-toggle="tooltip"> {val}</p>;
    }

    ///<summary>
    ///This method construct the edit and delete action buttons
    ///</summary>
    actionFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" title="generate next airing id in sequence" onClick={(event) => this.generateAiringId(rowData, event)} >
                    <i class="fa fa-level-up" aria-hidden="true"></i>
                </button>
                
                <button class="btn-link" title="Edit Current Airing ID" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Delete Current Airing ID" onClick={(event) => this.openDeleteModel(rowData, event)} >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
    }

    render() {

        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Code") {
                return <TableHeaderColumn width="10%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.prefixFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Airing ID") {
                return <TableHeaderColumn width="28%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.airingIdFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Billing Number") {
                return <TableHeaderColumn width="17%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.billingNumberFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="10%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} >{item.label}</TableHeaderColumn>
            }

        }.bind(this));
            
        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="New Product" onClick={(event) => this.openCreateNewAiringIdModel(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New ID</span>
                    </button>
                </div>
                <BootstrapTable
                    data={this.props.RowData}
                    striped={true}
                    hover={true}
                    keyField={this.props.KeyField}
                    pagination={true}
                    options={this.state.options}>
                        {row}
                </BootstrapTable>
                <RemoveCurrentAiringIdModal data={this.state} handleClose={this.closeDeleteModel.bind(this)} />
            </div>)
                        }

}

export default DistributionTable;

