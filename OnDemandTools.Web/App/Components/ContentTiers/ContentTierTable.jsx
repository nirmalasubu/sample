import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Button } from 'react-bootstrap';
import ContentTierExpandTable from 'Components/ContentTiers/ContentTierExpandTable';
import AddEditContentTier from 'Components/ContentTiers/AddEditContentTier';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import RemoveContentTierModal from 'Components/ContentTiers/RemoveContentTierModal';
import { getNewContentTier } from 'Actions/ContentTier/ContentTierActions';


class ContentTierTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            newContentTierModel: {},
            showModal: false,
            showAddEditModel: false,
            showDeleteModal: false,
            contentTierDetails: "",
            options: {
                defaultSortName: 'name',
                defaultSortOrder: 'asc',
                expandRowBgColor: '#EAECEE',
                expandBy: 'column',
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

    componentDidMount() {
        let promise = getNewContentTier();
        promise.then(message => {
            this.setState({
                newContentTierModel: message
            });
        }).catch(error => {
            this.setState({
                newContentTierModel: {}
            });
        });
    }

    openCreateNewProductModel() {
        this.setState({ showAddEditModel: true, contentTierDetails: jQuery.extend(true, {}, this.state.newContentTierModel) });
    }
    ///<summary>
    // when delete contentTier button event handled
    ///</summary>
    openDeleteModel(val) {
        this.setState({ showDeleteModal: true, contentTierDetails: val });
    }

    ///<summary>
    // React modal pop up control   delete contentTier is closed
    ///</summary>
    closeDeleteModel() {
        this.setState({ showDeleteModal: false });
    }

    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, contentTierDetails: val });
    }

    closeAddEditModel() {
        this.setState({ showAddEditModel: false, contentTierDetails: this.state.newContentTierModel });
    }

    descriptionFormat(val) {
    }

    isExpandableRow(row) {
        if (row.products.length > 0) return true;
        else return false;
    }

    expandComponent(row) {
        return (
            <ContentTierExpandTable data={row} />
        );
    }

    contentTierNameFormat(val, rowData) {
        return (
            <p data-toggle="tooltip"> {val}</p>
        );
    }

    productFormat(val, rowData) {
        var productNames = [];
        var rows = [];

        for (var idx = 0; idx < rowData.products.length; idx++) {
            productNames.push(rowData.products[idx].name);
        }
        //product names are sorted before rendering 
        if (productNames.length > 0) {
            productNames.sort();
            for (var idx = 0; idx < productNames.length; idx++) {
                rows.push(<Button className="addMarginRight" key={idx.toString()}> {productNames[idx]} </Button>);
            }
        }

        return (
            <div>
                {rows}
            </div>
        );

    }

    actionFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" title="Edit ContentTier" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Delete ContentTier" onClick={(event) => this.openDeleteModel(rowData, event)} >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
    }

    render() {

        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Name") {
                return <TableHeaderColumn width="250px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.contentTierNameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Products") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.productFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="100px" expandable={false} dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} >{item.label}</TableHeaderColumn>
            }

        }.bind(this));

        var contentTierIdandNames = [];

        for (var i = 0; i < this.props.RowData.length; i++) {
            contentTierIdandNames.push({ "id": this.props.RowData[i].id, "name": this.props.RowData[i].name });
        }

        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="New ContentTier" onClick={(event) => this.openCreateNewProductModel(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New ContentTier</span>
                    </button>
                </div>
                <BootstrapTable
                    expandableRow={this.isExpandableRow}
                    expandComponent={this.expandComponent}
                    data={this.props.RowData}
                    striped={true}
                    hover={true}
                    keyField={this.props.KeyField}
                    pagination={true}
                    options={this.state.options}>
                    {row}
                </BootstrapTable>

                <AddEditContentTier data={this.state} contentTierIdandNames={contentTierIdandNames} handleClose={this.closeAddEditModel.bind(this)} />
                <RemoveContentTierModal data={this.state} handleClose={this.closeDeleteModel.bind(this)} />
            </div>)
    }

}

export default ContentTierTable;

