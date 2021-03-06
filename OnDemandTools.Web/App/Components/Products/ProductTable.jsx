﻿import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import AddEditProduct from 'Components/Products/AddEditProduct';
import RemoveProductModal from 'Components/Products/RemoveProductModal';
import DestinationOverlay from 'Components/Common/DestinationOverlay';
import TextOverlay from 'Components/Common/TextOverlay';
import { getNewProduct } from 'Actions/Product/ProductActions';

@connect((store) => {
    return {
        products: store.products,
        user: store.user
    };
})


class ProductTable extends React.Component {
    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = {
            permissions: { canAdd: false, canRead: false, canEdit: false, canAddOrEdit: false, disableControl: true },
            newProductModel: {},
            showModal: false,
            showAddEditModel: false,
            showDeleteModal: false,
            productDetails: "",
            options: {
                defaultSortName: 'name',
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
                }],
                onSortChange :this.onSortChange.bind(this)
            }
        }
    }

    ///<summary>
    ///Called on component load
    ///</summary>
    componentDidMount() {
        let promise = getNewProduct();
        promise.then(message => {
            this.setState({
                newProductModel: message
            });
        }).catch(error => {
            this.setState({
                newProductModel: {}
            });
        });

        if (this.props.user && this.props.user.portal) {
            this.setState({ permissions: this.props.user.portal.modulePermissions.Products });
        }
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        if (nextProps.user && nextProps.user.portal) {
            this.setState({ permissions: nextProps.user.portal.modulePermissions.Products });
        }
    }

    ///<summary>
    /// on clicking sort arrow in any page of the table should take to the First page in the pagination.
    ///</summary>
    onSortChange() {
        const sizePerPage = this.refs.productTable.state.sizePerPage;
        this.refs.productTable.handlePaginationData(1, sizePerPage);
    }

    ///<summary>
    ///This is to open a modal popup to add a new product.
    ///</summary>
    openCreateNewDestinationModel() {
        this.setState({ showAddEditModel: true, productDetails: jQuery.extend(true, {}, this.state.newProductModel) });
    }

    ///<summary>
    // when delete product button event handled
    ///</summary>
    openDeleteModel(val) {
        this.setState({ showDeleteModal: true, productDetails: val });
    }

    ///<summary>
    // React modal pop up control delete product is closed
    ///</summary>
    closeDeleteModel() {
        this.setState({ showDeleteModal: false });
    }

    ///<summary>
    ///This is to open a modal popup to edit a product.
    ///</summary>
    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, productDetails: val });
    }

    ///<summary>
    ///This is to close a modal popup to edit a product.
    ///</summary>
    closeAddEditModel() {
        this.setState({ showAddEditModel: false, productDetails: this.state.newProductModel });
    }

    ///<summary>
    ///This method returns a product name to display in the grid.
    ///</summary>
    productNameFormat(val, rowData) {
        return (
            <p data-toggle="tooltip"> {val}</p>
        );
    }

    ///<summary>
    ///This method returns a product description to display in the grid.
    ///</summary>
    descriptionFormat(val) {
        return <TextOverlay data={val} numberOfChar={30} />;
    }

    ///<summary>
    ///This method returns all destinations that mapped to a product to display in the grid.
    ///</summary>
    destinationFormat(val) {
        var destinationNames = [];
        var rows = [];

        for (var idx = 0; idx < val.length; idx++) {
            destinationNames.push(val[idx]);
        }
        //destination names are sorted before rendering 
        if (destinationNames.length > 0) {
            destinationNames.sort();
            for (var idx = 0; idx < destinationNames.length; idx++)
                rows.push(<Button className="addMarginRight" key={idx.toString()}> {destinationNames[idx]} </Button>);
        }

        return <DestinationOverlay rows={rows} numberOfDestinations={2} />;
    }

    ///<summary>
    ///This method returns all tags to display in the grid.
    ///</summary>
    tagFormat(val) {
        var tags = [];
        for (var i = 0; i < val.length; i++) {
            tags.push(<Button className="addMarginRight" key={i.toString()}> {val[i].name} </Button>);
        }
        return <div> {tags} </div>;
    }

    ///<summary>
    ///This method construct the edit and delete action buttons
    ///</summary>
    actionFormat(val, rowData) {
            var readOrEditButton = null;
            if (this.state.permissions.canEdit) {
                readOrEditButton = <button class="btn-link" title="Edit Product" onClick={(event) => this.openAddEditModel(rowData, event)} >
                                        <i class="fa fa-pencil-square-o"></i>
                                    </button>;
            }
            else if (this.state.permissions.canRead) {
                readOrEditButton = <button class="btn-link" title="View Product" onClick={(event) => this.openAddEditModel(rowData, event)} >
                                        <i class="fa fa-book"></i>
                                    </button>;
            }

            var deleteButton = null;

            if (this.state.permissions.canDelete) {
                deleteButton = <button class="btn-link" title="Delete Product" onClick={(event) => this.openDeleteModel(rowData, event)} >
                                    <i class="fa fa-trash"></i>
                                </button>;
            }

            return (
                <div>
                    {readOrEditButton}
                    {deleteButton}
                </div>
            );
    }

    render() {

        var permissions = this.state.permissions;
        var addButton = null;

        if (this.state.permissions.canAdd) {
            addButton = <div>
                <button class="btn-link pull-right addMarginRight" title="New Product" onClick={(event) => this.openCreateNewDestinationModel(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New Product</span>
                </button>
            </div>;
        }

        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Prodduct") {
                return <TableHeaderColumn width="30%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.productNameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Description") {
                return <TableHeaderColumn width="28%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.descriptionFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Destinations") {
                return <TableHeaderColumn width="17%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.destinationFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Tags") {
                return <TableHeaderColumn width="15%" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.tagFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="10%" expandable={false} dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} >{item.label}</TableHeaderColumn>
            }

        }.bind(this));

        return (
            <div>
                {addButton}
                <BootstrapTable ref="productTable"
                    data={this.props.RowData}
                    striped={true}
                    hover={true}
                    keyField={this.props.KeyField}
                    pagination={true}
                    options={this.state.options}>
                    {row}
                </BootstrapTable>
                <AddEditProduct permissions={this.state.permissions} data={this.state} handleClose={this.closeAddEditModel.bind(this)} />
                <RemoveProductModal data={this.state} handleClose={this.closeDeleteModel.bind(this)} />
            </div>)
    }

}

export default ProductTable;

