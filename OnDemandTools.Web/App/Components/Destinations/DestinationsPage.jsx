import React from 'react';
import PageHeader from 'Components/Common/PageHeader';

class Destinations extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {
    document.title = "ODT - Destinations";
  }

  render() {
    return (
      <div>
        <PageHeader pageName="Destinations"/>
        <p>Under construction</p>
      </div>
    )
  }
}

export default Destinations