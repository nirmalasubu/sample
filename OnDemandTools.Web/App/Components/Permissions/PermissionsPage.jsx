import React from 'react';
import PageHeader from 'Components/Common/PageHeader';

class Permissions extends React.Component {

  constructor(props) {
    super(props);
  }
  
  componentDidMount() {
    document.title = "ODT - Permissions";
  }

  render() {
    return (
      <div>
        <PageHeader pageName="Permissions"/>
        <p>Under construction</p>
      </div>
    )
  }
}

export default Permissions