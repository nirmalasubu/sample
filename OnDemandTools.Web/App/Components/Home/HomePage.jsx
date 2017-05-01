import React from 'react';
import PageHeader from 'Components/Common/PageHeader';

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
        <PageHeader pageName="Home"/>
        <p>Under construction</p>
      </div>
    )
  }
}

export default Home