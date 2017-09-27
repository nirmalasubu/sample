import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
import { connect } from 'react-redux';
import { Button } from 'react-bootstrap';
import CategoryExpandTable from 'Components/Categories/CategoryExpandTable';
import AddEditCategory from 'Components/Categories/AddEditCategory';
require('react-bootstrap-table/css/react-bootstrap-table.css');
import RemoveCategoryModal from 'Components/Categories/RemoveCategoryModal';
import { getNewCategory } from 'Actions/Category/CategoryActions';
import TextButtons from 'Components/Common/TextButtons';
import * as categoryActions from 'Actions/Category/CategoryActions';
import TextOverlay from 'Components/Common/TextOverlay';


@connect((store) => {
    return {
        categories: store.categories,
        user: store.user
    };
})

class CategoryTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            permissions: { canAdd: false, canRead: false, canEdit: false, canAddOrEdit: false, disableControl: true },
            newCategoryModel: {},
            showModal: false,
            showAddEditModel: false,
            showDeleteModal: false,
            categoryDetails: "",
            options: {
                onRowClick: this.onRowClick.bind(this),
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
                }],
                onSortChange: this.onSortChange.bind(this)
            }
        }
    }

    componentDidMount() {
        let promise = getNewCategory();
        promise.then(message => {
            this.setState({
                newCategoryModel: message
            });
        }).catch(error => {
            this.setState({
                newCategoryModel: {}
            });
        });

        if (this.props.user && this.props.user.portal) {
            this.setState({ permissions: this.props.user.portal.modulePermissions.Categories })
        }
    }

    //receives prop changes to update state
    componentWillReceiveProps(nextProps) {
        if (nextProps.user && nextProps.user.portal) {
            this.setState({ permissions: nextProps.user.portal.modulePermissions.Categories });
        }
    }

    ///<summary>
    // On row click invoke the action to make the property clicked true corresponding row
    ///</summary>
    onRowClick(row) {
        this.props.dispatch(categoryActions.categoryExpandRowClickSuccess(row.id));
    }
    ///<summary>
    /// on clicking sort arrow in any page of the table should take to the First page in the pagination.
    ///</summary>
    onSortChange() {
        const sizePerPage = this.refs.categoryTable.state.sizePerPage;
        this.refs.categoryTable.handlePaginationData(1, sizePerPage);
    }

    openCreateNewDestinationModel() {
        this.setState({ showAddEditModel: true, categoryDetails: jQuery.extend(true, {}, this.state.newCategoryModel) });
    }
    ///<summary>
    // when delete category button event handled
    ///</summary>
    openDeleteModel(val) {
        this.setState({ showDeleteModal: true, categoryDetails: val });
    }

    ///<summary>
    // React modal pop up control   delete category is closed
    ///</summary>
    closeDeleteModel() {
        this.setState({ showDeleteModal: false });
    }

    openAddEditModel(val) {
        this.setState({ showAddEditModel: true, categoryDetails: val });
    }

    closeAddEditModel() {
        this.setState({ showAddEditModel: false, categoryDetails: this.state.newCategoryModel });
    }

    descriptionFormat(val) {
    }

    isExpandableRow(row) {
        if (row.destinations.length > 0) return true;
        else return false;
    }

    expandComponent(row) {
        return (
            <CategoryExpandTable data={row} />
        );
    }

    categoryNameFormat(val, rowData) {
            return <TextOverlay data={val} numberOfChar={60} />;
    }

    destinationFormat(val, rowData) {
        var destinationNames = [];
        var rows = [];
        for (var idx = 0; idx < rowData.destinations.length; idx++) {
                destinationNames.push(rowData.destinations[idx].name);
         }
            //destination names are sorted before rendering 
            if (destinationNames.length > 0) {
                destinationNames.sort();
              }

        return <TextButtons data={destinationNames} numberOfCharToDisplay={9} title={"Click to view more destinations"} />

    }

    actionFormat(val, rowData) {
        var readOrEditButton = null;
        if (this.state.permissions.canEdit) {
            readOrEditButton = <button class="btn-link" title="Edit Category" onClick={(event) => this.openAddEditModel(rowData, event)} >
                <i class="fa fa-pencil-square-o"></i>
            </button>;
        }
        else if (this.state.permissions.canRead) {
            readOrEditButton = <button class="btn-link" title="View Category" onClick={(event) => this.openAddEditModel(rowData, event)} >
                <i class="fa fa-book"></i>
            </button>;
        }

        var deleteButton = null;

        if (this.state.permissions.canDelete) {
            deleteButton = <button class="btn-link" title="Delete Category" onClick={(event) => this.openDeleteModel(rowData, event)} >
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
                <button class="btn-link pull-right addMarginRight" title="New Category" onClick={(event) => this.openCreateNewDestinationModel(event)}>
                    <i class="fa fa-plus-square fa-2x"></i>
                    <span class="addVertialAlign"> New Category</span>
                </button>
            </div>;
        }

        var row;
        row = this.props.ColumnData.map(function (item, index) {

            if (item.label == "Name" ) {
                return <TableHeaderColumn  width="600px" dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.categoryNameFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Destinations") {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.destinationFormat.bind(this)} >{item.label}</TableHeaderColumn>
            }
            else if (item.label == "Actions") {
                return <TableHeaderColumn width="100px" expandable={false} dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat.bind(this)}>{item.label}</TableHeaderColumn>
            }
            else {
                return <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} >{item.label}</TableHeaderColumn>
            }

        }.bind(this));

        return (
            <div>
                {addButton}
                <BootstrapTable ref="categoryTable"
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

                <AddEditCategory data={this.state} permissions={this.state.permissions} handleClose={this.closeAddEditModel.bind(this)} />
                <RemoveCategoryModal data={this.state} handleClose={this.closeDeleteModel.bind(this)} />
            </div>)
    }

}

export default CategoryTable;

