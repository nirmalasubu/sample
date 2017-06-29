import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import TitleNameOverlay from 'Components/Common/TitleNameOverlay';
import BrandImage from 'Components/Common/BrandImage';

class CategoryExpandTable extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {

  }

  stringColumnFormat(data, row) {
    return <p> {data} </p>;
  }

  brandColumnFormat(data, row) {
    var brandImageRows = [];
    for (var i = 0; i < row.categories[0].brands.length; i++) {
      brandImageRows.push(<BrandImage key={i.toString()} brandName={row.categories[0].brands[i]} />);
    }

    return <div> {brandImageRows} </div>
  }

  titleOrSeriesFormat(data, row) {
    var ids = [];

    for (var i = 0; i < row.categories[0].seriesIds.length; i++) {
      ids.push(row.categories[0].seriesIds[i])
    }

    for (var i = 0; i < row.categories[0].titleIds.length; i++) {
      ids.push(row.categories[0].titleIds[i])
    }

    return <TitleNameOverlay data={ids}/> 
  }



  render() {

    const options = {
      expandRowBgColor: 'rgb(242, 255, 163)',
      expandBy: 'column'  // Currently, available value is row and column, default is row
    };
    return (
      <div>
        <BootstrapTable options={options} data={this.props.data.destinations} striped hover>
          <TableHeaderColumn isKey dataSort={true} dataField="name" dataFormat={this.stringColumnFormat.bind(this)} >Destination</TableHeaderColumn>
          <TableHeaderColumn dataSort={false} dataField="name" dataFormat={this.brandColumnFormat.bind(this)} >Brands</TableHeaderColumn>
          <TableHeaderColumn dataSort={false} dataField="name" dataFormat={this.titleOrSeriesFormat.bind(this)} >Title/Series</TableHeaderColumn>
        </BootstrapTable>
      </div>
    )
  }
}

export default CategoryExpandTable