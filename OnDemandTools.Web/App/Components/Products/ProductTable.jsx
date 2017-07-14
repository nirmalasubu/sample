import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Button } from 'react-bootstrap';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import RemoveCategoryModal from 'Components/Categories/RemoveCategoryModal';
import { getNewCategory } from 'Actions/Category/CategoryActions';


class ProductTable extends React.Component {
    ///<summary>
    /// Define default component state information. This will
    /// get modified further based on how the user interacts with it
    ///</summary>
    constructor(props) {
        super(props);
        this.state = {
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
                }]
            }
        }
    }

    ///<summary>
    ///Called on component load
    ///</summary>
    componentDidMount() {
        //let promise = getNewCategory();
        //promise.then(message => {
        //    this.setState({
        //        newCategoryModel: message
        //    });
        //}).catch(error => {
        //    this.setState({
        //        newCategoryModel: {}
        //    });
        //});
    }

    ///<summary>
    ///This is to open a modal popup to add a new product.
    ///</summary>
    openCreateNewDestinationModel() {
        this.setState({ showAddEditModel: true, categoryDetails: jQuery.extend(true, {}, this.state.newProductModel) });
    }
    ///<summary>
    // when delete category button event handled
    ///</summary>
    openDeleteModel(val) {
        this.setState({ showDeleteModal: true, productDetails: val });
    }

    ///<summary>
    // React modal pop up control   delete category is closed
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
        return '<p data-toggle="tooltip" class="shortDesc">' + val + '</p>';
    }

    ///<summary>
    ///This method returns all destinations that mapped to a product to display in the grid.
    ///</summary>
    destinationFormat(val) {
        return '<p data-toggle="tooltip">' + val.toString() + '</p>';
    }

    ///<summary>
    ///This method returns all tags to display in the grid.
    ///</summary>
    tagFormat(val) {
        console.log(val);
        var tags = [];
        for(var i=0; i < val.length; i++)
        {
            tags.push(val[i].text);
        }
        return '<p data-toggle="tooltip">' + tags.toString() + '</p>';        
    }

    ///<summary>
    ///This method construct the edit and delete action buttons
    ///</summary>
    actionFormat(val, rowData) {
        return (
            <div>
                <button class="btn-link" title="Edit Category" onClick={(event) => this.openAddEditModel(rowData, event)} >
                    <i class="fa fa-pencil-square-o"></i>
                </button>

                <button class="btn-link" title="Delete Category" onClick={(event) => this.openDeleteModel(rowData, event)} >
                    <i class="fa fa-trash"></i>
                </button>
            </div>
        );
    }

    render() {

        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Prodduct") {
                return <TableHeaderColumn width="250px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.productNameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Description") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.descriptionFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Destinations") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.destinationFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Tags") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.tagFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="100px" expandable={ false } dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} >{item.label}</TableHeaderColumn>
            }

        }.bind(this));
            
        return (
            <div>
                <div>
                    <button class="btn-link pull-right addMarginRight" title="New Product" onClick={(event) => this.openCreateNewDestinationModel(event)}>
                        <i class="fa fa-plus-square fa-2x"></i>
                        <span class="addVertialAlign"> New Product</span>
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
            </div>)
                        }

}

export default ProductTable;

