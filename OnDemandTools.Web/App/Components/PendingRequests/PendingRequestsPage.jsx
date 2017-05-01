import React from 'react';
import PageHeader from 'Components/Common/PageHeader';

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
        <PageHeader pageName="Pending Requests"/>
        <p>Under construction</p>
      </div>
    )
  }
}

export default PendingRequests