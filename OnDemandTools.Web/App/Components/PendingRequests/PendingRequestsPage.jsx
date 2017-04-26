import React from 'react';

class PendingRequests extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {
    document.title = "ODT - Pending Requests";
  }

  render() {
    return (
      <div>
        <h1>Pending Requests Page</h1>
        <p>Under construction</p>
      </div>
    )
  }
}

export default PendingRequests