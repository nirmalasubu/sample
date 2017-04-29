import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import $ from 'jquery';
require('react-bootstrap-table/css/react-bootstrap-table.css');

var ReactTable = React.createClass({
    actionFormat: function() {
        return '<i class="fa fa-search" aria-hidden="true"></i> <i class="fa fa-calendar" aria-hidden="true"></i>'
    },
    queueFormat: function(val) {
        return   '<p>' +val+ '<br /> <br /><i>Delivery</i>: 0<button class="btn-xs btn-link" title="clear pending deliveries to queue">Clear</button><button class="btn-xs btn-link" title="clear pending deliveries to queue">Resend</button><br /><i>Consumption</i>: 21<button class="btn-xs btn-link">Purge</button><br/><span class="small">-Apr 25, 2017 2:24 PM</span></p>'
    },
    contactFormat:function(val)
    {
        return '<p data-toggle="tooltip" title="'+val +'">'+val +'</p>'
    },
    render: function() {
        var row;
        row = this.props.ColumnData.map(function(item, index) {

            if(item.label == "Remote Queue"){
                return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.queueFormat}>{item.label}</TableHeaderColumn>
            }
            else if(item.label == "Actions"){
                return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.actionFormat}>{item.label}</TableHeaderColumn>
            }
            else if(item.label=="Contact")
            {
                return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort} dataFormat={this.contactFormat}>{item.label}</TableHeaderColumn>
            }
            else{
               return  <TableHeaderColumn dataField={item.dataField} key={index++} dataSort={item.sort}>{item.label}</TableHeaderColumn>
            }
            
            }.bind(this));

        return (
              <BootstrapTable data={this.props.RowData} striped={true} hover={true} keyField={this.props.KeyField} pagination={true}>
                 {row}
                </BootstrapTable>
                  )
        }
});

export default ReactTable;