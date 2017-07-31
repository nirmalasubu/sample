import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandsOverlay from 'Components/Common/BrandsOverlay';

class ContentTierExpandTable extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {

  }

  stringColumnFormat(data, row) {
    return <p> {data} </p>;
  }

  brandColumnFormat(data, row) {
    return <BrandsOverlay data={row.contentTiers[0].brands} />
  }

  titleOrSeriesFormat(data, row) {
    var ids = [];

    for (var i = 0; i < row.contentTiers[0].seriesIds.length; i++) {
      ids.push(row.contentTiers[0].seriesIds[i])
    }

    for (var i = 0; i < row.contentTiers[0].titleIds.length; i++) {
      ids.push(row.contentTiers[0].titleIds[i])
    }

    return <TitleNameOverlay data={ids} />
  }

  descriptionColumnFormat(data, row) {
      return <p> {data} </p>;
      }

  render() {

      const options = {
       defaultSortName: 'name',
       defaultSortOrder: 'asc',
      expandRowBgColor: 'rgb(242, 255, 163)',
      expandBy: 'column'  // Currently, available value is row and column, default is row
    };
    return (
      <div>
        <BootstrapTable options={options} data={this.props.data.products} striped hover>
          <TableHeaderColumn isKey dataSort={true} dataField="name" dataFormat={this.stringColumnFormat.bind(this)} >Product</TableHeaderColumn>
          <TableHeaderColumn  dataSort={false} dataField="description" dataFormat={this.descriptionColumnFormat.bind(this)} >Description</TableHeaderColumn>
          <TableHeaderColumn width="200px" dataSort={false} dataField="name" dataFormat={this.brandColumnFormat.bind(this)} >Brands</TableHeaderColumn>
          <TableHeaderColumn dataSort={false} dataField="name" dataFormat={this.titleOrSeriesFormat.bind(this)} >Title/Series</TableHeaderColumn>
        </BootstrapTable>
      </div>
    )
  }
}

export default ContentTierExpandTable