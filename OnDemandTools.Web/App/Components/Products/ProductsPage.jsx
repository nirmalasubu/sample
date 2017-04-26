import React from 'react';

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
        <h1>Products Page</h1>
        <p>Under construction</p>
      </div>
    )
  }
}

export default Products