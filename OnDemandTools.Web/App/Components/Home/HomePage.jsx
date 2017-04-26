import React from 'react';

class Home extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {
    document.title = "ODT - Home";
  }

  render() {
    return (
      <div>
        <h1>Home Page</h1>
        <p>Under construction</p>
      </div>
    )
  }
}

export default Home