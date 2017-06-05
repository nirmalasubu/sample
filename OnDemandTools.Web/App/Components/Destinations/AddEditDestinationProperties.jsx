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

  render() {
    return (
      <div>
        <BootstrapTable data={this.props.data.properties} striped hover>
          <TableHeaderColumn isKey dataField="name" dataSort={true} dataFormat={this.textFormat.bind(this)}>Name</TableHeaderColumn>
          <TableHeaderColumn dataField="value" dataSort={true} dataFormat={this.textFormat.bind(this)}>Value</TableHeaderColumn>
        </BootstrapTable>
      </div>
    )
  }
}

export default AddEditDestinationProperties