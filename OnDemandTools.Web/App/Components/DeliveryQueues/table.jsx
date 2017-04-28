import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
require('react-bootstrap-table/css/react-bootstrap-table.css');

var ReactTable = React.createClass({
    render: function() {
        return (
              <BootstrapTable data={this.props.RowData} striped={true} hover={true} keyField={"friendlyName"} pagination={true}>
                  {this.props.ColumnData.map((item,index)=>
                      <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort}>{item.label}</TableHeaderColumn>
                    )} 
                </BootstrapTable>
                  )
        }
});

export default ReactTable;