import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
class AddEditDestinationDeliverables extends React.Component {

  constructor(props) {
    super(props);
  }

  componentDidMount() {

  }
  textFormat(val, row) {
    return <div>{val}</div>
  }

  render() {
    return (
      <div>
        <BootstrapTable data={this.props.data.deliverables} striped hover>
          <TableHeaderColumn isKey dataField="value" dataSort={true} dataFormat={this.textFormat.bind(this)}>Value</TableHeaderColumn>
        </BootstrapTable>
      </div>
    )
  }
}

export default AddEditDestinationDeliverables