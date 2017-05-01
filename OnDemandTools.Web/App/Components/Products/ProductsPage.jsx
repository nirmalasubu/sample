import React from 'react';
import PageHeader from 'Components/Common/PageHeader';

class Products extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {
    document.title = "ODT - Products";
  }

  render() {
    return (
      <div>
       <PageHeader pageName="Products"/>
        <p>Under construction</p>
      </div>
    )
  }
}

export default Products