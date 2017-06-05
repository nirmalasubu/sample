import React from 'react';
import PageHeader from 'Components/Common/PageHeader';

class AddEditDestinationProperties extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {

  }

  textFormat(val, row) {
    return <div>{val}</div>
  }

  actionFormat(val, rowData) {
    return (
      <div>
        <button class="btn-link" title="Edit Properties" >
          <i class="fa fa-pencil-square-o"></i>
        </button>

        <button class="btn-link" title="Delete Properties" >
          <i class="fa fa-trash"></i>
        </button>
      </div>
    );
  }

  render() {
    return (
      <div>
        <BootstrapTable data={this.props.data.properties} striped hover>
          <TableHeaderColumn isKey dataField="name" dataSort={true} dataFormat={this.textFormat.bind(this)}>Name</TableHeaderColumn>
          <TableHeaderColumn dataField="value" dataSort={true} dataFormat={this.textFormat.bind(this)}>Value</TableHeaderColumn>
          <TableHeaderColumn width="100px" dataField="value" dataSort={false} dataFormat={this.actionFormat.bind(this)}>Actions</TableHeaderColumn>
        </BootstrapTable>
      </div>
    )
  }
}

export default AddEditDestinationProperties